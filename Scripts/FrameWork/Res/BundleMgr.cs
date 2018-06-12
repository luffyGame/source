using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Text;
using UObj = UnityEngine.Object;

namespace FrameWork
{
	public class BundleCfg
	{
		public static readonly string relativePath = "ArtRes/";
        public static readonly string allDepFile = "all.info";
        public static readonly string[] manifests = new string[]{
            "ArtRes/Model/Avatar/Avatar"
        };
	}

	public class BundleLoadProgress
	{
		public List<BundleHolder> allNeedBundles = null;

		private float GetLoadingProgress()
		{
			if(allNeedBundles == null||allNeedBundles.Count == 0)
				return 1f;
			float val = 0f;
			for(int i=0;i<allNeedBundles.Count;++i)
			{
				val += allNeedBundles[i].SelfLoadingProgress;
			}
			return val/(float)allNeedBundles.Count;
		}
	}
    public class BundleMgr
	{
		#region Variables
		private Dictionary<string,BundleHolder> bundles = new Dictionary<string, BundleHolder>();
		//等待删除队列，根据没有ref的时间排序，越久没用的优先删除，它里面存在的肯定是bundles里有的，
		//也就是说没有真的删除的肯定还在bundles里，基于bundle如果是在内存里unity并不会重新加载的原则
		private List<BundleHolder> waitingDelBundles = new List<BundleHolder>();
		private BundleDepMap depMap = new BundleDepMap();
		private BundleLoader loader = null;
		private long loadedBundleSize = 0;
		private float gcDeltaTime = 0f;
		private bool isInLevel = false;//在场景中
		private static BundleMgr s_instance = new BundleMgr();
		private const float DEL_TIME = 15f;//无用资源的存在时间超过这个时间的才删除，这个时间应该超过检查的间隔
		private const int ZERO_REF_COUNT = 0;//超过这个数量，要不要数量排除？，如果不为0，应该保证依赖其他人的删除优先，跟依赖加载的顺序正好相反
		private const float GC_PERIOD = 1f;//gc周期
		private const long GC_THRESHOLD = 2048;//2M
		#endregion
		#region Properties
		public static BundleMgr Instance
		{
			get {return s_instance;}
		}
		#endregion
		#region Public Method
		public void ResetDep()
		{
            depMap.Init(BundleCfg.allDepFile);
		}
		public void ResetDep(List<string> deps)
		{
            depMap.Init(deps);
		}
		public void AddLoadedBundleSize(long size)
		{
			loadedBundleSize += size;
		}
		//获得holder
		public BundleHolder GetHolder(string bundlePath,bool isManifest = false)
		{
			if(bundles.ContainsKey(bundlePath))
				return bundles[bundlePath];
			else
			{
				BundleInfo bi = depMap.GetInfo(bundlePath,isManifest);
				if(null!=bi)
				{
                    BundleHolder holder = BundleHolder.Gen(bi, this);
					bundles.Add(bundlePath,holder);
					return holder;
				}
				else
					return null;
			}
		}
        //通过asset路径获取asset，cb是在获取后的回调，返回值是一个索引，用于取消获取
		public ulong GetAsset(string assetPath,OnAssetGot cb,bool isManifest = false)
		{
			BundleHolder holder = this.GetHolder(assetPath,isManifest);
            if (null != holder)
            {
                return holder.RefBy(cb);
            }
            else
            {
	            cb(null, 0);
            }
            return 0;
        }
        public UObj GetAssetSync(string assetPath)
        {
            BundleHolder holder = this.GetHolder(assetPath, false);
            if (null != holder)
            {
                if(!holder.IsLoaded())
                    holder.LoadSync();
                return holder.MyAsset;
            }
            return null;
        }
        public ulong GetManifest(string assetPath, OnAssetGot cb)
        {
            return GetAsset(assetPath, cb);
        }
		//直接获取已获得的资源，这种直接设置的情况，只限于它的引用关系已经根据bundle文件添加过了
		//对于程序手动设置的，即在打包的时候并不存在这种依赖关系的，切勿用此接口，注意！！！
		public UnityEngine.Object GetExistAsset(string assetPath)
		{
			BundleHolder holder = this.GetExistHolder(assetPath);
			if(null!=holder)
			{
				return holder.MyAsset;
			}
			return null;
		}
		public void RefAsset(string assetPath)
		{
			BundleHolder holder = this.GetExistHolder(assetPath);
			if(null!=holder)
				holder.Ref();
		}
		public void ReleaseAsset(string assetPath,bool delNow)
		{
			if(null == assetPath)
				return;
			BundleHolder holder = GetExistHolder(assetPath);
			if(null!=holder)
				holder.UnRef(delNow);
		}
        public void ReleaseAsset(string assetPath, bool delNow, int releasCount)
        {
            BundleHolder holder = GetExistHolder(assetPath);
            if (null != holder)
            {
                for (int i = 0; i < releasCount; ++i)
                    holder.UnRef(delNow);
            }
        }
		public void CancelUngotAsset(ulong cbIdx)
		{
            AssetCallback acb = AssetCallback.Get(cbIdx);
            if (null == acb)
                return;
			BundleHolder holder = GetExistHolder(acb.Path);
			if(null!=holder)
				holder.UnRefBy(cbIdx);
		}
		public void RemoveHolder(string assetPath)
		{
			if(bundles.ContainsKey(assetPath))
				bundles.Remove(assetPath);
		}
		public BundleHolder GetExistHolder(string bundlePath)
		{
			if(bundles.ContainsKey(bundlePath))
			   return bundles[bundlePath];
			else
				return null;
		}
		//尝试少量的删除
		public IEnumerator TryDelete()
		{
			yield return null;
			float curTime = Time.realtimeSinceStartup;
			while(waitingDelBundles.Count>ZERO_REF_COUNT)
			{
				BundleHolder one = waitingDelBundles[0];
				if(one.RefCount == 0 && curTime - one.NoRefTime > DEL_TIME) //它依赖的holder的时间肯定比它早
				{
					one.Unload();
					waitingDelBundles.RemoveAt(0);
				}
				else
					break;//排在前面的时间最久
			}
			//_Scripts.Main.App.Log(string.Format("bundle count = {0}",bundles.Count));
		}
		//一次清除所有没有引用的bundle
		public void DeleteNoRefBundles()
		{
			for(int i=0;i<waitingDelBundles.Count;++i)
			{
				BundleHolder one = waitingDelBundles[i];
				one.Unload();
			}
			waitingDelBundles.Clear();
		}
		public void AddDelete(BundleHolder holder)
		{
			waitingDelBundles.Add(holder);
		}
		public void NoDelete(BundleHolder holder)
		{
			int index = waitingDelBundles.IndexOf(holder);
			if(index>=0)
				waitingDelBundles.RemoveAt(index);
		}
		public void Update(float timeDelta)
		{
			gcDeltaTime += timeDelta;
			if(gcDeltaTime >= GC_PERIOD)
			{
				gcDeltaTime = 0f;
				if(isInLevel)
				{
					if(loadedBundleSize>=GC_THRESHOLD)
						Gc();
				}
			}
		}
		public void GcByLevel(bool isLevIn)
		{
			isInLevel = isLevIn;
			Gc();
		}
		#endregion
		#region Private Method
		private void Gc()
		{
			GC.Collect();
			loadedBundleSize = 0;
		}
		#endregion
	}
}
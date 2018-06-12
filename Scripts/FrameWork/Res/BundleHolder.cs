using UnityEngine;
using System.Collections;
using UObj = UnityEngine.Object;
using System.Collections.Generic;
using System.IO;
using System;
using FrameWork.BaseUtil;

namespace FrameWork
{
	public abstract class BundleHolder
	{
		#region Variables
		private static readonly int MAX_RETRY_TIMES = 5;//用于限制失败重试加载的次数
		protected BundleInfo info = null;
		protected AssetBundle bundle = null;
		
		private BundleMgr mgr = null;

		private int count = 0;
		private int retryTimes = 0;
		private float noRefTime = 0f;//没有引用的时间

        private List<AssetCallback> cbs = new List<AssetCallback>();
		private bool isLoading = false;
		private BundleLoadTask loadTask = null;
        private int waitDependNum;
        private Dictionary<ulong,string> waitDependCbIds = null;
		#endregion
		#region Properties
        public virtual UObj MyAsset
        {
            get { return null; }
        }
        public virtual bool IsHoldOneAsset
        {
            get { return true; }
        }
		public BundleInfo Info {get {return info;}}
		public AssetBundle MyBundle 
		{
			set 
			{	
				bundle = value;
				//if(null!=bundle)
				//	UObj.DontDestroyOnLoad(bundle);//它的删除自己管理
			}
			get {return bundle;}
		}
		public float SelfLoadingProgress
		{
			get
			{
				if(this.IsLoaded())
					return 1f;
				if(null != this.loadTask)
					return loadTask.GetProgress();
				return 0f;
			}
		}
		public float NoRefTime
		{
			get {return noRefTime;}
		}
		public int RefCount {get {return count;}}
		#endregion
		#region Private Method
		private void StartLoad()
		{
			//Debugger.Log("start loading {0}",info.path);
			retryTimes = 0;
			isLoading = true;
            waitDependNum = null == info.depends ? 0 : info.depends.Length;
            if(waitDependNum>0)
            {
                waitDependCbIds = new Dictionary<ulong, string>();
                for(int i=0,iCount = waitDependNum;i<iCount;++i)
                {
                    ulong cbId = mgr.GetAsset(info.depends[i], OnDependAssetGot);
                    if (cbId > 0)
                        waitDependCbIds.Add(cbId,info.depends[i]);
                }
            }
			else
			    TryLoadSelf();
		}
        private void OnDependAssetGot(System.Object asset,ulong cbId)
        {
            if(cbId>0&&null!=waitDependCbIds)
                waitDependCbIds.Remove(cbId);
            --waitDependNum;
            if (waitDependNum == 0)
            {
                waitDependCbIds = null;
                TryLoadSelf();
            }
        }
		private void TryLoadSelf()
		{
			if(this.IsLoaded()||null!=loadTask)//有可能出现，因为通过depend的回调有可能先执行到这，然后在StartLoaded执行到这
				return;
			loadTask = new BundleLoadTask(this);
		}
		#endregion
		#region Public Method
        public static BundleHolder Gen(BundleInfo bi,BundleMgr _mgr)
        {
            if (bi.type == AssetType.MULTI_ASSETS)
                return new BundleHolderMulti(bi, _mgr);
            return new BundleHolderOne(bi, _mgr);
        }
		public BundleHolder(BundleInfo bi,BundleMgr _mgr)
		{
			info = bi;
			mgr = _mgr;
		}

		public abstract bool IsLoaded();

		public void Ref()
		{
			if(count == 0)
				mgr.NoDelete(this);
			++count;
			if(this.IsLoaded()||this.isLoading)
			{
				if(null!=info.depends)
				{
					for(int i=0;i<info.depends.Length;++i)
						mgr.RefAsset(info.depends[i]);
				}
			}
			if(this.IsLoaded())
				this.OnLoaded();
			else if(!this.isLoading)
				this.StartLoad();
			//Debugger.Log("ref bundle = {0},count = {1}",info.path,count);
		}
		public ulong RefBy(OnAssetGot cb)
		{
            if (this.IsLoaded())
            {
	            Ref();
	            DoCallback(cb);
                return 0;
            }
            else
            {
                AssetCallback acb = AssetCallback.Gen(cb,info.path);
                cbs.Add(acb);
                Ref();
                return acb.Id;
            }
		}
		public void UnRefBy(ulong cbId)
		{
            int ret = -1;
            for (int i = 0; i < cbs.Count;++i )
            {
                if(cbs[i].Id == cbId)
                {
                    ret = i;
                    break;
                }
            }
            if(ret>=0)
            {
                AssetCallback.Remove(cbs[ret].Id);
                cbs.RemoveAt(ret);
                UnRef(true);
            }
		}
		public void UnRef(bool delNow)
		{
			if(count <= 0)
				return;
			--count;
			//Debugger.Log("unref bundle = {0},count = {1}",info.path,count);
			if(null!=info.depends)
			{
                List<string> unDone = null;
                if(null!=waitDependCbIds)
                {
                    unDone = new List<string>();
                    Dictionary<ulong,string>.Enumerator it = waitDependCbIds.GetEnumerator();
                    while(it.MoveNext())
                    {
                        unDone.Add(it.Current.Value);
                        mgr.CancelUngotAsset(it.Current.Key);
                    }
                }
				for(int i=0;i<info.depends.Length;++i)
                {
                    if(null==unDone||!unDone.Contains(info.depends[i]))
                        mgr.ReleaseAsset(info.depends[i], delNow);
                }
			}
			if(0==count)
			{
				noRefTime = Time.realtimeSinceStartup;
				if(delNow)
					Unload();
				else
				{
					//Debugger.Log("unref to 0 bundle {0}",info.path);
					mgr.AddDelete(this);
				}
			}
		}
		public void Unload()
		{
			//Debugger.Log("unload bundle {0}",info.path);
			if(this.isLoading)
			{
				if(null!=loadTask)
				{
					loadTask.Cancel();
					loadTask = null;
				}
				this.isLoading = false;
			}
			else
			{
                ReleaseAsset();
				if(null!=bundle)
				{
					bundle.Unload(true);
					UObj.DestroyImmediate(bundle,true);
					bundle = null;
				}
			}
			mgr.RemoveHolder(info.path);
		}
		public void OnLoaded()
		{
			int num = cbs.Count;
			while(num>0)
			{
                AssetCallback acb = cbs[num - 1];
                AssetCallback.Remove(acb.Id);
				cbs.RemoveAt(num-1);
				DoAssetCallback(acb);
				num = cbs.Count;
			}
		}
		/// <summary>
		/// 获取所有相关的holder，注意并不ref
		/// </summary>
		public void GetAllRelativeHolders(Dictionary<string,BundleHolder> holders)
		{
			if(!holders.ContainsKey(info.path))
				holders[info.path] = this;
			if(null!=info.depends)
			{
				for(int i=0;i<info.depends.Length;++i)
				{
					BundleHolder holder = mgr.GetHolder(info.depends[i]);
					if(null!=holder)
						holder.GetAllRelativeHolders(holders);
				}
			}
		}
		protected void OnAssetSet(bool retry = false)
		{
			loadTask = null;//清除任务
			bool needRetry = false;
			if(IsLoaded())
			{
				//UObj.DontDestroyOnLoad(asset);//它的删除自己管理
				if(retryTimes>0)
					Debugger.LogWarning("{0} times load {1} success",retryTimes,info.path);
			}
			else
			{
				Debugger.LogWarning("warning: {0} asset load null",info.path);
				if(retry&&retryTimes<=MAX_RETRY_TIMES)
					needRetry = true;
			}
			if(needRetry)
			{
				if(null!=bundle)
				{
					bundle.Unload(true);
					UObj.DestroyImmediate(bundle,true);
					bundle = null;
				}
				++retryTimes;
				TryLoadSelf();
			}
			else
			{
				if(null!=bundle)
				{
                    if (IsLoaded())
                    {
	                    if (IsHoldOneAsset&&info.BundleShouldUnload)
	                    {
		                    BundleLoader.Instance.StartCoroutine(Utils.DelayAction(DelayUnload,3));
	                    }
                    }
                    else
                    {
                        bundle.Unload(true);
                        UObj.DestroyImmediate(bundle, true);
                        bundle = null;
                    }
				}
				isLoading = false;
				OnLoaded();//如果asset为空，意味着彻底加载失败，通知客户端处理
			}
		}
		private void DelayUnload()
		{
			bundle.Unload(false);
		}

		protected virtual void DoAssetCallback(AssetCallback acb)
		{
			acb.Do(MyAsset);
		}

		protected virtual void DoCallback(OnAssetGot cb)
		{
			cb(MyAsset, 0);
		}
        protected abstract void ReleaseAsset();
        public virtual void LoadSync()
        {
            string bundlePath = FileUtility.GetFileReadFullPath(string.Format("{0}{1}", BundleCfg.relativePath, info.path));
            bundle = AssetBundle.LoadFromFile(bundlePath);
        }
		#endregion
	}

    public class BundleHolderOne : BundleHolder
    {
        public override UObj MyAsset
        {
            get
            {
                return asset;
            }
        }
        private UObj asset = null;
        public BundleHolderOne(BundleInfo bi, BundleMgr _mgr) : base(bi, _mgr) { }
        
        public override bool IsLoaded()
        {
            return asset != null;
        }
        protected override void ReleaseAsset()
        {
            if (null != asset)
            {
                UObj.DestroyImmediate(asset, true);
                asset = null;
            }
        }
        public void SetAsset(UObj _asset,bool retry = false)
        {
	        asset = _asset;
            this.OnAssetSet(retry);
        }
        public override void LoadSync()
        {
            base.LoadSync();
            UObj uo = bundle.LoadAsset(info.mainName, info.GetAssetType());
            SetAsset(uo, true);
        }
    }

    public class BundleHolderMulti:BundleHolder
    {
        private UObj[] assets = null;
        public override bool IsHoldOneAsset
        {
            get
            {
                return false;
            }
        }
        public BundleHolderMulti(BundleInfo bi, BundleMgr _mgr) : base(bi, _mgr) { }
        
        public override bool IsLoaded()
        {
            return assets != null;
        }
        protected override void ReleaseAsset()
        {
            if (null != assets)
            {
                for (int i = 0; i < assets.Length; ++i)
                {
                    if(null!=assets[i])
                        UObj.DestroyImmediate(assets[i], true);
                }
                assets = null;
            }
        }
        public void SetAssets(UObj[] _assets, bool retry = false)
        {
            assets = _assets;
            this.OnAssetSet(retry);
        }
        public override void LoadSync()
        {
            base.LoadSync();
            UObj[] uos = bundle.LoadAllAssets();
            SetAssets(uos, true);
        }

	    protected override void DoAssetCallback(AssetCallback acb)
	    {
		    acb.Do(assets);
	    }

	    protected override void DoCallback(OnAssetGot cb)
	    {
		    cb(assets, 0);
	    }
    }
}

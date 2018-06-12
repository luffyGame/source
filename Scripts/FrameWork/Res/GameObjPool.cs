using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UObj = UnityEngine.Object;

//GameObject回收站
namespace FrameWork
{
	public class GameObjPool : SingleBehavior<GameObjPool>
	{
		#region Variables
		private const float CLEAN_TIME = 30f;//清空超时
		private class UnusedGo
		{
			private string assetPath;
			private List<GameObject> gos;
			private float updateTime;
			public UnusedGo(string path)
			{
				assetPath = path;
				gos = new List<GameObject>();
				updateTime = 0f;
			}
			public GameObject Peek()
			{
				if(gos.Count > 0)
				{
					updateTime = Time.realtimeSinceStartup;
					GameObject go = gos[0];
					gos.RemoveAt(0);
                    go.transform.SetParent(null);
					return go;
				}
				return null;
			}
			public void Push(GameObject go,Transform trans)
			{
				go.SetActive(false);
                go.transform.SetParent(trans);
				gos.Add(go);
				updateTime = Time.realtimeSinceStartup;
			}
			public void Empty()
			{
				for(int i=0;i<gos.Count;++i)
				{
					GameObject go = gos[i];
					GameObject.Destroy(go);
				}
				BundleMgr.Instance.ReleaseAsset(assetPath,false,gos.Count);
				gos.Clear();
			}
			public bool PeriodClean(float curTime)//周期清理
			{
                if (gos.Count == 0)
                    return true;
				if(curTime - updateTime>=CLEAN_TIME)
				{
					Empty();
                    return true;
				}
                return false;
			}
		}
		private Dictionary<string,UnusedGo> unUsed = new Dictionary<string, UnusedGo>();
        private Dictionary<ulong, GoCallback> goTasks = new Dictionary<ulong, GoCallback>();
        private Dictionary<ulong, IEnumerator> instanters = new Dictionary<ulong, IEnumerator>();
        private Transform cachedTrans;
        public Transform CachedTrans { get { if (null == cachedTrans) cachedTrans = transform; return cachedTrans; } }
		#endregion
        
		#region Public Method
		public ulong GetGameObj(string assetPath,OnGameObjGot cb)
		{
			if(unUsed.ContainsKey(assetPath))
			{
				UnusedGo one = unUsed[assetPath];
				GameObject go = one.Peek();
                if (null != go)
                {
                    cb(go, 0);
                    return 0;
                }
			}
            if (string.IsNullOrEmpty(assetPath))
            {
                GameObject go = new GameObject();
                cb(go, 0);
                return 0;
            }
            GoCallback gcb = GenTask(assetPath, cb);
            gcb.AssetCbId = BundleMgr.Instance.GetAsset(assetPath, (asset, cbId) =>
            {
                gcb.AssetCbId = 0;
                IEnumerator instant = Instant(gcb.Id, (UObj)asset);
                instanters.Add(gcb.Id, instant);
                StartCoroutine(instant);
            });
            return gcb.Id;
		}
        //取消获取，reserve标识对于取消加载的对象是缓存到自己的缓冲池
        //还是完全取消对象的加载
        public void CancelUngotGameObj(ulong cbIdx,bool reserve)
        {
            GoCallback gcb = GoCallback.Get(cbIdx);
            if (null == gcb)
                return;
            if(goTasks.ContainsKey(gcb.Id))
            {
                if(reserve&&!unUsed.ContainsKey(gcb.Path))//需要缓存的情况
                {
                    gcb.Cb = (go, cbId) =>
                    {
                        UnuseGameObj(gcb.Path, go);
                    };
                }
                else
                {
                    goTasks.Remove(gcb.Id);
                    GoCallback.Remove(gcb.Id);
                    gcb.Cancel();
                    if (instanters.ContainsKey(gcb.Id))
                    {
                        IEnumerator instanter = instanters[gcb.Id];
                        StopCoroutine(instanter);
                        instanters.Remove(gcb.Id);
                    }
                }
            }
        }
		public void UnuseGameObj(string assetPath,GameObject go)
		{
			if(!unUsed.ContainsKey(assetPath))
				unUsed.Add(assetPath,new UnusedGo(assetPath));
			UnusedGo one = unUsed[assetPath];
			one.Push(go,CachedTrans);
		}
		public void Empty()
		{
			foreach(KeyValuePair<string,UnusedGo> element in unUsed)
			{
				element.Value.Empty();
			}
			unUsed.Clear();
		}
        public bool IsIdle()
        {
            return goTasks.Count == 0;
        }
		public IEnumerator PeriodClean()
		{
			yield return null;
			float curTime = Time.realtimeSinceStartup;
            Dictionary<string, UnusedGo>.Enumerator it = unUsed.GetEnumerator();
            List<string> dels = null;
            while(it.MoveNext())
            {
                if (it.Current.Value.PeriodClean(curTime))
                {
                    if(null == dels)
                        dels = new List<string>();
                    dels.Add(it.Current.Key);
                }
            }
            if (null != dels)
            {
                for (int i = 0; i < dels.Count; ++i)
                    unUsed.Remove(dels[i]);
            }
		}
		#endregion
        #region Private Method
        private GoCallback GenTask(string resPath, OnGameObjGot cb)
        {
            GoCallback gcb = GoCallback.Gen(cb,resPath);
            goTasks.Add(gcb.Id, gcb);
            return gcb;
        }
        private IEnumerator Instant(ulong gcbId,UObj asset)
        {
            yield return null;
	        GameObject go = null;
	        if(null!=asset)
            	go = Instantiate(asset) as GameObject;
#if UNITY_EDITOR
            ApplyShader.CheckShader(go);
#endif
            FinishInstant(gcbId, go);
        }
        private void FinishInstant(ulong gcbId, GameObject go)
        {
            if (goTasks.ContainsKey(gcbId))
            {
                GoCallback task = goTasks[gcbId];
                task.Do(go);
                GoCallback.Remove(gcbId);
                goTasks.Remove(gcbId);
            }
            if (instanters.ContainsKey(gcbId))
                instanters.Remove(gcbId);
        }
        #endregion
    }
}
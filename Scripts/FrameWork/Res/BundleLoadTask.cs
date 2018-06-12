using UnityEngine;
using System.Collections;
using UObj = UnityEngine.Object;
using System.Collections.Generic;
using System.IO;
using System;
using FrameWork.BaseUtil;

namespace FrameWork
{
	public class BundleLoadTask
	{
		#region Variables
		private BundleHolder holder = null;
        private bool assetSync = true;
		private WWW wwwRequest = null;//自www的文件请求
		private AssetBundleCreateRequest diskRequest = null;//自文件请求
		private AssetBundleRequest assetRequest = null;//资源load请求
		private bool isFinished = false;
        private IEnumerator loadCouroutine;
		#endregion
		#region Properties
		public bool IsFinished 
		{get {return isFinished;}}
		public BundleHolder Holder
		{get {return holder;}}
		#endregion
		#region Public Method
		public BundleLoadTask(BundleHolder _holder,bool _assetSync = true)
		{
			holder = _holder;
            assetSync = _assetSync;
            loadCouroutine = Run();
            BundleLoader.Instance.StartCoroutine(loadCouroutine);
		}
		public void Cancel()
		{
            if (null != loadCouroutine)
            {
                BundleLoader.Instance.StopCoroutine(loadCouroutine);
                loadCouroutine = null;
            }
			holder = null;
		}
		public IEnumerator Run()
		{
			//_Scripts.Main.App.LogGame(string.Format("load begin {0} at {1:F3}",holder.Info.path,Time.realtimeSinceStartup));
			if(null == holder)
				yield break;
			string bundlePath = FileUtility.GetFileReadFullPath(string.Format("{0}{1}",BundleCfg.relativePath,holder.Info.path));
			if (bundlePath == null)
			{
				isFinished = true;
				OnBundleLoaded(null);
				yield break;
			}
			if (bundlePath.Contains("://"))
			{
				using(wwwRequest = new WWW(bundlePath))
				{
					yield return wwwRequest;
                    loadCouroutine = null;
					if(null == wwwRequest.error)
					{
						//_Scripts.Main.App.LogGame(string.Format("load file {0} at {1:F3}",holder.Info.path,Time.realtimeSinceStartup));
						OnBundleLoaded(wwwRequest.assetBundle);
					}
					else
					{
						Debugger.LogWarning("fail to load file {0}",holder.Info.path);
						OnBundleLoaded(null);
					}
					wwwRequest.Dispose();
				}
				wwwRequest = null;
			}
			else
			{
				diskRequest = AssetBundle.LoadFromFileAsync(bundlePath);
				yield return diskRequest;
                loadCouroutine = null;
				//_Scripts.Main.App.LogGame(string.Format("load file {0} at {1:F3}",holder.Info.path,Time.realtimeSinceStartup));
				OnBundleLoaded(diskRequest.assetBundle);
				diskRequest = null;
			}
			isFinished = true;
		}
		public float GetProgress()
		{
			if(null!=wwwRequest)
				return wwwRequest.progress*0.5f;
			if(null!=diskRequest)
				return diskRequest.progress*0.5f;
			if(null!=assetRequest)
				return 0.5f + assetRequest.progress*0.5f;
			return 0f;
		}
		#endregion
		#region Private Method
		private void OnBundleLoaded(AssetBundle bundle)
		{
			if(null!=holder)
			{
				holder.MyBundle = bundle;
				if(assetSync)
				{
                    if (null == bundle)//读取资源文件失败的
                    {
                        Debugger.LogWarning("err: load file {0},bundle is null", holder.Info.path);
                        if (holder.IsHoldOneAsset)
                            (holder as BundleHolderOne).SetAsset(null);
                        else
                            (holder as BundleHolderMulti).SetAssets(null, true);
                    }
                    else
                    {
                        if (holder.IsHoldOneAsset)
                        {
                            UObj uo = bundle.LoadAsset(holder.Info.mainName, holder.Info.GetAssetType());
                            (holder as BundleHolderOne).SetAsset(uo, true);
                        }
                        else
                        {
                            UObj[] uos = bundle.LoadAllAssets();
                            (holder as BundleHolderMulti).SetAssets(uos, true);
                        }
                    }
				}
				else
                {
                    loadCouroutine = DoAsyncAssetLoad(bundle);
                    BundleLoader.Instance.StartCoroutine(loadCouroutine);
                }
			}
			else if(null!=bundle)//没有holder直接删除
			{
				bundle.Unload(true);
				UObj.DestroyImmediate(bundle,true);
			}
		}

        private IEnumerator DoAsyncAssetLoad(AssetBundle bundle)
        {
            if (holder.IsHoldOneAsset)
                assetRequest = bundle.LoadAssetAsync(holder.Info.mainName, holder.Info.GetAssetType());
            else
                assetRequest = bundle.LoadAllAssetsAsync();
            yield return assetRequest;
            if (holder != null)
            {
                if(holder.IsHoldOneAsset)
                    (holder as BundleHolderOne).SetAsset(assetRequest.asset,true);
                else
                    (holder as BundleHolderMulti).SetAssets(assetRequest.allAssets, true);
            }
            else
            {
                for (int i = 0; i < assetRequest.allAssets.Length; ++i)
                {
                    UObj asset = assetRequest.allAssets[i];
                    Debugger.LogWarning("del before {0} create finished", asset.name);
                    UObj.DestroyImmediate(asset, true);
                }
                bundle.Unload(true);
                UObj.DestroyImmediate(bundle, true);
                bundle = null;
            }
            assetRequest = null;
            loadCouroutine = null;
        }
		#endregion
	}
}

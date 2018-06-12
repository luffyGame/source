using UnityEngine;
using System.Collections;
using FrameWork.BaseUtil;
using System.IO;
using System;
using UnityEngine.SceneManagement;

namespace FrameWork
{
    public class SceneLoader : SingleBehavior<SceneLoader>
	{
        public delegate void OnLevelLoad(float progress, bool isDone);
		private string levelName = null;
		private bool isLevelAdded = false;
		private WWW wwwRequest = null;//自www的文件请求
        private AssetBundleCreateRequest diskRequest = null;//自文件请求
		private AsyncOperation async = null;
		private bool isLoading = false;
		private AssetBundle bundle = null;
        private OnLevelLoad onLoad;
		// Update is called once per frame
		void Update ()
		{
			if(isLoading)
			{
				bool isDone = null!=async&&async.isDone;
				if (null != onLoad)
				{
					if (isDone)
					{
						OnLevelLoad tmp = onLoad;
						onLoad = null;
						async = null;
						isLoading = false;
						OnSceneLoaded();
						tmp(1f, true);
					}
					else
						onLoad(GetProgress(), false);
				}
			}
		}
		#region Public Method
		public void AsnycLoadLevel (string _levelName,OnLevelLoad onLoad = null, bool isAdded = true)
		{
			if(!isLoading)
			{
				isLoading = true;
				levelName = _levelName;
				isLevelAdded = isAdded;
                this.onLoad = onLoad;
				StartCoroutine(Run());
			}
		}
		#endregion
		#region Private Method
		private IEnumerator Run()
		{
			if(!isLevelAdded)
			{
				string bundlePath = FileUtility.GetFileReadFullPath(string.Format("{0}Scene/{1}.assetbundle",BundleCfg.relativePath,levelName));
				if (bundlePath.Contains("://"))
				{
					using(wwwRequest = new WWW(bundlePath))
					{
						yield return wwwRequest;
						if(null == wwwRequest.error)
						{
							bundle = wwwRequest.assetBundle;
						}
						else
						{
							OnSceneLoaded();
						}
						wwwRequest.Dispose();
					}
					wwwRequest = null;
				}
				else
				{
                    diskRequest = AssetBundle.LoadFromFileAsync(bundlePath);
                    yield return diskRequest;
                    bundle = diskRequest.assetBundle;
                    diskRequest = null;
				}
			}
			if(null!=bundle||isLevelAdded)
			{
                async = SceneManager.LoadSceneAsync(levelName);
				yield return async;
			}
		}
		private float GetProgress()
		{
			if(isLevelAdded)
			{
				if(null!=async)
					return async.progress;
			}
			else
			{
				if(null!=wwwRequest)
					return wwwRequest.progress*0.5f;
				if(null!=diskRequest)
                    return diskRequest.progress * 0.5f;
				if(null!=async)
					return 0.5f + async.progress*0.5f;
			}
			return 0f;
		}
		private void OnSceneLoaded()
		{
			if(null!=bundle)
			{
				StartCoroutine(Utils.DelayAction(() =>
				{
					bundle.Unload(false);
					DestroyImmediate(bundle,true);
					bundle = null;
				}));
				/*
				#if UNITY_EDITOR
				GameObject[] roots = GameObject.FindGameObjectsWithTag("SceneRoot");
				if(null!=roots)
				{
					for(int i=0;i<roots.Length;++i)
					{
						//ApplyShader.CheckShader(roots[i]);
						//StaticBatchingUtility.Combine(roots[i]);
					}
				}
				Shader skyShader = null == RenderSettings.skybox?null:RenderSettings.skybox.shader;
				if(null!=skyShader)
					RenderSettings.skybox.shader = Shader.Find(skyShader.name);
				#endif
				*/
			}
		}
		#endregion
	}
}
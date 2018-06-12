using FrameWork.BaseUtil;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameWork
{
    public class App : SingleBehavior<App>
    {
        #region Var
        public int targetFrameRate = 30;
        public bool unzipToDisk;
        public UiProgress uiProgress;
        #endregion
        public override void OnInit()
        {
            InitRunDir();
            Debugger.logger = Global.LogInstance;
            
            Application.targetFrameRate = targetFrameRate;
            UnityEngine.Object.DontDestroyOnLoad(gameObject);
        }

        public override void OnStart()
        {
            Launch();

        }
        public void Launch()
        {
            bool needUnzip = false;
#if !UNITY_EDITOR && UNITY_ANDROID
            needUnzip = unzipToDisk;
#endif
            if (needUnzip)
            {
                ResUnzip.Instance.ReleaseApkRes(() =>
                {
                    OnLaunch();
                });
                uiProgress.StartProgress(ResUnzip.Instance.GetProgress);
                StartCoroutine(ResUnzip.Instance.WaitFor());
            }
            else
            {
                OnLaunch();
            }
        }

        protected virtual void OnLaunch()
        {
            uiProgress.ProgressOver();
            BundleMgr.Instance.ResetDep();
        }
        
        private void InitRunDir()
        {
            string runPath;
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.OSXEditor:
                    runPath = Application.dataPath + "/_run/";
                    if (!System.IO.Directory.Exists(runPath))
                        System.IO.Directory.CreateDirectory(runPath);
                    break;
                default:
                    runPath = Application.persistentDataPath;
                    break;
            }
            BaseUtil.FileUtility.DirRoot2Path = runPath;
        }
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

namespace FrameWork
{
    public class Sdk : SingleBehavior<Sdk>
    {
        public bool useSdk;
#if UNITY_ANDROID
        protected static AndroidJavaObject m_activity = null;
#endif
        public override void OnInit()
        {
            if (useSdk)
            {
#if UNITY_EDITOR
#elif UNITY_ANDROID
            using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                m_activity = jc.GetStatic<AndroidJavaObject>("currentActivity");
                if (null != m_activity)
                {
                    m_activity.CallStatic("InitCallBackObj", name);
                }
            }
#endif
            }
            InitRunDir();
            Debugger.logger = Global.LogInstance;
        }

        #region Private Method
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
        #endregion
    }
}

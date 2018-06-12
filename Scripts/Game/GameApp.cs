using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using FrameWork;
using UnityEngine.Rendering;

namespace Game
{
    public class GameApp : App
    {
        public GameObject splash;
        protected override void OnLaunch()
        {
            base.OnLaunch();
            DOTween.Init(true, false).SetCapacity(50, 50);
            Debugger.useLog = false;
            Debugger.usePrefix = true;
            Debugger.Log("App start");
            Debugger.Log("cur quality = {0}", QualitySettings.GetQualityLevel());
            bool glEs3 = SystemInfo.graphicsDeviceType == GraphicsDeviceType.OpenGLES3;
            Debugger.Log("support gles3.0 : {0}",glEs3);
            Prepare();
            LuaEntrance.Instance.Start();
        }
        public override void OnQuit()
        {
            base.OnQuit();
            //注意调用顺序
            LuaEntrance.Instance.Relsease();
            Storage.Instance.ShutDown();
            WebExecWorker.Instance.ShutDown();
        }
        private void Prepare()
        {
            Storage.Instance.Init();
            LuaEntrance.Instance.Init();
        }

        private void Update()
        {
            LuaEntrance.Instance.Update();
            Storage.Instance.OnUpdate();
            WebExecWorker.Instance.UpdateDone();
        }

        public void DestroySplash()
        {
            GameObject.Destroy(splash);
            splash = null;
        }
    }
}
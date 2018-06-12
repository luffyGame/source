using UnityEngine;
using System.Collections;
using FrameWork.BaseUtil;
using System;

namespace FrameWork
{
    public class SceneMnger : SingleBehavior<SceneMnger>
    {
        #region Var
        public SceneBase curScene { get; private set; }
        #endregion
        #region Public Method
        public override void OnStart()
        {
            
        }
        public override void OnQuit()
        {
            if (null != curScene)
            {
                curScene.Leave();
                curScene = null;
            }
        }
        public void SwitchScene<T>(T newScene,bool showLoading = true) where T : SceneBase
        {
            if (showLoading)
                Global.Instance.BeginWaiting();
            if (null != curScene)
            {
                curScene.Leave();
                curScene = null;
            }
            SceneLoader.Instance.AsnycLoadLevel(newScene.LevelName, (progress, isDone) =>
            {
                if (isDone)
                {
                    curScene = newScene;
                    curScene.Enter();
                    StartCoroutine(WaitSceneReady());
                }
            });
        }
        
        public IEnumerator WaitSceneReady()
        {
            yield return new WaitForSeconds(1f);
            while (!curScene.isReady)
                yield return null;
            Global.Instance.EndWaiting();
            curScene.OnReady();
        }
        #endregion
    }
}
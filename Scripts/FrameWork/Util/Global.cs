using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using URandom = UnityEngine.Random;
using SRandom = System.Random;

namespace FrameWork
{
    /// <summary>
    /// 全局变量保存
    /// </summary>
    public class Global : SingleBehavior<Global>
    {
        #region Var
        public string LogServerUrl = null;
        private static LogExecutor s_LogExecutor = null;
        public static LogExecutor LogInstance
        {
            get { if (null == s_LogExecutor) s_LogExecutor = new LogExecutor(); return s_LogExecutor; }
        }

        public RectTransform canvasRectTransform;

        private Vector2 canvasSize = new Vector2(-1,-1);
        public Vector2 CanvasSize { 
            get 
            {
                if(canvasSize.x<0)
                    canvasSize = canvasRectTransform.sizeDelta;
                return canvasSize; 
            } 
        }
        public GameObject UiTopMask;
        public Camera UiCamera;
        public CanvasScaler[] canvasScalers;
        public RectTransform[] uiRoots;
        public Transform MainCameraTrans { get; private set; }
        public Camera MainCamera { get; private set; }
        public static SRandom random { get; private set; }
        public UiWaiting waiting;
        public ActionConfig actionConfig { get; set; }
        public CameraFollow MainCameraFollow { get; set; }

        public float CameraYRot
        {
            get
            {
                if (null == MainCameraFollow)
                    return 0f;
                return MainCameraFollow.yRot;
            }
        }

        public Transform TerrainTransform
        {
            get
            {
                Terrain terrain = Terrain.activeTerrain;
                if (null != terrain)
                    return terrain.transform;
                return null;
            }
        }

        #endregion
        #region Public Method

        public override void OnInit()
        {
            IphoneXAdapter.Instance.Adapt(canvasScalers,uiRoots);
        }

        public override void OnQuit()
        {
            Debugger.Log("App shutDown");
            Debugger.logger = null;
            LogInstance.ShutDown();
            MainCamera = null;
        }

        public static void Log(string message)
        {
            LogInstance.Log(message);
        }

        public static void Log(string msg, string stack, LogType type)
        {
            Log(msg);
        }

        public static void LogAndUpload(string message)
        {
#if UNITY_EDITOR
            Log(message);
#else
			LogInstance.Log(message,true);
#endif
            Debug.LogError(message);
        }

        public static void InitRandom(int seed)
        {
            random = new SRandom(seed);
        }

        public static int RandomRange(int min, int max)
        {
            if (null != random)
            {
                int v = random.Next(min, max);
                return v;
            }
            return URandom.Range(min, max);
        }

        public static float RandomRange(float min, float max)
        {
            if (null != random)
            {
                float v = min + (float)random.NextDouble() * (max - min);
                return v;
            }
            return URandom.Range(min, max);
        }
        public static float Random()
        {
            if (null != random)
            {
                float v = (float)random.NextDouble();
                return v;
            }
            return URandom.Range(0f, 1f);
        }
        public void SetMainCamera(GameObject mainCameraGo)
        {
            if (null == mainCameraGo)
            {
                MainCameraTrans = null;
                MainCamera = null;
            }
            else
            {
                MainCameraTrans = mainCameraGo.transform;
                MainCamera = mainCameraGo.GetComponent<Camera>();
            }
        }
        public Vector2 GetUiPosInCanvas(Vector3 pos)
        {
            Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(null, pos);
            Vector2 ret;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, screenPos, UiCamera, out ret))
                return ret;
            return Vector2.zero;
        }
        public void BeginWaiting()
        {
            if (null != waiting)
                waiting.Show(true);
        }
        public void EndWaiting()
        {
            if (null != waiting)
                waiting.Show(false);
        }

        public ActionLen GetActionlen(string res)
        {
            if (null != actionConfig)
            {
                if (actionConfig.cfg.ContainsKey(res))
                    return actionConfig.cfg[res];
            }

            return null;
        }
        #endregion
    }
}

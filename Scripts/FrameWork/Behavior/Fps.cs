using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FrameWork
{
    public class Fps : MonoBehaviour
    {
        #region Variables
        public Text fpsLabel;
        public float mUpdateInterval = 1f;
        private float mLastTime;
        private int iFrames = 0;
        private float fFps = 0f;
        private string fpsFormat;
        //private bool enable_GUI = true;
        private static readonly string FPS_FORMAT = "fps:{0:F1}";
        #endregion
        // Use this for initialization
        void Start()
        {
            mLastTime = Time.realtimeSinceStartup;
            iFrames = 0;
        }

        // Update is called once per frame
        void Update()
        {
            ++iFrames;
            float curTime = Time.realtimeSinceStartup;
            if (curTime > mUpdateInterval + mLastTime)
            {
                fFps = iFrames / (curTime - mLastTime);
                fpsFormat = string.Format(FPS_FORMAT, fFps);
                fpsLabel.text = fpsFormat;
                iFrames = 0;
                mLastTime = curTime;
            }
        }
        /*
        void OnGUI()
        {
            if (enable_GUI)
            {
                GUI.skin.label.fontSize = 35;
                GUI.Label(new Rect((Screen.width - 100) / 2, (Screen.height - 50) / 2, 100, 50), fpsFormat);
            }
        }
         * */
    }
}

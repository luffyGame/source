using System;
using System.Reflection;
using UnityEngine;

namespace Game
{
    public class OnTime : MonoBehaviour
    {
        public float continueTime;
        public Action onTime;
        private float curTime;

        public void Play()
        {
            curTime = 0f;
            enabled = true;
        }

        private void Update()
        {
            curTime += Time.deltaTime;
            if (curTime >= continueTime)
            {
                enabled = false;
                if (null != onTime)
                    onTime();
            }
        }
    }
}
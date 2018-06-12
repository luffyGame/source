using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FrameWork
{
    public class UiProgress : MonoBehaviour
    {
        public Slider slider;
        private Func<float> getProgress;

        public void StartProgress(Func<float> getProgress)
        {
            this.getProgress = getProgress;
            slider.value = 0f;
            slider.gameObject.SetActive(true);
        }
        public void ProgressOver()
        {
            slider.gameObject.SetActive(false);
        }

        void Update()
        {
            if (null != getProgress)
                slider.value = getProgress();
        }
    }
}

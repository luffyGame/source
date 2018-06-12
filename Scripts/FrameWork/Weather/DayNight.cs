using System;
using System.Collections.Generic;
using UnityEngine;

namespace FrameWork
{
    public class DayNight : MonoBehaviour
    {
        private LightmapData[] dayData,nightData;
        private bool isInited;
        private bool isNight;
        public Texture2D[] dayText,nightText;

        public Color dayFogColor, nightFogColor;
        public FogMode dayFogMode, nightFogMode;
        public float[] dayFogDist,nightFogDist;
        public float dayFogDensity, nightFogDensity;
        public LensFlare dayFlare;
        void Awake()
        {
            Init();
        }
        public void Day()
        {
            isNight = false;
            LightmapSettings.lightmaps = dayData;
            this.UpdateFog();
            if (null != this.dayFlare)
                dayFlare.enabled = true;
        }
        public void Night()
        {
            isNight = true;
            LightmapSettings.lightmaps = nightData;
            this.UpdateFog();
            if (null != this.dayFlare)
                dayFlare.enabled = false;
        }
        [ContextMenu("Switch")]
        public void Switch()
        {
            Init();
            if (isNight)
                Day();
            else
                Night();
        }
        private void Init()
        {
            if (isInited)
                return;
            isInited = true;
            dayData = new LightmapData[dayText.Length];
            for (int i = 0; i < dayText.Length; ++i)
            {
                dayData[i] = new LightmapData();
                dayData[i].lightmapColor = dayText[i];
            }
            nightData = new LightmapData[nightText.Length];
            for (int i = 0; i < nightText.Length; ++i)
            {
                nightData[i] = new LightmapData();
                nightData[i].lightmapColor = nightText[i];
            }
        }
        private void UpdateFog()
        {
            if (isNight)
            {
                RenderSettings.fogColor = nightFogColor;
                RenderSettings.fogMode = nightFogMode;
            }
            else
            {
                RenderSettings.fogColor = dayFogColor;
                RenderSettings.fogMode = dayFogMode;
            }
            switch (RenderSettings.fogMode)
            {
                case FogMode.Linear:
                    float[] dist = isNight ? nightFogDist : dayFogDist;
                    RenderSettings.fogStartDistance = dist[0];
                    RenderSettings.fogEndDistance = dist[1];
                    break;
                default:
                    RenderSettings.fogDensity = isNight ? nightFogDensity : dayFogDensity;
                    break;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

namespace FrameWork
{
    public class PrefabLightmapData : MonoBehaviour
    {
        [System.Serializable]
        public struct RendererInfo
        {
            public Renderer renderer;
            public int lightmapIndex;
            public Vector4 lightmapOffsetScale;
        }
        public List<RendererInfo> m_RendererInfo;
        public List<Texture2D> m_lightmap0;
        public List<Texture2D> m_lightmap1;
        void Awake()
        {
            LoadLightmap();
        }

        public void SaveLightmap()
        {
            m_lightmap0 = new List<Texture2D>();
            m_lightmap1 = new List<Texture2D>();
            for (int i = 0; i < LightmapSettings.lightmaps.Length; ++i)
            {
                m_lightmap0.Add(LightmapSettings.lightmaps[i].lightmapColor);
                m_lightmap1.Add(LightmapSettings.lightmaps[i].lightmapDir);
            }
            m_RendererInfo = new List<RendererInfo>();
            var renderers = GetComponentsInChildren<Renderer>();
            foreach (Renderer r in renderers)
            {
                if (r.lightmapIndex != -1)
                {
                    RendererInfo info = new RendererInfo();
                    info.renderer = r;
                    info.lightmapOffsetScale = r.lightmapScaleOffset;
                    info.lightmapIndex = r.lightmapIndex;
                    m_RendererInfo.Add(info);
                }
            }
        }

        public void LoadLightmap()
        {
            if (null != m_lightmap0)
            {
                LightmapData[] lightmaps = new LightmapData[m_lightmap0.Count];
                for (int i = 0; i < m_lightmap0.Count; ++i)
                {
                    lightmaps[i] = new LightmapData();
                    lightmaps[i].lightmapColor = m_lightmap0[i];
                    lightmaps[i].lightmapDir = m_lightmap1[i];
                }
                LightmapSettings.lightmaps = lightmaps;
            }

            if (null == m_RendererInfo || m_RendererInfo.Count <= 0) return;

            foreach (var item in m_RendererInfo)
            {
                item.renderer.lightmapIndex = item.lightmapIndex;
                item.renderer.lightmapScaleOffset = item.lightmapOffsetScale;
            }
        }

        public void Clear()
        {
            m_RendererInfo = null;
            m_lightmap0 = null;
            m_lightmap1 = null;
        }
    }
}

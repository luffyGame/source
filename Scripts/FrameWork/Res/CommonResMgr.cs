using System;
using System.Collections.Generic;
using UnityEngine;
using UObj = UnityEngine.Object;

namespace FrameWork
{
    public class CommonResMgr : SingleBehavior<CommonResMgr>
    {
        #region Var
        public UiAtlasAsset[] cachedAtlass;//处理过的图集
        public Font[] uiFonts;
        
        public string shaderAsset;//全部的shader，一个就ok
        public string fontAsset;//字体，目前一个也ok
        
        private string[] atlasAssets;
        
        private Dictionary<string, Sprite> cache = new Dictionary<string,Sprite>();
        private Dictionary<string, Font> fonts = new Dictionary<string, Font>();
        
        #endregion
        #region Public Method
        public override void OnInit()
        {
            if (null != cachedAtlass)
            {
                for (int i = 0; i < cachedAtlass.Length; ++i)
                {
                    if (null != cachedAtlass[i])
                        CacheSprite(cachedAtlass[i].sprites);
                }
            }

            for(int i=0;i<uiFonts.Length;++i)
            {
                Font f = uiFonts[i];
                if(null!=f)
                {
                    if (!fonts.ContainsKey(f.name))
                        fonts.Add(f.name, f);
                }
            }
        }
        public void CacheAtlas(string[] atlasRess)
        {
            atlasAssets = atlasRess;
        }

        public void Cache(Action callback)
        {
            int count = atlasAssets.Length;
            if (!string.IsNullOrEmpty(shaderAsset))
                count++;
            if (!string.IsNullOrEmpty(fontAsset))
                count++;
            if(count == 0)
            {
                if (null != callback) callback();
                return;
            }
            Action done = () => { --count; if (count == 0 && null != callback) callback(); };
            for (int i = 0; i < atlasAssets.Length; ++i)
            {
                LoadAtlas(atlasAssets[i], done);
            }
            if (!string.IsNullOrEmpty(shaderAsset))
                LoadShader(done);
            if(!string.IsNullOrEmpty(fontAsset))
                LoadFont(done);
        }
        public Sprite GetSprite(string name)
        {
            if (null == name)
                return null;
            if (cache.ContainsKey(name))
                return cache[name];
            return null;
        }
        public Font GetFont(string name)
        {
            if (fonts.ContainsKey(name))
                return fonts[name];
            return null;
        }
        #endregion
        #region Private Method
        private void LoadAtlas(string atlasRes,Action cb=null)
        {
            string assetPath = ResObjUtil.GetObjPath(EObjType.ATLAS, atlasRes);
            BundleMgr.Instance.GetAsset(assetPath, (objs, cbId) =>
            {
                UObj[] assets = (UObj[])objs;
                foreach (var asset in assets)
                {
                    Sprite sprite = asset as Sprite;
                    if (null != sprite)
                    {
                        if(cache.ContainsKey(sprite.name))
                            Debugger.Log("atlas {0} sprite {1} already added",atlasRes,sprite.name);
                        else
                            cache.Add(sprite.name,sprite);
                    }
                }
                if (null != cb)
                    cb();
            });
        }
        private void CacheSprite(List<Sprite> sprites)
        {
            for (int i = 0; i < sprites.Count; ++i)
            {
                cache.Add(sprites[i].name, sprites[i]);
            }
        }
        private void RemoveSprite(List<Sprite> sprites)
        {
            for (int i = 0; i < sprites.Count; ++i)
            {
                cache.Remove(sprites[i].name);
            }
        }
        private void LoadShader(Action cb = null)
        {
            string assetPath = ResObjUtil.GetObjPath(EObjType.SHADERS, shaderAsset);
            BundleMgr.Instance.GetAsset(assetPath, (assets, cbId) =>
            {
                //暂时注释掉，现在会影响加载速度，预计替换为ShaderVariantCollection
                //Shader.WarmupAllShaders();
                if (null != cb)
                    cb();
            });
        }

        private void LoadFont(Action cb = null)
        {
            string assetPath = ResObjUtil.GetObjPath(EObjType.FONT, fontAsset);
            BundleMgr.Instance.GetAsset(assetPath, (asset, cbId) =>
            {
                Font f = asset as Font;
                if(null!=f)
                {
                    if (!fonts.ContainsKey(f.name))
                        fonts.Add(f.name, f);
                }
                if (null != cb)
                    cb();
            });
        }
        #endregion
    }
}

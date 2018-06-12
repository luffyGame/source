using System;
using System.Collections.Generic;
using UnityEngine;

namespace FrameWork
{
    //记录ani长度，且按加入的顺序排序
    public class AniClipLen : Dictionary<string,float>
    {
        //public List<string> Names { get; private set; }

        public AniClipLen(Animation anim)
        {
            //Names = new List<string>();
            foreach (AnimationState animState in anim)
            {
                string name = animState.name;
                AnimationClip clip = animState.clip;
                float length = clip.length;
                //Names.Add(name);
                Add(name,length);
            }
        }

        private string uniqueAni;

        public String UniqueAni
        {
            get
            {
                if (null == uniqueAni)
                {
                    if (Count > 0)
                    {
                        foreach (var kvp in this)
                        {
                            uniqueAni = kvp.Key;
                            break;
                        }
                    }
                }

                return uniqueAni;
            }
        }
    }

    public class AniClipLenCfg : Singleton<AniClipLenCfg>
    {
        private Dictionary<string, AniClipLen> cfg = new Dictionary<string, AniClipLen>();

        public AniClipLen GetClipLen(string res)
        {
            if (cfg.ContainsKey(res))
                return cfg[res];
            return null;
        }

        public void SetClipLen(string res, AniClipLen clipLen)
        {
            cfg.Add(res,clipLen);
        }
    }
}
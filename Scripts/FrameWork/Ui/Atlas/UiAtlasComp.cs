using System;
using System.Collections.Generic;
using UnityEngine;

namespace FrameWork
{
    public class UiAtlasComp : MonoBehaviour
    {
        public List<Sprite> sprites = new List<Sprite>();
        public int Count { get { return sprites.Count; } }
        public Sprite this[int index]
        {
            get
            {
                return sprites[index];
            }
        }
    }
}

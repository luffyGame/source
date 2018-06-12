using System;
using System.Collections.Generic;
using UnityEngine;
using FrameWork;
using System.Text;

namespace FrameWork
{
    [Serializable]
    public class ActionLen : SerializableDictionary<string,float>
    {
    }
    [Serializable]
    public class ActionLens : SerializableDictionary<string, ActionLen> { }
    public class ActionConfig : ScriptableObject
    {
        [SerializeField]
        public ActionLens cfg;
    }
}

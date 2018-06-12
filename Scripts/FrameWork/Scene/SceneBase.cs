using System;
using System.Collections.Generic;

namespace FrameWork
{
    public abstract class SceneBase
    {
        public bool isReady { get; set; }
        public abstract string LevelName { get; }
        public abstract void Leave();
        public abstract void Enter();
        public virtual void OnReady()
        {
        }
    }
}

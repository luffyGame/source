using System;
using System.Collections.Generic;
using UnityEngine;

namespace FrameWork.BehaviorTree
{
    public class BtActionWithInfo : BTAction
    {
        private float enterTime;
        private float costTime;
        protected bool log;
        public BtActionWithInfo(BTPrecondition precondition = null) : base(precondition) { }
        protected override void Enter()
        {
            enterTime = Time.realtimeSinceStartup;
            base.Enter();
        }
        public override BTResult Tick()
        {
            BTResult ret = base.Tick();
            if (ret == BTResult.Ended)
            {
                costTime = Time.realtimeSinceStartup - enterTime;
                if(log)
                    Debug.Log(this.GetType().ToString() + "cost : " + costTime);
            }
            return ret;
        }
    }
}

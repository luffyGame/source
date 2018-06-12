using System;
using System.Collections.Generic;
using UnityEngine;

namespace FrameWork.BehaviorTree
{
    public class BtWaitAction : BTAction
    {
        private float _startTime;
        public float seconds { get; set; }
        public BtWaitAction(float seconds)
        {
            this.seconds = seconds;
        }
        protected override void Enter()
        {
            base.Enter();
            _startTime = Time.time;
        }
        protected override BTResult Execute()
        {
            if (Time.time - _startTime >= seconds)
                return BTResult.Ended;
            return BTResult.Running;
        }
    }
}

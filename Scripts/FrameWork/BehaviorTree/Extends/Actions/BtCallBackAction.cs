using System;
using System.Collections.Generic;

namespace FrameWork.BehaviorTree
{
    public class BtCallBackAction : BTAction
    {
        private Action callback;
        public BtCallBackAction(Action callback)
        {
            this.callback = callback;
        }
        protected override BTResult Execute()
        {
            this.callback();
            return BTResult.Ended;
        }
    }
}

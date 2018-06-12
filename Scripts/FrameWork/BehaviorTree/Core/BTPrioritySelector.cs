using System;
using System.Collections.Generic;

namespace FrameWork.BehaviorTree
{
    /// <summary>
    /// BTPrioritySelector selects the first sussessfully evaluated child as the active child.
    /// </summary>
    public class BTPrioritySelector : BTNode
    {
        private BTNode _activeChild;
        public BTPrioritySelector(BTPrecondition precondition = null) : base(precondition) { }
        protected override bool DoEvaluate()
        {
            for(int i=0;i<children.Count;++i)
            {
                BTNode child = children[i];
                if (child.Evaluate())
                {
                    if (_activeChild != null && _activeChild != child)
                    {
                        _activeChild.Clear();
                    }
                    _activeChild = child;
                    return true;
                }
            }

            if (_activeChild != null)
            {
                _activeChild.Clear();
                _activeChild = null;
            }

            return false;
        }

        public override void Clear()
        {
            if (_activeChild != null)
            {
                _activeChild.Clear();
                _activeChild = null;
            }
        }

        public override BTResult Tick()
        {
            if (_activeChild == null)
            {
                return BTResult.Ended;
            }

            BTResult result = _activeChild.Tick();
            if (result != BTResult.Running)
            {
                _activeChild.Clear();
                _activeChild = null;
            }
            return result;
        }
    }
}

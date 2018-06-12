using System;
using System.Collections.Generic;
using UnityEngine;

namespace FrameWork.BehaviorTree
{
    public enum BTResult
    {
        Ended = 1,
        Running = 2,
    }
    public abstract class BTNode
    {
        public string name;
        protected List<BTNode> _children;
        public List<BTNode> children { get { return _children; } }
        public BTPrecondition precondition;
        public BTDatabase database;
        public float interval = 0;
        private float _lastTimeEvaluated = 0;

        public bool activated;
        public BTNode(BTPrecondition precondition)
        {
            this.precondition = precondition;
        }
        public BTNode() : this(null) { }
        public virtual void Activate(BTDatabase database)
        {
            if (activated) return;

            this.database = database;
            //			Init();

            if (precondition != null)
            {
                precondition.Activate(database);
            }
            if (_children != null)
            {
                for(int i=0;i<_children.Count;++i)
                {
                    _children[i].Activate(database);
                }
            }

            activated = true;
        }
        public void Deactive()
        {
            if (!activated)
                return;
            this.database = null;
            //			Init();

            if (precondition != null)
            {
                precondition.Deactive();
            }
            if (_children != null)
            {
                for (int i = 0; i < _children.Count; ++i)
                {
                    _children[i].Deactive();
                }
            }

            activated = false;
        }

        public bool Evaluate()
        {
            //return activated && CheckTimer() && (precondition == null || precondition.Check()) && DoEvaluate();
            return activated &&(precondition == null || precondition.Check()) && DoEvaluate();
        }

        protected virtual bool DoEvaluate() { return true; }
        public virtual BTResult Tick() { return BTResult.Ended; }
        public virtual void Clear() { }
        public virtual void AddChild(BTNode aNode)
        {
            if (_children == null)
            {
                _children = new List<BTNode>();
            }
            if (aNode != null)
            {
                _children.Add(aNode);
            }
        }
        public virtual void RemoveChild(BTNode aNode)
        {
            if (_children != null && aNode != null)
            {
                _children.Remove(aNode);
            }
        }
        private bool CheckTimer()
        {
            if (Time.time - _lastTimeEvaluated > interval)
            {
                _lastTimeEvaluated = Time.time;
                return true;
            }
            return false;
        }
    }
}

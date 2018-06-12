using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 
    /// </summary>
    [RequireComponent(typeof(Collider),typeof(Rigidbody))]
    public class UseChecker : MonoBehaviour
    {
        public Collider triggerCollider;
        public bool isEnabled;
        public Action<int, int> onTargetSet;
        
        private List<UseableCollider> visibles = new List<UseableCollider>();
        private UseableCollider currentTarget;
        private Action<UseableCollider> cachedOnRemove;

        private Action<UseableCollider> CachedOnRemove
        {
            get
            {
                if (null == cachedOnRemove)
                    cachedOnRemove = this.RemoveOne;
                return cachedOnRemove;
            }
        }

        private Transform trans;
        public Transform Trans {get
        {
            if (null == trans) trans = transform;
            return trans;
        }}

        public void EnableCheck(bool bEnable)
        {
            isEnabled = bEnable;
            triggerCollider.enabled = bEnable;
            if (!bEnable)
            {
                this.ReleaseAll();
            }
            currentTarget = null;
        }
        private void OnTriggerEnter(Collider other)
        {
            if(!isEnabled)
                return;
            var useCollider = other.GetComponent<UseableCollider>();
            if(null == useCollider)
                return;
            OneEnter(useCollider);
        }

        private void OnTriggerExit(Collider other)
        {
            if(!isEnabled)
                return;
            var useCollider = other.GetComponent<UseableCollider>();
            if(null == useCollider)
                return;
            OneExit(useCollider);
        }

        private void Update()
        {
            if (enabled)
            {
                UseableCollider target = null;
                float dist = float.MaxValue;
                Vector3 selfPos = Trans.position;
                foreach (var visibleOne in visibles)
                {
                    if (visibleOne.IsValid()&&visibleOne.useable)
                    {
                        Vector3 delta = visibleOne.Trans.position - selfPos;
                        float deltaDist = Mathf.Max(0f,delta.magnitude - visibleOne.radius);
                        if (deltaDist < dist)
                        {
                            dist = deltaDist;
                            target = visibleOne;
                        }
                    }
                }
                SetTarget(target);
            }
        }

        private void OneEnter(UseableCollider useCollider)
        {
            if(visibles.Contains(useCollider))
                return;
            useCollider.onRelease = (Action<UseableCollider>)Delegate.Combine(useCollider.onRelease,CachedOnRemove);
            visibles.Add(useCollider);
        }

        private void RemoveOne(UseableCollider useCollider)
        {
            visibles.Remove(useCollider);
        }

        private void OneExit(UseableCollider useCollider)
        {
            if (visibles.Contains(useCollider))
            {
                useCollider.onRelease =
                    (Action<UseableCollider>) Delegate.Remove(useCollider.onRelease, CachedOnRemove);
                visibles.Remove(useCollider);
            }
        }

        private void ReleaseAll()
        {
            if (null != visibles)
            {
                foreach (var one in visibles)
                {
                    one.onRelease = (Action<UseableCollider>)Delegate.Remove(one.onRelease,CachedOnRemove);
                }
                visibles.Clear();
            }
        }

        private void SetTarget(UseableCollider target)
        {
            if(target == currentTarget)
                return;
            currentTarget = target;
            if (null != onTargetSet)
            {
                int id = -1;
                int useTag = -1;
                if (null != target)
                {
                    id = target.GetId();
                    useTag = target.useTag;
                }

                onTargetSet(id, useTag);
            }
        }
    }
}
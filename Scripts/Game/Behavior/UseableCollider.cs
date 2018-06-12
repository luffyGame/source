using System;
using UnityEngine;

namespace Game
{
    public class UseableCollider : ObjCollider
    {
        public float radius
        {
            get
            {
                if (null == collider)
                    return 0f;
                return ((SphereCollider) collider).radius;
            }
        }
        public int useTag;
        public Action<UseableCollider> onRelease;
        public bool useable;

        public void Release()
        {
            if (null != onRelease)
                onRelease(this);
            onRelease = null;
            useable = false;
        }
    }
}
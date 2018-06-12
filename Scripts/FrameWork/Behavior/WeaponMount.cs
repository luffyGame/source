using System;
using System.Collections.Generic;
using UnityEngine;

namespace FrameWork
{
    public class WeaponMount : MonoBehaviour
    {
        [Serializable]
        public class MountSet
        {
            public string mountBone;
            public Vector3 mountPos, mountScale;
            public Quaternion mountRotate;
        }

        public Transform fireMount;
        public MountSet set;
        public bool IsWeaponMount
        {
            get { return set != null; }
        }
        private Transform trans;
        public Transform Trans {get
        {
            if (null == trans) trans = transform;
            return trans;
        }}
        [ContextMenu("WeaponSet")]
        private void SetCfg()
        {
            if(null == set)
                set = new MountSet();
            if (null != Trans.parent)
            {
                set.mountBone = Trans.parent.name;
                set.mountPos = Trans.localPosition;
                set.mountScale = Trans.localScale;
                set.mountRotate = Trans.localRotation;
            }

            fireMount = Trans.FindRecursive("fire");
        }

        public void Mount(Dictionary<string,Transform> bones)
        {
            if(null == set)
                return;
            if (bones.ContainsKey(set.mountBone))
            {
                Transform mountTo = bones[set.mountBone];
                Trans.parent = mountTo;
                Trans.localPosition = set.mountPos;
                Trans.localRotation = set.mountRotate;
                Trans.localScale = set.mountScale;
            }
        }
    }
}
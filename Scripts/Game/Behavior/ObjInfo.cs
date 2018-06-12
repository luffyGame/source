using FrameWork;
using UnityEngine;

namespace Game
{
    public abstract class ObjInfo : MonoBehaviour
    {
        public abstract ObjCollider objCollider { get; }
        public int id;
        public ObjBase objRef { get; set; }

        public int ObjId
        {
            get
            {
                if (null != objRef)
                    return objRef.oid;
                return -1;
            }
        }

        private Transform trans;
        public Transform Trans {get
        {
            if (null == trans) trans = transform;
            return trans;
        }}
        public void EnableCollider(bool enable)
        {
            if(null!=objCollider)
                objCollider.SetActive(enable);
        }

        public Collider GetCollider()
        {
            return null != objCollider ? objCollider.collider : null;
        }

        public Vector3 GetColliderCenter()
        {
            if (null == objCollider)
                return Trans.position;
            return objCollider.Trans.position;
        }

        public virtual void Release()
        {
            id = -1;
            objRef = null;
        }
    }
}
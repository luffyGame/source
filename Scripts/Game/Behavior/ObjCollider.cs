using UnityEngine;

namespace Game
{
    public class ObjCollider : MonoBehaviour
    {
        public Collider collider;//collider不限制
        public ObjInfo info;//挂载的info，碰撞通过它能拿到碰撞体的游戏逻辑
        private GameObject cachedGo;
        public GameObject Go 
        {
            get
            {
                if (null == cachedGo) cachedGo = gameObject;
                return cachedGo;
            }
        }
        private Transform trans;
        public Transform Trans 
        {
            get
            {
                if (null == trans) trans = transform;
                return trans;
            }
        }

        public int GetId()
        {
            return info.id;
        }

        public ObjBase GetObj()
        {
            return info.objRef;
        }

        public bool IsValid()
        {
            return null != info && null != info.objRef;
        }

        public void SetActive(bool b)
        {
            if(Go.activeSelf != b)
                Go.SetActive(b);
        }
    }
}
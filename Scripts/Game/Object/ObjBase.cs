using System;
using FrameWork;
using UnityEngine;

namespace Game
{
    public enum ObjType : int
    {
        O_BASIC = 0,
        O_SPRITE = 1,
        O_SCENE_ITEM = 2,
        O_EFFECT = 3,
        O_EQUIP = 4,
        O_BUILD = 5,
    }
    //场景对象的基类
    //Obj->info(策划美术配的挂在模型上的一些配置数据)->objCollider（本体的collider)
    public abstract class ObjBase
    {
        #region Var
        private static int s_objid = 0;
        public int oid { get; protected set; }
        public abstract ObjType objType { get; }
        public AssetModel rootModel { get; protected set; }
        public virtual ObjInfo info
        {
            get { return null; }
        }
        #endregion
        protected ObjBase()
        {
            oid = ++s_objid;
        }
		public abstract void Load(Action done = null);
        public virtual void Release()
        {
            if (null != info)
            {
                info.Release();
            }
            if (null != rootModel)
            {
                rootModel.Release();
                rootModel = null;
            }
        }
        public Transform GetRootTrans()
        {
            if (null != rootModel)
                return rootModel.trans;
            return null;
        }

        public GameObject GetRootGameObj()
        {
            if (null != rootModel)
                return rootModel.gameObj;
            return null;
        }
        public bool IsLoaded() { return null != rootModel; }

        public void SetPos(float x, float y, float z, bool local = false)
        {
            this.SetPos(new Vector3(x,y,z),local);
        }
        public void SetPos(Vector3 pos,bool local = false)
        {
            if (null != rootModel)
                rootModel.SetPos(pos,local);
        }
        public Vector3 GetPos(bool local = false)
        {
            return null == rootModel ? Vector3.zero : rootModel.GetPos(local);
        }

        public void GetPos(bool local, out float x, out float y, out float z)
        {
            Vector3 pos = GetPos(local);
            x = pos.x;
            y = pos.y;
            z = pos.z;
        }

        public void SetRot(float x, float y, float z, bool local = false)
        {
            this.SetRot(new Vector3(x,y,z),local);
        }
        public void SetRot(Vector3 rot,bool local = false)
        {
            if (null != rootModel)
                rootModel.SetRot(rot, local);
        }
        public void Rotate(Vector3 axis, float angle)
        {
            if (null != rootModel)
                rootModel.Rotate(axis, angle);
        }
        public void SetRot(Quaternion quat, bool local = false)
        {
            if (null != rootModel)
            {
                rootModel.SetRot(quat, local);
            }
        }
        public Quaternion GetRot(bool local = false)
        {
            if (null != rootModel)
            {
                return rootModel.GetRot(local);
            }
            return Quaternion.identity;
        }

        public void SetScale(float x, float y, float z, bool local = false)
        {
            if(null!=rootModel)
                rootModel.SetScale(new Vector3(x,y,z),local);
        }

        public void Translate(Vector3 mov)
        {
            if(null!=rootModel)
                rootModel.Translate(mov);
        }
        public void LocSameAsTransform(Transform trans)
        {
            SetPos(trans.position);
            SetRot(trans.rotation);
        }
        public Vector3 GetForward()
        {
            return null == rootModel ? Vector3.zero : rootModel.GetForward();
        }

        public void SetForward(Vector3 forward)
        {
            if(null!=rootModel)
                rootModel.SetForward(forward);
        }

        public void SetForward(float x, float y, float z)
        {
            SetForward(new Vector3(x,y,z));
        }
        public Vector3 GetRight()
        {
            return null == rootModel ? Vector3.zero : rootModel.GetRight();
        }

        public Vector3 TransformPoint(Vector3 pos)
        {
            return null == rootModel ? pos : rootModel.TransformPoint(pos);
        }

        public Vector3 InverseTransformPoint(Vector3 pos)
        {
            if (null != rootModel)
                return rootModel.InverseTransformPoint(pos);
            return pos;
        }

        public Vector3 TransformDirection(Vector3 dir)
        {
            return null == rootModel ? dir : rootModel.TransformDirection(dir);
        }

        public Vector3 InverseTransformDirection(Vector3 dir)
        {
            if (null != rootModel)
                return rootModel.InverseTransformDirection(dir);
            return dir;
        }

        public bool IsNear(ObjBase other,float dist)
        {
            return IsNear(other.GetPos(),dist);
        }
        public bool IsNear(Vector3 other, float dist)
        {
            Vector3 delta = other - GetPos();
            return delta.magnitude <= dist;
        }
        public float CalcDist(Vector3 other)
        {
            Vector3 delta = other - GetPos();
            return delta.magnitude;
        }
        public float CalcDistSquare(Vector3 other)
        {
            Vector3 delta = other - GetPos();
            return delta.sqrMagnitude;
        }
        public void LookAtH(ObjBase other)
        {
            Vector3 dir = other.GetPos() - GetPos();
            dir.y = 0f;
            SetRot(Quaternion.LookRotation(dir));
        }
        public void SetVisible(bool bVisible)
        {
            if (null != rootModel)
                rootModel.SetVisible(bVisible);
        }
        public bool IsVisible()
        {
            return null != rootModel && rootModel.IsVisible();
        }
        public void SetParent(Transform parent, bool worldPosStay = false)
        {
            if (null != rootModel)
            {
                rootModel.SetParent(parent, worldPosStay);
            }
        }
        public Vector3 GetBodyCenterPos()
        {
            if(null == info)
                return GetPos();
            return info.GetColliderCenter();
        }
        public virtual Bounds GetBounds()
        {
            return new Bounds(GetPos(), Vector3.zero);
        }
        public Bounds GetBoundsAt(Vector3 at)
        {
            Bounds b = GetBounds();
            at.y += b.extents.y;
            b.center = at;
            return b;
        }
        public void SetObjInfo(int id)
        {
            if (null != info)
            {
                info.id = id;
            }
        }

        public void InfoRef()
        {
            if (null != info)
                info.objRef = this;
        }
        public void ColliderEnable(bool bEnable)
        {
            if(null!=info)
                info.EnableCollider(bEnable);
        }
        public Collider GetCollider()
        {
            return null==info?null: info.GetCollider();
        }

        public void SetRendererLayer(int layer)
        {
            var allRenderers = rootModel.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in allRenderers)
            {
                renderer.gameObject.layer = layer;
            }
        }
    }
}
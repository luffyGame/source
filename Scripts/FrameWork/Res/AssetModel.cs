using System;
using System.Collections.Generic;
using UnityEngine;

namespace FrameWork
{
    public class AssetModel
    {
        #region Var
        protected readonly string modelRes;
        public string ModelRes
        {
            get { return modelRes; }
        }

        public GameObject gameObj { get; protected set; }
        public Transform trans { get; protected set;}
        private ulong gcbid;
        private static string EMPTY_RES = string.Empty;
        public bool IsLoaded
        {
            get { return gameObj != null; }
        }
        #endregion
        public AssetModel(string modelRes)
        {
            this.modelRes = modelRes??EMPTY_RES;
        }
        #region Public Method
        public void Load(Action onLoaded = null)
        {
            gcbid = GameObjPool.Instance.GetGameObj(modelRes, (go, cbId) =>
            {
                gcbid = 0;
                gameObj = go;
                if (null != go)
                {
                    trans = go.transform;
                    go.SetActive(true);
                }

                if(null!=onLoaded)
                    onLoaded();
            });
        }
        public void LoadSync()
        {
            if (string.IsNullOrEmpty(modelRes))
            {
                gameObj = new GameObject();
                trans = gameObj.transform;
            }
            UnityEngine.Object uobj = BundleMgr.Instance.GetAssetSync(modelRes);
            gameObj = GameObject.Instantiate<GameObject>(uobj as GameObject);
#if UNITY_EDITOR
            ApplyShader.CheckShader(gameObj).Start();
#endif
            trans = gameObj.transform;
        }
        public void Release()
        {
            trans = null;
            if (null != gameObj)
            {
                if(null!=GameObjPool.Instance)
                    GameObjPool.Instance.UnuseGameObj(this.modelRes, gameObj);
                else
                {
                    GameObject.Destroy(gameObj);
                }
                gameObj = null;
            }
            if (gcbid > 0)
            {
                if(null!=GameObjPool.Instance)
                    GameObjPool.Instance.CancelUngotGameObj(gcbid, true);
                gcbid = 0;
            }
        }
        public void SetPos(Vector3 pos, bool local = false)
        {
            if(null!=trans)
            {
                if (local)
                    trans.localPosition = pos;
                else
                    trans.position = pos;
            }
        }

        public Vector3 GetPos(bool isLocal = false)
        {
            if(null!=trans)
            {
                if (isLocal)
                    return trans.localPosition;
                else
                    return trans.position;
            }
            return Vector3.zero;
        }
        public void SetRot(Vector3 rot,bool local = false)
        {
            if (null != trans)
            {
                if (local)
                    trans.localEulerAngles = rot;
                else
                    trans.eulerAngles = rot;
            }
        }
        public void SetRot(Quaternion quat,bool local = false)
        {
            if (null != trans)
            {
                if (local)
                    trans.localRotation = quat;
                else
                    trans.rotation = quat;
            }
        }
        public void SetScale(Vector3 scale, bool local = false)
        {
            if(null!=trans)
            {
                if (local)
                    trans.localScale = scale;
                else
                    trans.SetScale(scale);
            }
        }
        public void Rotate(Vector3 axis,float angle)
        {
            if (null != trans)
                trans.Rotate(axis, angle);
        }
        public Quaternion GetRot(bool local = false)
        {
            if (null != trans)
            {
                return local? trans.localRotation: trans.rotation;
            }
            return Quaternion.identity;
        }
        public Vector3 GetForward()
        {
            if (null != trans)
                return trans.forward;
            return Vector3.zero;
        }

        public void SetForward(Vector3 forward)
        {
            if (null != trans)
                trans.forward = forward;
        }

        public void Translate(Vector3 mov)
        {
            if(null!=trans)
                trans.Translate(mov,Space.World);
        }

        public Vector3 GetRight()
        {
            return null == trans ? Vector3.zero : trans.right;
        }
        public void SetParent(AssetModel parent, bool worldPosStay = false)
        {
            SetParent(parent.trans, worldPosStay);
        }
        public void SetParent(Transform parent, bool worldPosStay = false)
        {
            if (null != trans)
            {
                if(worldPosStay)
                    trans.SetParent(parent,true);
                else
                {
                    trans.SetParentIndentical(parent);
                }
            }
        }
        public void SetName(string name)
        {
            if (null != gameObj)
                gameObj.name = name;
        }
        public T AddComponent<T>() where T : Component
        {
            if (null != gameObj)
                return gameObj.AddComponent<T>();
            return null;
        }
        public void RemoveComponent<T>(T comp) where T : Component
        {
            UnityEngine.Object.Destroy(comp);
        }

        public Component GetComponent(Type type)
        {
            if (null != gameObj)
                return gameObj.GetComponent(type);
            return null;
        }
        public T GetComponent<T>() where T : Component
        {
            if (null != gameObj)
                return gameObj.GetComponent<T>();
            return null;
        }

        public T GetOrAddComponent<T>() where T : Component
        {
            T comp = null;
            if (null != gameObj)
            {
                comp = gameObj.GetComponent<T>();
                if (null == comp)
                    comp = gameObj.AddComponent<T>();
            }
            return comp;
        }

        public T GetComponentInChildren<T>() where T : Component
        {
            if (null != gameObj)
                return gameObj.GetComponentInChildren<T>();
            return null;
        }
        
        public T[] GetComponentsInChildren<T>() where T : Component
        {
            if (null != gameObj)
                return gameObj.GetComponentsInChildren<T>();
            return null;
        }
        
        public Vector3 TransformPoint(Vector3 pos)
        {
            if (null == trans)
                return pos;
            return trans.TransformPoint(pos);
        }

        public Vector3 InverseTransformPoint(Vector3 pos)
        {
            if (null != trans)
                return trans.InverseTransformPoint(pos);
            return pos;
        }

        public Vector3 TransformDirection(Vector3 dir)
        {
            if (null == trans)
                return dir;
            return trans.TransformDirection(dir);
        }

        public Vector3 InverseTransformDirection(Vector3 dir)
        {
            if (null != trans)
                return trans.InverseTransformDirection(dir);
            return dir;
        }
        public void SetVisible(bool bVisible)
        {
            if (null != gameObj)
                gameObj.SetActive(bVisible);
        }
        public bool IsVisible()
        {
            if (null != gameObj)
                return gameObj.activeSelf;
            return false;
        }
        public void LookAt(Transform target)
        {
            if (null != trans)
            {
                trans.LookAt(target);
            }
        }
        #endregion
    }
}

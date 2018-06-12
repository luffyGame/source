using System;
using System.Collections.Generic;
using UnityEngine;

namespace FrameWork.Tools
{
    public class ColliderMod : MonoBehaviour
    {
        [ContextMenu("AddCollider")]
        public void AddCollider()
        {
            MeshFilter[] filters = GetComponentsInChildren<MeshFilter>();
            for (int i = 0; i < filters.Length; ++i)
            {
                MeshFilter mf = filters[i];
                if (mf.sharedMesh != null)
                {
                    Collider collider = mf.GetComponent<Collider>();
                    if(null==collider)
                    {
                        MeshCollider mc = mf.gameObject.AddComponent<MeshCollider>();
                        mc.sharedMesh = mf.sharedMesh;
                    }
                }
            }
            Debug.Log("AddCollider done");
        }

        [ContextMenu("RemoveCollider")]
        public void RemoveCollider()
        {
            MeshFilter[] filters = GetComponentsInChildren<MeshFilter>();
            for (int i = 0; i < filters.Length; ++i)
            {
                MeshFilter mf = filters[i];
                if (mf.sharedMesh != null)
                {
                    Collider collider = mf.GetComponent<Collider>();
                    if (null != collider)
                    {
                        UnityEngine.Object.DestroyImmediate(collider);
                    }
                }
            }
            Debug.Log("RemoveCollider done");
        }
    }
}

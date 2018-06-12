using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FrameWork.Editor
{
    [CustomEditor(typeof(Helper))]
    public class HelperEditor : UnityEditor.Editor
    {
        public Helper script { get; private set; }
        public void OnEnable()
        {
            script = target as Helper;
        }
        public void OnSceneGUI()
        {
            if (Event.current.type == EventType.MouseDown)
            {
                if (Event.current.button == 1)
                {
                    System.Object hit = HandleUtility.RaySnap(HandleUtility.GUIPointToWorldRay(Event.current.mousePosition));
                    if (null != hit)
                    {
                        var rayHit = (RaycastHit)hit;
                        Selection.activeGameObject = rayHit.collider.gameObject;
                        EditorGUIUtility.PingObject(rayHit.collider.gameObject);
                    }
                }
            }
        }
    }
}

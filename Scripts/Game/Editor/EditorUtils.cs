using System;
using System.Collections.Generic;
using System.IO;
using FrameWork;
using FrameWork.Editor;
using UnityEditor;
using UnityEngine;

namespace Game
{
    public static class EditorUtils
    {
        //设置使用碰撞体
        public static UseableCollider AddUsableCollider(Transform root,
            ObjInfo info, int useTag, bool defaultEnable)
        {
            UseableCollider useableCollider = null;
            SetUsableCollider(ref useableCollider,root,info,useTag,defaultEnable,true);
            return useableCollider;
        }
        public static void SetUsableCollider(ref UseableCollider useableCollider,Transform root,
            ObjInfo info,int useTag,bool defaultEnable,bool create = false)
        {
            if (useableCollider == null)
            {
                Transform colliderTrans = null;
                if(!create)
                    colliderTrans = root.Find("useable");
                if (null == colliderTrans)
                {
                    GameObject go = new GameObject("useable");
                    colliderTrans = go.transform;
                    colliderTrans.SetParentIndentical(root);
                }
                useableCollider = colliderTrans.GetComponent<UseableCollider>();
                if (useableCollider == null)
                    useableCollider = colliderTrans.gameObject.AddComponent<UseableCollider>();
            }
            useableCollider.collider = useableCollider.gameObject.GetComponent<SphereCollider>();
            if (useableCollider.collider == null)
            {
                useableCollider.collider = useableCollider.gameObject.AddComponent<SphereCollider>();
            }
            useableCollider.gameObject.SetActive(defaultEnable);
            useableCollider.useable = defaultEnable;
            useableCollider.gameObject.layer = Const.LAYER_USABLE;
            useableCollider.info = info;
            useableCollider.useTag = useTag;
        }

        public static void DrawUsableColliderInfo(ref UseableCollider useableCollider)
        {
            EditorGUI.indentLevel += 2;
            EditorGUILayout.ObjectField("碰撞体",
                useableCollider,
                typeof(ObjCollider), true);
            useableCollider.gameObject.layer = EditorGUILayout.LayerField("碰撞体层级", useableCollider.gameObject.layer, GUILayout.ExpandWidth(true));
            EditorGUILayout.ObjectField("信息体", useableCollider.info, typeof(ObjInfo), true);
            EditorGUILayout.ObjectField("Collider", useableCollider.collider, typeof(CapsuleCollider), true);
            EditorGUILayout.FloatField("使用半径", useableCollider.radius);
            useableCollider.useTag = EditorGUILayout.IntField("使用标记", useableCollider.useTag);
            useableCollider.gameObject.SetActive(
                EditorGUILayout.Toggle("默认开启", useableCollider.gameObject.activeSelf));
            useableCollider.useable = EditorGUILayout.Toggle("当前可用", useableCollider.useable);
            EditorGUI.indentLevel -= 2;
        }
        
        public static void DrawNoCollider()
        {
            EditorGUILayout.LabelField("没有设置碰撞体",GUILayout.Width(120));
        }
        
        public static void DrawApply(GameObject go, UnityEngine.Object targetPrefab)
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUI.color = Color.cyan;
            if (GUILayout.Button("应用到Prefab",GUILayout.Width(120f)))
            {
                Selection.activeGameObject = PrefabUtility.ReplacePrefab(go, targetPrefab, ReplacePrefabOptions.ConnectToPrefab);
            }

            GUI.color = Color.white;
            GUILayout.EndHorizontal();
        }

        public static void SetAnimation(GameObject go,string aniFolder)
        {
            Animation animation = go.GetComponent<Animation>();
            if (null == animation)
                animation = go.AddComponent<Animation>();
            List<FileInfo> files = Util.GetFiles(Application.dataPath +"//" + aniFolder, new []{".anim"});
            if (files.Count == 0) return;
            for (int i = 0; i < files.Count; i++)
            {
                FileInfo f = files[i];
                string assetPath = f.FullName.Substring(f.FullName.IndexOf("Assets"));
                UnityEngine.Object asset = AssetDatabase.LoadMainAssetAtPath(assetPath);
                AnimationClip clip = asset as AnimationClip;
                if(null!=clip)
                    animation.AddClip(clip,clip.name);
            }
        }

        public static void DrawAniSet(GameObject go,ref string aniFolder)
        {
            GUILayout.BeginHorizontal();
            aniFolder = EditorGUILayout.TextField("动画目录", aniFolder);
            if (GUILayout.Button("设置", GUILayout.Width(120f)))
            {
                SetAnimation(go,aniFolder);
            }
            GUILayout.EndHorizontal();
        }
        
        public static void DrawNewScene(GameObject prefab,Action release,Action<GameObject> setPrefab)
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUI.color = Color.cyan;
            bool newScene = GUILayout.Button("新建场景", GUILayout.Width(120f));
            GUI.color = Color.white;
            GUILayout.EndHorizontal();
            if (newScene)
            {
                GameObject oldPrefab = prefab;
                release();
                ShowUtil.NewScene();
                setPrefab(oldPrefab);
            }
        }
        
        public static void DrawPrefabInfo(GameObject prefab)
        {
            ShowUtil.DrawTitle("当前物体信息",Color.cyan);
            ShowUtil.DrawLine();
            EditorGUI.indentLevel += 2;
            EditorGUILayout.LabelField("模型名：" + (prefab==null?"无": prefab.name),GUILayout.ExpandWidth(true));
            EditorGUI.indentLevel -= 2;
        }
    }
}
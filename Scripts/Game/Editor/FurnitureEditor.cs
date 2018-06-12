using System;
using System.Collections.Generic;
using FrameWork;
using FrameWork.Editor;
using UnityEditor;
using UnityEngine;

namespace Game
{
    public class FurnitureEditor: EditorWindow
    {
        private GameObject prefab;
        private GameObject furniture;
        private FurnitureInfo furnitureInfo;
        
        private Vector2 scrollPosition;
        [MenuItem("FrameWork/预制体/建筑编辑",false,110003)]
        static void ShowCharacterEditor()
        {
            Type gameView = ShowUtil.GetGameViewType();
            var window = GetWindow<FurnitureEditor>("建筑编辑",true,gameView);            
            window.Show();
        }
        private void OnDestroy()
        {
            Release();
        }

        private void OnGUI()
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);
            DrawNewScene();
            ShowUtil.DrawLine();
            DrawPrefabInfo();
            ShowUtil.DrawLine();
            if (null == furnitureInfo)
            {
                DrawNoFurnitureInfo();
            }
            else
            {
                DrawFurnitureInfo();    
            }
            GUILayout.EndScrollView();
        }

        private void OnSelectionChange()
        {
            SetFurniture(Selection.activeGameObject);
            Repaint();
        }

        private void SetFurniture(GameObject go)
        {
            if(go == null||go==prefab || go == furniture)
                return;;
            if (PrefabUtility.GetPrefabType(go) == PrefabType.None)
                return;
            Release();
            prefab = go;
            furniture = Instantiate(prefab,Vector3.zero,Quaternion.identity);
            furniture.name = prefab.name;
            furniture.transform.localScale = Vector3.one;
            GetFurnitureSetting();
        }

        private void Release()
        {
            prefab = null;
            if (null != furniture)
            {
                DestroyImmediate(furniture);
                furniture = null;
            }
            furnitureInfo = null;
        }

        private void GetFurnitureSetting()
        {
            if(null == furniture)
                return;
            furnitureInfo = furniture.GetComponent<FurnitureInfo>();
        }
        
        private void DrawFurnitureInfo()
        {
            DrawComponentInfo();
            ShowUtil.DrawLine();
            DrawFurnitureColliderInfo();
            ShowUtil.DrawLine();
            DrawUsableColliderInfo();
            ShowUtil.DrawLine();
            DrawObstacleInfo();
            ShowUtil.DrawLine();
            DrawOtherInfo();
            ShowUtil.DrawLine();
            EditorUtils.DrawApply(furniture,prefab);
        }

        private void DrawNoFurnitureInfo()
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("没有挂载建筑物体脚本",GUILayout.Width(120));
            if (GUILayout.Button("挂载",GUILayout.Width(120f)))
            {
                if (null != furniture)
                {
                    furnitureInfo = furniture.AddComponent<FurnitureInfo>();
                }
            }
            GUILayout.EndHorizontal();
        }

        private void DrawComponentInfo()
        {
            ShowUtil.DrawTitle("组件",Color.white);
        }

        
        private void DrawFurnitureColliderInfo()
        {
            ShowUtil.DrawTitle("代表本体的碰撞信息",Color.magenta);
            ShowUtil.DrawLine();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("重置",GUILayout.Width(120f)))
            {
                SetFurnitureCollider();
            }
            GUILayout.EndHorizontal();
            if (furnitureInfo.furnitureCollider == null)
            {
                EditorUtils.DrawNoCollider();
                return;
            }
            FurnitureCollider furnitureCollider = furnitureInfo.furnitureCollider;
            EditorGUI.indentLevel += 2;
            EditorGUILayout.ObjectField("碰撞体",
                furnitureCollider,
                typeof(FurnitureCollider), true);
            furnitureCollider.gameObject.layer = EditorGUILayout.LayerField("碰撞体层级", furnitureCollider.gameObject.layer, GUILayout.ExpandWidth(true));
            EditorGUILayout.ObjectField("信息体", furnitureCollider.info, typeof(ObjInfo), true);
            EditorGUI.indentLevel -= 2;
        }
        
        private void SetFurnitureCollider()
        {
            FurnitureCollider furnitureCollider = furnitureInfo.furnitureCollider;
            if (furnitureCollider == null)
            {
                Transform colliderTrans = furniture.transform.Find("collider");
                if (null == colliderTrans)
                {
                    GameObject go = new GameObject("collider");
                    colliderTrans = go.transform;
                    colliderTrans.SetParentIndentical(furniture.transform);
                }
                furnitureCollider = colliderTrans.GetComponent<FurnitureCollider>();
                if (furnitureCollider == null)
                    furnitureCollider = colliderTrans.gameObject.AddComponent<FurnitureCollider>();
                furnitureInfo.furnitureCollider = furnitureCollider;
            }
            furnitureCollider.collider = furnitureCollider.gameObject.GetComponent<BoxCollider>();
            if (furnitureCollider.collider == null)
            {
                furnitureCollider.collider = furnitureCollider.gameObject.AddComponent<BoxCollider>();
            }

            if (furnitureInfo.furnitureType == FurnitureType.furniture)
            {
                furnitureCollider.gameObject.layer = Const.LAYER_FURNITURE_COLLIDER;
            }
            else
            {
                furnitureCollider.gameObject.layer = Const.LAYER_BUILDING_COLLIDER;
            }

            furnitureCollider.info = furnitureInfo;
        }

        private void DrawUsableColliderInfo()
        {
            ShowUtil.DrawTitle("使用的碰撞信息",Color.magenta);
            ShowUtil.DrawLine();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("添加",GUILayout.Width(120f)))
            {
                UseableCollider collider = EditorUtils.AddUsableCollider(furniture.transform,furnitureInfo,3,true);
                List<UseableCollider> newColliders = new List<UseableCollider>(furnitureInfo.usableColliders);
                newColliders.Add(collider);
                furnitureInfo.usableColliders = newColliders.ToArray();
            }
            GUILayout.EndHorizontal();
            
            if (furnitureInfo.usableColliders == null || furnitureInfo.usableColliders.Length == 0)
            {
                EditorUtils.DrawNoCollider();
                return;
            }
            for (int i = 0; i < furnitureInfo.usableColliders.Length; ++i)
            {
                ShowUtil.DrawLine();
                EditorUtils.DrawUsableColliderInfo(ref furnitureInfo.usableColliders[i]);
            }
            
        }
        
        private void DrawObstacleInfo()
        {
            ShowUtil.DrawTitle("障碍信息",Color.yellow);
        }
        
        private void DrawOtherInfo()
        {
            ShowUtil.DrawTitle("其他配置",Color.yellow);
            
            EditorGUIUtility.labelWidth = 250f;
            EditorGUI.indentLevel += 2;
            furnitureInfo.furnitureType = (FurnitureType)EditorGUILayout.EnumPopup("建筑类型", furnitureInfo.furnitureType);
            furnitureInfo.cellWidth = EditorGUILayout.IntField("格子长度", furnitureInfo.cellWidth);
            furnitureInfo.cellHeight = EditorGUILayout.IntField("格子宽度", furnitureInfo.cellHeight);
            furnitureInfo.direction = (FurnitureDirect)EditorGUILayout.EnumPopup("默认朝向", furnitureInfo.direction);
            EditorGUI.indentLevel -= 2;
        }

        private void DrawNewScene()
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
                Release();
                ShowUtil.NewScene();
                SetFurniture(oldPrefab);
            }
        }

        private void DrawPrefabInfo()
        {
            ShowUtil.DrawTitle("当前物体信息",Color.cyan);
            ShowUtil.DrawLine();
            EditorGUI.indentLevel += 2;
            EditorGUILayout.LabelField("模型名：" + (prefab==null?"无": prefab.name),GUILayout.ExpandWidth(true));
            EditorGUI.indentLevel -= 2;
        }
    }
}
using System;
using FrameWork;
using FrameWork.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace Game
{
    public class SceneItemEditor : EditorWindow
    {
        private GameObject prefab;
        private GameObject item;
        private SceneItemInfo itemInfo;
        
        private Vector2 scrollPosition;
        [MenuItem("FrameWork/预制体/场景物体编辑",false,110002)]
        static void ShowCharacterEditor()
        {
            Type gameView = ShowUtil.GetGameViewType();
            var window = GetWindow<SceneItemEditor>("场景物体编辑",true,gameView);            
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
            if (null == itemInfo)
            {
                DrawNoItemInfo();
            }
            else
            {
                DrawItemInfo();    
            }
            GUILayout.EndScrollView();
        }

        private void OnSelectionChange()
        {
            SetItem(Selection.activeGameObject);
            Repaint();
        }

        private void SetItem(GameObject go)
        {
            if(go == null||go==prefab || go == item)
                return;;
            if (PrefabUtility.GetPrefabType(go) == PrefabType.None)
                return;
            Release();
            prefab = go;
            item = Instantiate(prefab,Vector3.zero,Quaternion.identity);
            item.name = prefab.name;
            item.transform.localScale = Vector3.one;
            GetItemSetting();
        }

        private void Release()
        {
            prefab = null;
            if (null != item)
            {
                DestroyImmediate(item);
                item = null;
            }
            itemInfo = null;
        }

        private void GetItemSetting()
        {
            if(null == item)
                return;
            itemInfo = item.GetComponent<SceneItemInfo>();
        }
        
        private void DrawItemInfo()
        {
            DrawComponentInfo();
            ShowUtil.DrawLine();
            DrawItemColliderInfo();
            ShowUtil.DrawLine();
            DrawUseableColliderInfo();
            ShowUtil.DrawLine();
            DrawObstacleInfo();
            ShowUtil.DrawLine();
            EditorUtils.DrawApply(item,prefab);
        }

        private void DrawNoItemInfo()
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("没有挂载场景物体脚本",GUILayout.Width(120));
            if (GUILayout.Button("挂载",GUILayout.Width(120f)))
            {
                if (null != item)
                {
                    itemInfo = item.AddComponent<SceneItemInfo>();
                }
            }
            GUILayout.EndHorizontal();
        }

        private void DrawComponentInfo()
        {
            ShowUtil.DrawTitle("组件",Color.white);
            EditorGUI.indentLevel += 2;
            GUILayout.BeginHorizontal();
            itemInfo.animation = (Animation)EditorGUILayout.ObjectField("动画", itemInfo.animation, typeof(Animation), true);
            if (GUILayout.Button("设置", GUILayout.Width(120f)))
            {
                itemInfo.animation = item.GetComponent<Animation>();
                if (null != itemInfo.animation)
                {
                    itemInfo.animation.playAutomatically = false;
                    itemInfo.animation.clip = null;
                }
            }
            GUILayout.EndHorizontal();
            EditorGUI.indentLevel -= 2;
        }

        
        private void DrawItemColliderInfo()
        {
            ShowUtil.DrawTitle("代表本体的碰撞信息",Color.magenta);
            ShowUtil.DrawLine();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("重置",GUILayout.Width(120f)))
            {
                SetItemCollider();
            }
            GUILayout.EndHorizontal();
            if (itemInfo.itemCollider == null)
            {
                EditorUtils.DrawNoCollider();
                return;
            }
            ObjCollider objCollider = itemInfo.itemCollider;
            EditorGUI.indentLevel += 2;
            EditorGUILayout.ObjectField("碰撞体",
                objCollider,
                typeof(ObjCollider), true);
            objCollider.gameObject.layer = EditorGUILayout.LayerField("碰撞体层级", objCollider.gameObject.layer, GUILayout.ExpandWidth(true));
            EditorGUILayout.ObjectField("信息体", objCollider.info, typeof(ObjInfo), true);
            EditorGUI.indentLevel -= 2;
        }
        
        private void SetItemCollider()
        {
            ObjCollider itemCollider = itemInfo.itemCollider;
            if (itemCollider == null)
            {
                Transform colliderTrans = item.transform.Find("collider");
                if (null == colliderTrans)
                {
                    GameObject go = new GameObject("collider");
                    colliderTrans = go.transform;
                    colliderTrans.SetParentIndentical(item.transform);
                }
                itemCollider = colliderTrans.GetComponent<ObjCollider>();
                if (itemCollider == null)
                    itemCollider = colliderTrans.gameObject.AddComponent<ObjCollider>();
                itemInfo.itemCollider = itemCollider;
            }
            itemCollider.collider = itemCollider.gameObject.GetComponent<CapsuleCollider>();
            if (itemCollider.collider == null)
            {
                itemCollider.collider = itemCollider.gameObject.AddComponent<CapsuleCollider>();
            }
            
            itemCollider.gameObject.layer = Const.LAYER_ITEM_COLLIDER;
            itemCollider.info = itemInfo;
        }
        
        private void DrawUseableColliderInfo()
        {
            ShowUtil.DrawTitle("代表被拾取的碰撞体信息",Color.magenta);
            ShowUtil.DrawLine();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("重置",GUILayout.Width(120f)))
            {
                EditorUtils.SetUsableCollider(ref itemInfo.useableCollider,item.transform,itemInfo,0,true);
                
            }
            GUILayout.EndHorizontal();
            if (itemInfo.useableCollider == null)
            {
                EditorUtils.DrawNoCollider();
                return;
            }
            EditorUtils.DrawUsableColliderInfo(ref itemInfo.useableCollider);
        }

        private void DrawObstacleInfo()
        {
            ShowUtil.DrawTitle("障碍信息",Color.yellow);
            ShowUtil.DrawLine();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("设置障碍",GUILayout.Width(120f)))
            {
                SetObstacle();
            }
            GUILayout.EndHorizontal();
            if (itemInfo.obstacle == null)
            {
                EditorGUILayout.LabelField("没有设置障碍",GUILayout.Width(120));
                return;
            }
            EditorGUI.indentLevel += 2;
            EditorGUILayout.ObjectField("障碍", itemInfo.obstacle, typeof(GameObject), true);
            Collider obstacleCollider = itemInfo.obstacle.GetComponent<Collider>();
            if(obstacleCollider == null)
                EditorGUILayout.LabelField("请设置障碍的Collider");
            EditorGUI.indentLevel -= 2;
        }

        private void SetObstacle()
        {
            GameObject obstacleObj = itemInfo.obstacle;
            if (null == obstacleObj)
            {
                Transform obstacleTrans = item.transform.Find("obstacle");
                if (null == obstacleTrans)
                {
                    obstacleObj = new GameObject("obstacle");
                    obstacleTrans = obstacleObj.transform;
                    obstacleTrans.SetParentIndentical(item.transform);
                }
                else
                {
                    obstacleObj = obstacleTrans.gameObject;
                }
                NavMeshObstacle navMeshObstacle = obstacleObj.GetComponent<NavMeshObstacle>();
                if (navMeshObstacle == null)
                    navMeshObstacle = obstacleObj.AddComponent<NavMeshObstacle>();
                navMeshObstacle.carving = true;
                navMeshObstacle.carveOnlyStationary = false;
                obstacleObj.layer = Const.LAYER_OBSTACLE;
                itemInfo.obstacle = obstacleObj;
            }
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
                SetItem(oldPrefab);
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
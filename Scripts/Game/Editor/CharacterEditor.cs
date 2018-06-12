using System;
using FrameWork;
using FrameWork.Editor;
using UnityEditor;
using UnityEngine;

namespace Game
{
    public class CharacterEditor : EditorWindow
    {
        private GameObject prefab;
        private GameObject charater;
        private SpriteInfo spriteInfo;

        private string aniFolder;
        private static readonly string DEFAULT_BONE_PRE = "Bip,Bone";
        private string bonePre = DEFAULT_BONE_PRE;
        
        private Vector2 scrollPosition;
        private static string[] SPRITE_HH = {"head", "body", "leg", "foot"};
        private static string[] SPRITE_HH_CHS = {"头", "身体", "腿", "脚"};
        [MenuItem("FrameWork/预制体/角色编辑",false,110001)]
        static void ShowCharacterEditor()
        {
            Type gameView = ShowUtil.GetGameViewType();
            var window = GetWindow<CharacterEditor>("精灵编辑",true,gameView);            
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
            DrawCharatorInfo();
            ShowUtil.DrawLine();
            if (null == spriteInfo)
            {
                DrawNoSpriteInfo();
            }
            else
            {
                DrawSpriteInfo();    
            }
            GUILayout.EndScrollView();
        }

        private void OnSelectionChange()
        {
            SetCharacter(Selection.activeGameObject);
            Repaint();
        }

        private void SetCharacter(GameObject go)
        {
            if(go == null||go==prefab || go == charater)
                return;;
            if (PrefabUtility.GetPrefabType(go) == PrefabType.None)
                return;
            Release();
            prefab = go;
            charater = Instantiate(prefab,Vector3.zero,Quaternion.identity);
            charater.name = prefab.name;
            charater.transform.localScale = Vector3.one;
            GetCharaterSetting();
        }

        private void Release()
        {
            prefab = null;
            if (null != charater)
            {
                DestroyImmediate(charater);
                charater = null;
            }
            spriteInfo = null;
            aniFolder = null;
            bonePre = DEFAULT_BONE_PRE;
        }

        private void GetCharaterSetting()
        {
            if(null == charater)
                return;
            spriteInfo = charater.GetComponent<SpriteInfo>();
            if(null == spriteInfo)
                return;
            if(null == spriteInfo.mounts||spriteInfo.mounts.Length!=4)
                spriteInfo.mounts = new Transform[4];
        }
        
        private void DrawSpriteInfo()
        {
            DrawComponentInfo();
            ShowUtil.DrawLine();
            DrawMountInfo();
            ShowUtil.DrawLine();
            DrawSpriteColliderInfo();
            ShowUtil.DrawLine();
            DrawUseableColliderInfo();
            ShowUtil.DrawLine();
            DrawOtherInfo();
            ShowUtil.DrawLine();
            EditorUtils.DrawApply(charater,prefab);
        }

        private void DrawNoSpriteInfo()
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("没有挂载精灵脚本",GUILayout.Width(120));
            if (GUILayout.Button("挂载",GUILayout.Width(120f)))
            {
                if (null != charater)
                {
                    spriteInfo = charater.AddComponent<SpriteInfo>();
                    spriteInfo.mounts = new Transform[4];
                }
            }
            GUILayout.EndHorizontal();
        }

        private void DrawComponentInfo()
        {
            ShowUtil.DrawTitle("角色组件",Color.white);
            EditorGUI.indentLevel += 2;
            GUILayout.BeginHorizontal();
            spriteInfo.animation = (Animation)EditorGUILayout.ObjectField("动画", spriteInfo.animation, typeof(Animation), true);
            if (GUILayout.Button("设置", GUILayout.Width(120f)))
            {
                spriteInfo.animation = charater.GetComponent<Animation>();
                if (null != spriteInfo.animation)
                {
                    spriteInfo.animation.playAutomatically = false;
                    spriteInfo.animation.clip = null;
                }
            }
            GUILayout.EndHorizontal();
            EditorUtils.DrawAniSet(charater,ref aniFolder);
            GUILayout.BeginHorizontal();
            spriteInfo.cct = (CharacterController)EditorGUILayout.ObjectField("移动控制", spriteInfo.cct, typeof(CharacterController), true);
            if (GUILayout.Button("设置", GUILayout.Width(120f)))
            {
                spriteInfo.cct = charater.GetComponent<CharacterController>();
                if (null == spriteInfo.cct)
                    spriteInfo.cct = charater.AddComponent<CharacterController>();
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            spriteInfo.follower = (NavFollower)EditorGUILayout.ObjectField("寻路", spriteInfo.follower, typeof(NavFollower), true);
            if (GUILayout.Button("设置", GUILayout.Width(120f)))
            {
                spriteInfo.follower = charater.GetComponent<NavFollower>();
                if (null == spriteInfo.follower)
                    spriteInfo.follower = charater.AddComponent<NavFollower>();
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            spriteInfo.equipWear = (EquipWear)EditorGUILayout.ObjectField("换装", spriteInfo.equipWear, typeof(Replacement), true);
            if (GUILayout.Button("设置", GUILayout.Width(120f)))
            {
                spriteInfo.equipWear = charater.GetComponent<EquipWear>();
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            spriteInfo.dismemberment = (Dismemberment)EditorGUILayout.ObjectField("肢解", spriteInfo.dismemberment, typeof(Dismemberment), true);
            if (GUILayout.Button("设置", GUILayout.Width(120f)))
            {
                spriteInfo.dismemberment = charater.GetComponent<Dismemberment>();
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            spriteInfo.ragdollManager = (RagdollManagerHum)EditorGUILayout.ObjectField("布娃娃", spriteInfo.ragdollManager, typeof(RagdollManagerHum), true);
            if (GUILayout.Button("设置", GUILayout.Width(120f)))
            {
                spriteInfo.ragdollManager = charater.GetComponent<RagdollManagerHum>();
            }
            GUILayout.EndHorizontal();
            EditorGUI.indentLevel -= 2;
        }

        private void DrawMountInfo()
        {
            ShowUtil.DrawTitle("角色挂点",Color.green);
            EditorGUI.indentLevel += 2;
            for (int i = 0; i < 4; ++i)
            {
                GUILayout.BeginHorizontal();
                spriteInfo.mounts[i] = (Transform) EditorGUILayout.ObjectField(SPRITE_HH_CHS[i],
                    spriteInfo.mounts[i],
                    typeof(Transform), true);
                if (GUILayout.Button("设置", GUILayout.Width(120f)))
                {
                    spriteInfo.mounts[i] = charater.transform.FindRecursive(SPRITE_HH[i]);
                }
                GUILayout.EndHorizontal();
            }
            EditorGUI.indentLevel -= 2;
        }
        private void DrawSpriteColliderInfo()
        {
            ShowUtil.DrawTitle("代表本体的碰撞信息",Color.magenta);
            ShowUtil.DrawLine();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("重置",GUILayout.Width(120f)))
            {
                SetSpriteCollider();
            }
            GUILayout.EndHorizontal();
            if (spriteInfo.spriteCollider == null)
            {
                EditorUtils.DrawNoCollider();
                return;
            }

            ObjCollider objCollider = spriteInfo.spriteCollider;
            EditorGUI.indentLevel += 2;
            EditorGUILayout.ObjectField("碰撞体",
                objCollider,
                typeof(ObjCollider), true);
            objCollider.gameObject.layer = EditorGUILayout.LayerField("碰撞体层级", objCollider.gameObject.layer, GUILayout.ExpandWidth(true));
            EditorGUILayout.ObjectField("信息体", objCollider.info, typeof(ObjInfo), true);
            EditorGUI.indentLevel -= 2;
        }
        
        private void DrawUseableColliderInfo()
        {
            ShowUtil.DrawTitle("代表被拾取的碰撞体信息",Color.magenta);
            ShowUtil.DrawLine();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("重置",GUILayout.Width(120f)))
            {
                EditorUtils.SetUsableCollider(ref spriteInfo.useableCollider,charater.transform,spriteInfo,1,false);
                
            }
            GUILayout.EndHorizontal();
            if (spriteInfo.useableCollider == null)
            {
                EditorUtils.DrawNoCollider();
                return;
            }
            EditorUtils.DrawUsableColliderInfo(ref spriteInfo.useableCollider);
        }

        private void DrawOtherInfo()
        {
            ShowUtil.DrawTitle("其他配置",Color.yellow);
            
            EditorGUIUtility.labelWidth = 250f;
            EditorGUI.indentLevel += 2;
            //角色层级
            GUILayout.BeginHorizontal();
            charater.layer = EditorGUILayout.LayerField("角色层级", charater.layer);
            if (GUILayout.Button("设置", GUILayout.Width(120f)))
            {
                charater.layer = Const.LAYER_CHARACTER_MOVER;
            }
            GUILayout.EndHorizontal();
            //骨骼数
            bonePre = EditorGUILayout.TextField("骨骼前缀", bonePre);
            GUILayout.BeginHorizontal();
            EditorGUILayout.IntField("骨骼数", spriteInfo.boneCount);
            if (GUILayout.Button("设置", GUILayout.Width(120f)))
            {
                spriteInfo.boneCount = Utils.CalcBoneCount(charater, bonePre);
            }
            GUILayout.EndHorizontal();
            //面数
            GUILayout.BeginHorizontal();
            EditorGUILayout.IntField("模型面数", spriteInfo.trisCount);
            if (GUILayout.Button("设置", GUILayout.Width(120f)))
            {
                spriteInfo.trisCount = Utils.CalcTriCount(charater);
            }
            GUILayout.EndHorizontal();
            EditorGUI.indentLevel -= 2;
        }

        private void SetSpriteCollider()
        {
            ObjCollider spriteCollider = spriteInfo.spriteCollider;
            if (spriteCollider == null)
            {
                Transform colliderTrans = charater.transform.Find("collider");
                if (null == colliderTrans)
                {
                    GameObject go = new GameObject("collider");
                    colliderTrans = go.transform;
                    colliderTrans.SetParent(charater.transform);
                }
                spriteCollider = colliderTrans.GetComponent<ObjCollider>();
                if (spriteCollider == null)
                    spriteCollider = colliderTrans.gameObject.AddComponent<ObjCollider>();
                spriteInfo.spriteCollider = spriteCollider;
            }
            spriteCollider.collider = spriteCollider.gameObject.GetComponent<CapsuleCollider>();
            if (spriteCollider.collider == null)
            {
                spriteCollider.collider = spriteCollider.gameObject.AddComponent<CapsuleCollider>();
            }
            
            spriteCollider.gameObject.layer = Const.LAYER_MONSTER_COLLIDER;
            spriteCollider.info = spriteInfo;
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
                SetCharacter(oldPrefab);
            }
        }

        private void DrawCharatorInfo()
        {
            ShowUtil.DrawTitle("当前角色信息",Color.cyan);
            ShowUtil.DrawLine();
            EditorGUI.indentLevel += 2;
            EditorGUILayout.LabelField("模型名：" + (prefab==null?"无": prefab.name),GUILayout.ExpandWidth(true));
            EditorGUI.indentLevel -= 2;
        }
    }
}
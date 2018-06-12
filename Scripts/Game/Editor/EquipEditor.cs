using System;
using System.Collections.Generic;
using FrameWork;
using FrameWork.Editor;
using UnityEditor;
using UnityEngine;

namespace Game
{
    public class EquipEditor : EditorWindow
    {
        private GameObject prefab;
        private GameObject equip;
        private EquipInfo equipInfo;
        
        private Vector2 scrollPosition;
        
        [MenuItem("FrameWork/预制体/装备编辑",false,110004)]
        static void ShowEquipEditor()
        {
            Type gameView = ShowUtil.GetGameViewType();
            var window = GetWindow<EquipEditor>("装备编辑",true,gameView);            
            window.Show();
        }
        private void OnDestroy()
        {
            Release();
        }

        private void OnGUI()
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);
            EditorUtils.DrawNewScene(prefab,Release,SetEquip);
            ShowUtil.DrawLine();
            EditorUtils.DrawPrefabInfo(prefab);
            ShowUtil.DrawLine();
            if (null == equipInfo)
            {
                DrawNoEquipInfo();
            }
            else
            {
                DrawEquipInfo();    
            }
            GUILayout.EndScrollView();
        }

        private void OnSelectionChange()
        {
            SetEquip(Selection.activeGameObject);
            Repaint();
        }

        private void SetEquip(GameObject go)
        {
            if(go == null||go==prefab || go == equip)
                return;;
            if (PrefabUtility.GetPrefabType(go) == PrefabType.None)
                return;
            Release();
            prefab = go;
            equip = Instantiate(prefab,Vector3.zero,Quaternion.identity);
            equip.name = prefab.name;
            equip.transform.localScale = Vector3.one;
            GetEquipSetting();
        }

        private void Release()
        {
            prefab = null;
            if (null != equip)
            {
                DestroyImmediate(equip);
                equip = null;
            }
            equipInfo = null;
        }

        private void GetEquipSetting()
        {
            if(null == equip)
                return;
            equipInfo = equip.GetComponent<EquipInfo>();
        }
        
        private void DrawEquipInfo()
        {
            DrawBoneWearInfo();
            ShowUtil.DrawLine();
            DrawOtherInfo();
            ShowUtil.DrawLine();
            EditorUtils.DrawApply(equip,prefab);
        }

        private void DrawNoEquipInfo()
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("没有挂载装备脚本",GUILayout.Width(120));
            if (GUILayout.Button("挂载",GUILayout.Width(120f)))
            {
                if (null != equip)
                {
                    equipInfo = equip.AddComponent<EquipInfo>();
                }
            }
            GUILayout.EndHorizontal();
        }

        private void DrawBoneWearInfo()
        {
            ShowUtil.DrawTitle("骨骼装配方案",Color.magenta);
            ShowUtil.DrawLine();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("添加骨骼",GUILayout.Width(120f)))
            {
                List<EquipInfo.EquipSki> skis;
                if(equipInfo.skis == null)
                    skis = new List<EquipInfo.EquipSki>();
                else
                    skis = new List<EquipInfo.EquipSki>(equipInfo.skis);
                skis.Add(new EquipInfo.EquipSki());
                equipInfo.skis = skis.ToArray();
            }
            GUILayout.EndHorizontal();
            if(null == equipInfo.skis)
                return;
            for (int i = 0; i < equipInfo.skis.Length; ++i)
            {
                ShowUtil.DrawLine();
                DrawOneSki(equipInfo.skis[i]);
            }
        }

        private void DrawOneSki(EquipInfo.EquipSki ski)
        {
            EditorGUI.indentLevel += 2;
            ski.skiGo = (GameObject)EditorGUILayout.ObjectField("骨骼对象", ski.skiGo, typeof(GameObject), true);
            ski.render = (SkinnedMeshRenderer)EditorGUILayout.ObjectField("骨骼渲染", ski.render, typeof(SkinnedMeshRenderer), true);
            ski.conflit = (EquipMask)EditorGUILayout.EnumFlagsField("冲突层级", ski.conflit);
            EditorGUILayout.IntField("冲突层级Int值", (int) ski.conflit);
            ski.op = (EquipInfo.ConflitOp)EditorGUILayout.EnumPopup("冲突方案", ski.op);
            EditorGUI.indentLevel -= 2;
        }
        
        private void DrawOtherInfo()
        {
            ShowUtil.DrawTitle("其他配置", Color.yellow);
            equipInfo.suitTag = EditorGUILayout.IntField("套装标记(0表示非套装)", equipInfo.suitTag);
            equipInfo.firePoint =  (Transform) EditorGUILayout.ObjectField("武器枪口", equipInfo.firePoint, typeof(Transform), true);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

namespace FrameWork.Editor
{
    public class AnimationInfoWnd : EditorWindow
    {
        private class SpriteAni
        {
            public GameObject go;
            public Dictionary<string, float> spriteAniLens = new Dictionary<string, float>();

            public void Init(GameObject go)
            {
                this.go = go;
                spriteAniLens.Clear();
                Animation anim = null==go?null: go.GetComponent<Animation>();
                if (null != anim)
                {
                    foreach (AnimationState animState in anim)
                    {
                        string name = animState.name;
                        AnimationClip clip = animState.clip;
                        float length = clip.length;
                        if(!spriteAniLens.ContainsKey(name))
                            spriteAniLens.Add(name,length);
                    }
                }
            }

            private string GetTitle()
            {
                if (null == go)
                    return string.Format("没有选中任何精灵，请在Project中选取");
                return string.Format("{0}[动作数：{1}]", go.name, spriteAniLens.Count);
            }

            public void OnGui()
            {
                GUILayout.BeginVertical();
                GUI.color = Color.green;
                GUILayout.Box(GetTitle(), GUILayout.ExpandWidth(true));
                GUI.color = Color.yellow;
                //EditorGUI.indentLevel++;
                ShowRow("动作名","时长（秒）");
                GUI.color = Color.white;
                foreach (var kvp in spriteAniLens)
                {
                    ShowRow(kvp.Key,string.Format("{0:F3}",kvp.Value));
                }
                //EditorGUI.indentLevel--;
                GUILayout.EndVertical();
            }

            private void ShowRow(string col1, string col2)
            {
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(col1,GUILayout.Width(200f));
                EditorGUILayout.LabelField(col2,GUILayout.Width(200f));
                GUILayout.EndHorizontal();
                GUILayout.Box("", GUILayout.Height(2), GUILayout.ExpandWidth(true));
            }
        }

        private SpriteAni ani;
        private Vector2 scrollPosition;
        [MenuItem("FrameWork/其他/显示动画时长",false,100001)]
        static void ShowAniInfo()
        {
            var window = GetWindow<AnimationInfoWnd>("动画时长",true,typeof(SceneView));            
            window.Show();
        }

        private void OnDestroy()
        {
            ani = null;
        }

        private void OnEnable()
        {
            if(null == ani)
                ani = new SpriteAni();
        }

        private void OnGUI()
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);
            ani.OnGui();
            GUILayout.EndScrollView();
        }

        private void OnSelectionChange()
        {
            ani.Init(Selection.activeGameObject);
            Repaint();
        }
    }
}
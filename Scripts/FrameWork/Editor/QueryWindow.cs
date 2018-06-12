using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace FrameWork.Editor
{
    public class QueryWindow : EditorWindow
    {
        private Action<string> callback;
        private string text;
        private string input;
        public static void Show(string text, Action<string> cb)
        {
            QueryWindow window = ScriptableObject.CreateInstance<QueryWindow>();
            window.position = new Rect(300, 300, 250, 150);
            window.text = text;
            window.callback = cb;
            window.ShowPopup();
        }

        void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField(text);
            input = EditorGUILayout.TextField("请输入:", input);
            GUILayout.Space(40);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("确认"))
            {
                callback(input);
                this.Close();
            }
            if (GUILayout.Button("取消"))
                this.Close();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }
    }
}

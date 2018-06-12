using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Game;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(LuaItem),true)]
public class LuaItemEditor : Editor
{
    private LuaItem _instance;
    private void OnEnable()
    {
        _instance = target as LuaItem;
        if (_instance != null && _instance.Data == null)
        {
            _instance.Data = _instance.GetComponent<LuaInjector>();
            EditorUtility.SetDirty(_instance);
        }
    }

    public override void OnInspectorGUI()
    {
        if (_instance == null)
        {
            return;
        }
        base.OnInspectorGUI();
        _instance.Data = EditorGUILayout.ObjectField("LuaItem", _instance.Data, typeof(LuaInjector), true) as LuaInjector;
        EditorUtility.SetDirty(_instance);
    }
   
}

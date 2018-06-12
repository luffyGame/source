using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FrameWork.Editor
{
    public class PrefabLightmapDataEditor : UnityEditor.Editor
    {
        // 把renderer上面的lightmap信息保存起来，以便存储到prefab上面
        [MenuItem("GameObject/Lightmap/Save %#K", false, 0)]
        static void SaveLightmapInfo()
        {
            GameObject go = Selection.activeGameObject;
            if (go == null) return;
            PrefabLightmapData data = go.GetComponent<PrefabLightmapData>();
            if (data == null)
            {
                data = go.AddComponent<PrefabLightmapData>();
            }

            data.SaveLightmap();
            EditorUtility.SetDirty(go);
        }

        // 把保存的lightmap信息恢复到renderer上面
        [MenuItem("GameObject/Lightmap/Load", false, 0)]
        static void LoadLightmapInfo()
        {
            GameObject go = Selection.activeGameObject;
            if (go == null) return;

            PrefabLightmapData data = go.GetComponent<PrefabLightmapData>();
            if (data == null) return;

            data.LoadLightmap();
            EditorUtility.SetDirty(go);
        }

        [MenuItem("GameObject/Lightmap/Clear", false, 0)]
        static void ClearLightmapInfo()
        {
            GameObject go = Selection.activeGameObject;
            if (go == null) return;

            PrefabLightmapData data = go.GetComponent<PrefabLightmapData>();
            if (data == null) return;
            data.Clear();
            EditorUtility.SetDirty(go);
        }
    }
}

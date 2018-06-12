using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace FrameWork.Editor
{
    public class UiAtlasMaker
    {
        [MenuItem("Assets/FrameWork/Ui/MakerAtlasAsset")]
        private static void MakeAtlasAsset()
        {
            UnityEngine.Object obj = Selection.activeObject;
            string path = AssetDatabase.GetAssetPath(obj);
            UnityEngine.Object[] objs = AssetDatabase.LoadAllAssetsAtPath(path);
            UiAtlasAsset atlas = ScriptableObject.CreateInstance<UiAtlasAsset>();
            foreach (UnityEngine.Object o in objs)
            {
                Debug.Log(o);
                if (o.GetType() == typeof(Sprite))
                    CacheSprite(o as Sprite,ref atlas.sprites);
            }
            int postIdx = path.LastIndexOf('.');
            if(postIdx>=0)
                path = path.Remove(postIdx);
            string atlasPath = path + ".asset";
            AssetDatabase.CreateAsset(atlas, atlasPath);
            AssetDatabase.SaveAssets();
        }

        [MenuItem("Assets/FrameWork/Ui/MakerAtlasPrefab")]
        private static void MakeAtlasfab()
        {
            UnityEngine.Object obj = Selection.activeObject;
            string path = AssetDatabase.GetAssetPath(obj);
            UnityEngine.Object[] objs = AssetDatabase.LoadAllAssetsAtPath(path);
            GameObject go = new GameObject(obj.name);
            UiAtlasComp atlas = go.AddComponent<UiAtlasComp>();
            foreach (UnityEngine.Object o in objs)
            {
                Debug.Log(o);
                if (o.GetType() == typeof(Sprite))
                    CacheSprite(o as Sprite, ref atlas.sprites);
            }
            int postIdx = path.LastIndexOf('.');
            if (postIdx >= 0)
                path = path.Remove(postIdx);
            string atlasPath = path + ".prefab";
            PrefabUtility.CreatePrefab(atlasPath, go);
            GameObject.DestroyImmediate(go);
        }

        private static void CacheSprite(Sprite sprite,ref List<Sprite> sprites)
        {
            if (null == sprite)
                return;
            if (!sprites.Contains(sprite))
                sprites.Add(sprite);
        }
    }
}
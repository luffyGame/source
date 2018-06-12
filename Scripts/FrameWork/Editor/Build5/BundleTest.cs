using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UObj = UnityEngine.Object;

namespace FrameWork.Editor
{
    public class BundleTest
    {
        [MenuItem("Assets/FrameWork/Bundle/LoadShader")]
        static void LoadShaderBundle()
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            AssetBundle bundle = AssetBundle.LoadFromFile(Application.dataPath + path.Substring("Assets".Length));
            Shader[] shaders = bundle.LoadAllAssets<Shader>();
            for (int i = 0; i < shaders.Length; ++i)
                Debug.Log(shaders[i]);
            //Shader.WarmupAllShaders();
        }
        [MenuItem("Assets/FrameWork/Bundle/CheckBundle")]
        static void CheckBundle()
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            AssetBundle bundle = AssetBundle.LoadFromFile(Application.dataPath + path.Substring("Assets".Length));
            GameObject[] gos = bundle.LoadAllAssets<GameObject>();
            for(int i=0;i<gos.Length;++i)
            {
                GameObject go = gos[i];
                List<Renderer> renders = null;
                Utils.GetComponent(go, ref renders);
                //Renderer[] renders = go.GetComponentsInChildren<Renderer>();排除在非active下的情况
                if (renders.Count == 0)
                    return;
                for (int j = 0; j < renders.Count; ++j)
                {
                    Material[] materials = renders[i].sharedMaterials;
                    if (materials == null)
                        continue;
                    for (int k = 0; k < materials.Length; ++k)
                    {
                        Material m = materials[j];
                        Debug.Log(m.shader.name);
                    }
                }
            }
            bundle.Unload(true);

        }
    }
}

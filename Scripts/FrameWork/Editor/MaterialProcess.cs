using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using FrameWork;

namespace FrameWork.Editor
{
    public class MaterialProcess
    {

        [MenuItem("Assets/FrameWork/Material/ShaderMobile")]
        static void Process()
        {
            UnityEngine.Object[] mats = GetSelectedMaterials();
            Selection.objects = new UnityEngine.Object[0];
            Dictionary<string, Shader> subs = GetMobileSubShaders();
            foreach (Material mat in mats)
            {
                if (null != mat.shader)
                {
                    if (subs.ContainsKey(mat.shader.name))
                    {
                        string path = AssetDatabase.GetAssetPath(mat);
                        mat.shader = subs[mat.shader.name];
                        AssetDatabase.ImportAsset(path);
                    }
                }
            }
        }
        [MenuItem("Assets/FrameWork/Material/ShaderPrintNoMobile")]
        static void DisplayNoMobileShader()
        {
            string rootPath = Application.dataPath;
            string filePath = rootPath.Substring(0, rootPath.Length - 6) + AssetDatabase.GetAssetPath(Selection.activeObject) + "/shader.txt";
            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);

            UnityEngine.Object[] mats = GetSelectedMaterials();
            foreach (Material mat in mats)
            {
                if (null != mat.shader)
                {
                    if (!mat.shader.name.StartsWith("Mobile"))
                        Utils.FileWrite(filePath, string.Format("{0} : {1}", mat.name, mat.shader.name));
                }
            }
        }

        static Dictionary<string, Shader> GetMobileSubShaders()
        {
            Dictionary<string, Shader> ret = new Dictionary<string, Shader>();
            #region Paritcles
            Shader shader = Shader.Find("Mobile/Particles/Additive");
            if (null != shader)
                ret.Add("Particles/Additive", shader);
            /////////////////////////////////////////////////////
            shader = Shader.Find("Mobile/Particles/Alpha Blended");
            if (null != shader)
                ret.Add("Particles/Alpha Blended", shader);
            /////////////////////////////////////////////////////
            shader = Shader.Find("Mobile/Particles/Multiply");
            if (null != shader)
                ret.Add("Particles/Multiply", shader);
            /////////////////////////////////////////////////////
            shader = Shader.Find("Mobile/Particles/VertexLit Blended");
            if (null != shader)
                ret.Add("Particles/VertexLit Blended", shader);
            #endregion
            /////////////////////////////////////////////////////
            shader = Shader.Find("Mobile/Diffuse");
            if (null != shader)
                ret.Add("Diffuse", shader);
            return ret;
        }
        [MenuItem("Assets/FrameWork/Material/ShaderToDiffuse")]
        static void ShaderToMobileDiffuse()
        {
            Shader subShader = Shader.Find("Mobile/Diffuse");
            UnityEngine.Object[] mats = GetSelectedMaterials();
            Selection.objects = new UnityEngine.Object[0];
            foreach (Material mat in mats)
            {
                if (null != mat.shader)
                {
                    string path = AssetDatabase.GetAssetPath(mat);
                    mat.shader = subShader;
                    AssetDatabase.ImportAsset(path);
                }
            }
        }

        static UnityEngine.Object[] GetSelectedMaterials()
        {
            return Selection.GetFiltered(typeof(Material), SelectionMode.DeepAssets);
        }
    }
}

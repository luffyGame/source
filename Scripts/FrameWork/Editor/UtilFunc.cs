using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UObj = UnityEngine.Object;

namespace FrameWork.Editor
{
    public class UtilFunc
    {
        public static string GetObjPath(UObj obj)
        {
            string rootPath = Application.dataPath;
            return rootPath.Substring(0, rootPath.Length - 6) + AssetDatabase.GetAssetPath(Selection.activeObject) + "/";
        }

        public static GameObject[] GetRootObjs()
        {
            return SceneManager.GetActiveScene().GetRootGameObjects();
        }

        public static GameObject GetRootObj(String name)
        {
            GameObject[] roots = GetRootObjs();
            for(int i=0;i<roots.Length;++i)
            {
                if (roots[i].name == name)
                    return roots[i];
            }
            return null;
        }
        
    }
}

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FrameWork.Editor
{
    public class UiResProcess
    {
        [MenuItem("Assets/FrameWork/Ui/SepRes")]
        static void SepUiRes()
        {
            GameObject go = Selection.activeGameObject;
            if (null == go)
                return;
            ProcessSingle(go);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log(string.Format("{0} done", go.name));
        }

        private static void ProcessSingle(GameObject go)
        {
            string path = AssetDatabase.GetAssetPath(go);
            GameObject newGo = GameObject.Instantiate<GameObject>(go);
            SepResWrapper.Attach(newGo);
            int postIdx = path.LastIndexOfAny( new char[]{'\\','/'});
            if (postIdx >= 0)
                path = path.Remove(postIdx+1);
            string atlasPath = path + go.name.ToLower() + "_dny.prefab";
            PrefabUtility.CreatePrefab(atlasPath, newGo);
            GameObject.DestroyImmediate(newGo);
        }
    }
}

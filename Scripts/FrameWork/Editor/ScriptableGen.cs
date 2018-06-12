using System.IO;
using UnityEditor;
using UnityEngine;

namespace FrameWork.Editor
{
    public class ScriptableGen
    {
        private static string assetDirPath = "Assets/ArtRes/Scriptable/";
        
        public static T Load<T>(string adbFileName, bool createDefault = true) where T : ScriptableObject
        {
            T asset = AssetDatabase.LoadAssetAtPath<T>(GetPath(adbFileName));
            if (asset == null && createDefault)
            {
                asset = Create<T>(adbFileName);
            }
            return asset;
        }
        
        // 创建一个ScriptableAsset
        public static T Create<T>(string adbFileName, T obj = null) where T : ScriptableObject
        {
            if (obj == null) obj = ScriptableObject.CreateInstance<T>();
            if (!Directory.Exists(assetDirPath)) { Directory.CreateDirectory(assetDirPath); }
            AssetDatabase.CreateAsset(obj, GetPath(adbFileName));
            return obj;
        }
        
        public static string GetPath(string adbFileName)
        {
            return assetDirPath + adbFileName + ".asset";
        }
    }
}
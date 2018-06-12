using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UObj = UnityEngine.Object;
using System.IO;
using System.Text;

namespace FrameWork.Editor
{
    public class BundleTools
    {
        private static BuildAssetBundleOptions options =
                BuildAssetBundleOptions.DisableWriteTypeTree |//在资源包不包含类型信息
                BuildAssetBundleOptions.DeterministicAssetBundle;//编译资源包使用一个哈希表储存对象ID在资源包中
        [MenuItem("Assets/FrameWork/Bundle/SetEachInsideDir")]
        static void SetEachInfoInsideDir()
        {
            UObj dir = Selection.activeObject;
            
            QueryWindow.Show("set what kind obj to one bundle",(ret)=>{
                UObj[] os = Selection.GetFiltered(typeof(UObj), SelectionMode.DeepAssets);
                foreach (UObj selected in os)
                {
                    if (selected.GetType() == typeof(DefaultAsset))
                        continue;
                    string path = AssetDatabase.GetAssetPath(selected);
                    if (!string.IsNullOrEmpty(ret) && !path.EndsWith(ret))
                        continue;
                    AssetImporter asset = AssetImporter.GetAtPath(path);
                    asset.assetBundleName = dir.name + "/" + selected.name + ".assetbundle"; //设置Bundle文件的名称    
                }
                AssetDatabase.Refresh();
                Debug.Log("Set Done");
            });
        }

        [MenuItem("Assets/FrameWork/Bundle/SetEachInsideDirToOne")]
        static void SetEachInsideDirToOne()
        {
            UObj dir = Selection.activeObject;
            QueryWindow.Show("set what kind obj to one bundle", (ret) => {
                UObj[] os = Selection.GetFiltered(typeof(UObj), SelectionMode.DeepAssets);
                foreach (UObj selected in os)
                {
                    if (selected.GetType() == typeof(DefaultAsset))
                        continue;
                    string path = AssetDatabase.GetAssetPath(selected);
                    Debug.Log(path);
                    AssetImporter asset = AssetImporter.GetAtPath(path);
                    if (path.EndsWith(ret))
                    {
                        asset.assetBundleName = dir.name + ".assetbundle"; //设置Bundle文件的名称    
                    }
                    else
                        asset.assetBundleName = null;

                }
                AssetDatabase.Refresh();
                Debug.Log("Set Done");
            });
        }

        [MenuItem("Assets/FrameWork/Bundle/ClearInsideDir")]
        static void ClearBundleInfo()
        {
            UObj[] os = Selection.GetFiltered(typeof(UObj), SelectionMode.DeepAssets);
            foreach (UObj selected in os)
            {
                string path = AssetDatabase.GetAssetPath(selected);
                AssetImporter asset = AssetImporter.GetAtPath(path);
                if (!string.IsNullOrEmpty(asset.assetBundleName))
                {
                    AssetDatabase.RemoveAssetBundleName(asset.assetBundleName, true);
                }
            }
            AssetDatabase.Refresh();
            Debug.Log("Clear Done");
        }
        [MenuItem("FrameWork/Bundle/Build", false, 1)]
        static void BuildBundle()
        {
            BuildAssetBundleOptions opt = BuildAssetBundleOptions.None;
            opt |= BuildAssetBundleOptions.ChunkBasedCompression;//用Lz4的方式进行压缩
            Util.CreateDir(BundlePacker.exportRootPath, false);
            AssetBundleManifest mani = BuildPipeline.BuildAssetBundles(BundlePacker.exportRootPath,
                opt,EditorUserBuildSettings.activeBuildTarget);
            Debug.Log("Bundle Created");
            GenBundleInfo(mani);
            Debug.Log("Info gen");
            AssetDatabase.Refresh();
        }
        static void GenBundleInfo(AssetBundleManifest mani)
        {
            string[] bundles = mani.GetAllAssetBundles();
            StreamWriter sw = File.CreateText(BundlePacker.exportRootPath+"all.info");
            foreach (string bundle in bundles)
            {
                sw.WriteLine(GenOneBundleInfo(mani,bundle));
            }
            sw.Close();
        }
        static void SetBundleName(string bundleName,string ext)
        {
            UObj dir = Selection.activeObject;
            UObj[] os = Selection.GetFiltered(typeof(UObj), SelectionMode.DeepAssets);
            foreach (UObj selected in os)
            {
                if (selected.GetType() == typeof(DefaultAsset))
                    continue;
                string path = AssetDatabase.GetAssetPath(selected);
                if (!string.IsNullOrEmpty(ext) && !path.EndsWith(ext))
                    continue;
                string name = bundleName;
                if (string.IsNullOrEmpty(name))
                {
                    name = selected.name.Replace('/', '^');
                }
                AssetImporter asset = AssetImporter.GetAtPath(path);
                asset.assetBundleName = dir.name + "/" + name + ".assetbundle"; //设置Bundle文件的名称    
            }
            AssetDatabase.Refresh();
        }
        static string GenOneBundleInfo(AssetBundleManifest mani,string bundle)
        {
            StringBuilder sb = new StringBuilder();
            string name = bundle;
            string path;
            int index = name.IndexOfAny(new char[]{'/','\\'});
            if (index >= 0)
            {
                name = name.Substring(index + 1, name.Length - index-1 - ".assetbundle".Length);
                path = bundle.Substring(0, index);
            }
            else
                path = name.Substring(0, name.Length - ".assetbundle".Length);
            name = name.Replace('^', '/');
            AssetType type = AssetType.GAMEOBJ;
            switch(path)
            {
                case "font": type = AssetType.FONT;
                    break;
                case "atlas": type = AssetType.MULTI_ASSETS;
                    break;
                case "sound": type = AssetType.SOUND; break;
                case "shaders": type = AssetType.MULTI_ASSETS; break;
                case "scriptable": type = AssetType.SCRIPTABLE;
                        break;
            }
            sb.AppendFormat("{0}:{1},{2},{3}:", bundle, name, (int)type, 1);
            string[] dependBundles = mani.GetAllDependencies(bundle);
            for (int i = 0; i < dependBundles.Length; ++i)
            {
                if (i == 0)
                    sb.Append(dependBundles[i]);
                else
                    sb.AppendFormat(",{0}", dependBundles[i]);
            }
            return sb.ToString();
        }
    }
}

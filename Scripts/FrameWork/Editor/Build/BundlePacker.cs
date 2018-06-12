using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using UObj = UnityEngine.Object;

namespace FrameWork.Editor
{
    //程序优先分离出来的依赖信息
    public class SepDependInfo
    {
        public string name;
        public List<string> depends = new List<string>();
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}:", name);
            for (int i = 0; i < depends.Count; ++i)
            {
                if (i == 0)
                    sb.Append(depends[i]);
                else
                    sb.AppendFormat(",{0}", depends[i]);
            }
            return sb.ToString();
        }
        public static SepDependInfo ParseString(string infoStr)
        {
            if (string.IsNullOrEmpty(infoStr))
                return null;
            string[] parts = infoStr.Split(new char[] { ':' });
            if (parts.Length < 2)
                return null;
            SepDependInfo info = new SepDependInfo();
            info.name = parts[0];
            if (!string.IsNullOrEmpty(parts[1]))
            {
                string[] deps = parts[1].Split(new char[] { ',' });
                foreach (string dep in deps)
                    info.depends.Add(dep);
            }
            return info;
        }
        public static Dictionary<string, SepDependInfo> ParseFile(string filePath)
        {
            Dictionary<string, SepDependInfo> ret = new Dictionary<string, SepDependInfo>();
            try
            {
                using (var stream = File.OpenRead(filePath))
                {
                    using (StreamReader sr = new StreamReader(stream, Encoding.UTF8))
                    {
                        while (sr.Peek() > -1)
                        {
                            string line = sr.ReadLine();
                            if (line.Length == 0)
                                continue;
                            SepDependInfo info = SepDependInfo.ParseString(line);
                            if (null != info)
                                ret.Add(info.name, info);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                Debug.Log(ex.Message);
            }
            return ret;
        }
        public static void SaveFile(string file, Dictionary<string, SepDependInfo> infos)
        {
            StreamWriter sw = File.CreateText(file);
            foreach (KeyValuePair<string, SepDependInfo> element in infos)
            {
                sw.WriteLine(element.Value.ToString());
            }
            sw.Close();
        }
    }

    //根据打包获取的依赖信息
    public class BundleInfo
    {
        public string bundlePath;
        public UnityEngine.Object asset;
        public AssetType type;
        public IList<string> dependBundles = new List<string>();
        public IList<string> dependOthers = new List<string>();
        public long bundleSize;//一个相对的值
        public void CompleteInfo()
        {
            string path = BundlePacker.exportRootPath+bundlePath;
            FileInfo bundelFile = new FileInfo(path);
            bundleSize = bundelFile.Length/1024;
            if (bundleSize <= 0)
                bundleSize = 1;
        }
        public void ExportDepends(Dictionary<UObj, BundleInfo> infos)
        {
            UObj[] d = EditorUtility.CollectDependencies(new UObj[] { asset });
            foreach (UObj o in d)
            {
                if (o == null)
                {
                    Debug.Log(string.Format("{0}:path{1} get a null depend object", asset.name, bundlePath));
                    continue;
                }
                if (null != infos)
                {
                    if (infos.ContainsKey(o) && !dependBundles.Contains(infos[o].bundlePath))
                        dependBundles.Add(infos[o].bundlePath);
                    else
                    {
                        string other = string.Format("{0}({1})", o.name, o.GetType().ToString());
                        if (!dependOthers.Contains(other))
                            dependOthers.Add(other);
                    }
                }
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}:{1},{2},{3}:", bundlePath, asset.name, (int)type, bundleSize);
            for (int i = 0; i < dependBundles.Count; ++i)
            {
                if (i == 0)
                    sb.Append(dependBundles[i]);
                else
                    sb.AppendFormat(",{0}", dependBundles[i]);
            }
            return sb.ToString();
        }

        public string GetExcludeDepend()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}:", bundlePath);
            for (int i = 0; i < dependOthers.Count; ++i)
            {
                if (i == 0)
                    sb.Append(dependOthers[i]);
                else
                    sb.AppendFormat(",{0}", dependOthers[i]);
            }
            return sb.ToString();
        }

        public void MergeSepDepend(SepDependInfo info)
        {
            foreach (string dep in info.depends)
            {
                if (!dependBundles.Contains(dep))
                    dependBundles.Add(dep);
            }
        }
    }
    class BundlePacker
    {
        private static BuildAssetBundleOptions options =
                BuildAssetBundleOptions.DisableWriteTypeTree |//在资源包不包含类型信息
                BuildAssetBundleOptions.DeterministicAssetBundle;//编译资源包使用一个哈希表储存对象ID在资源包中
        //UncompressedAssetBundle//不压缩Assetbundle，默认会进行压缩
        public static readonly string exportRootPath = Application.streamingAssetsPath + "/ArtRes/";

        //程序会手动收集依赖信息
        public static void BuildPath(string assetPath,string[] pattern, AssetType assetType,string relPath,
            ref Dictionary<UnityEngine.Object, BundleInfo> infos, Dictionary<string, SepDependInfo> sepInfos = null)
        {
            List<AssetBundleBuild> builds = new List<AssetBundleBuild>();
            GenBuilds(builds, assetPath, pattern, assetType, relPath, ref infos, sepInfos);
            Build(relPath, builds);
        }

        public static void GenBuilds(List<AssetBundleBuild> builds,
            string assetPath, string[] pattern, AssetType assetType, string relPath,
            ref Dictionary<UnityEngine.Object, BundleInfo> infos, Dictionary<string, SepDependInfo> sepInfos = null)
        {
            if (null == infos) infos = new Dictionary<UObj, BundleInfo>();
            List<FileInfo> files = Util.GetFiles(Application.dataPath + assetPath, pattern);
            if (files.Count == 0) return;
            for (int i = 0; i < files.Count; i++)
            {
                builds.Add(GenBundle(infos, files[i], relPath, assetType, sepInfos));
            }
        }

        public static void Build(string relPath,List<AssetBundleBuild> builds)
        {
            BuildPipeline.BuildAssetBundles(exportRootPath + "/" + relPath, builds.ToArray(),
                options, EditorUserBuildSettings.activeBuildTarget);
        }

        //添加要打包的bundle，infos和sepInfos用于手动收集依赖信息
        private static AssetBundleBuild GenBundle(Dictionary<UObj, BundleInfo> infos, FileInfo f,
            string relPath,AssetType assetType,Dictionary<string, SepDependInfo> sepInfos = null)
        {
            string assetPath = f.FullName.Substring(f.FullName.IndexOf("Assets")).Replace('\\','/');
            UObj asset = AssetDatabase.LoadMainAssetAtPath(assetPath);
            string bundlefileName = f.Name.Substring(0, f.Name.LastIndexOf(".")) + ".assetbundle";
            BundleInfo bi = new BundleInfo();
            bi.asset = asset;
            bi.bundlePath = relPath + bundlefileName;
            bi.type = assetType;
            //收集依赖信息
            bi.ExportDepends(infos);
            if (null != sepInfos && sepInfos.ContainsKey(asset.name))
                bi.MergeSepDepend(sepInfos[asset.name]);
            infos.Add(asset, bi);
            AssetBundleBuild build = new AssetBundleBuild();
            build.assetBundleName = bundlefileName;
            build.assetNames = new string[] { assetPath };
            return build;
        }

        public static void GenBundleInfoFile(Dictionary<UObj, BundleInfo> infos, string file)
        {
            StreamWriter sw = File.CreateText(file);
            foreach (KeyValuePair<UnityEngine.Object, BundleInfo> element in infos)
            {
                element.Value.CompleteInfo();
                sw.WriteLine(element.Value.ToString());
            }
            sw.Close();
            sw = File.CreateText(file + ".exclude");
            foreach (KeyValuePair<UnityEngine.Object, BundleInfo> element in infos)
            {
                sw.WriteLine(element.Value.GetExcludeDepend());
            }
            sw.Close();
        }
        public static string GetBundleInfoDir()
        {
            string dirpath = Application.dataPath + "/BundleInfo";
            Util.CreateDir(dirpath, false);
            return dirpath;
        }

        [MenuItem("Assets/FrameWork/资源名小写")]
        static void FolderFileLower()
        {
            string selPath = Util.GetObjectAbsolutPath(Selection.objects[0]);
            List<FileInfo> files = Util.GetFiles(selPath);
            foreach(FileInfo f in files)
            {
                string oldFileName = f.Name.Substring(0, f.Name.Length - f.Extension.Length);
                string newFileName = oldFileName.ToLower();
                if(oldFileName != newFileName)
                {
                    string newName = f.DirectoryName + "/" + newFileName + f.Extension;
                    Debug.Log(newName);
                    f.MoveTo(newName);
                }
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Text;
using FrameWork.BaseUtil;

namespace FrameWork
{
    public enum AssetType : int
    {
        GAMEOBJ = 0,
        TEXTURE = 1,
        MATERIAL = 2,
        SOUND = 3,
        FONT = 4,
        MANIFEST = 5,
        SCRIPTABLE = 6,
        SHADER = 7,
        BASETYPE_MAX = 7,//原始类型
        MULTI_ASSETS = 8,
    }
	public class BundleInfo
	{
		public string path;
		public string mainName;
        public AssetType type;//是否不需要instante
		public string[] depends;
        public long bundleSize;
        private static readonly Type[] types = {
            typeof(UnityEngine.GameObject),
            typeof(UnityEngine.Texture),
            typeof(UnityEngine.Material),
            typeof(UnityEngine.AudioClip),
            typeof(UnityEngine.Font),
            typeof(UnityEngine.AssetBundleManifest),
            typeof(UnityEngine.ScriptableObject),
            typeof(UnityEngine.Shader),
        };
        private static readonly Type[] componentTypes = {
        };
        public static BundleInfo ParseString(string infoStr)
        {
            if (string.IsNullOrEmpty(infoStr))
                return null;
            string[] parts = infoStr.Split(new char[] { ':' });
            if (parts.Length < 3)
                return null;
            BundleInfo bi = new BundleInfo();
            bi.path = parts[0];
            string[] mains = parts[1].Split(new char[] { ',' });
            bi.mainName = mains[0];
            int typeVal = 0;
            int.TryParse(mains[1], out typeVal);
            bi.type = (AssetType)typeVal;
            if (mains.Length > 2)
                long.TryParse(mains[2], out bi.bundleSize);
            else
                bi.bundleSize = 0;
            if (string.IsNullOrEmpty(parts[2]))
                bi.depends = null;
            else
                bi.depends = parts[2].Split(new char[] { ',' });

            return bi;
        }
        public Type GetAssetType()
        {
            if (type > AssetType.BASETYPE_MAX)
                return types[0];
            return types[(int)type];
        }

	    public bool BundleShouldUnload
	    {
	        get { return type != AssetType.FONT; }
	    }
	}
	public class BundleDepMap 
	{
		private Dictionary<string, BundleInfo> depMap = new Dictionary<string, BundleInfo>();
        
		#region 采用Manifest的加载方式
        private Dictionary<string, AssetBundleManifest> manifests = null;
        private int loadedManifestCount = 0;
		public void InitManifest()//依赖信息文件
		{
            manifests = new Dictionary<string, AssetBundleManifest>();
            for (int i = 0; i < BundleCfg.manifests.Length; ++i)
            {
                string path = BundleCfg.manifests[i];
                BundleMgr.Instance.GetManifest(path, (asset, cbId) =>
                    {
                        manifests[path] = asset as AssetBundleManifest;
                        ++loadedManifestCount;
                        if (loadedManifestCount == BundleCfg.manifests.Length)
                            ;
                    });
            }
		}
		
		#endregion
        #region 采用手动收集文件的加载
        private void ParseBundleInfo(string infoFileName)
        {
            string infoFile = FileUtility.GetFileReadFullPath(string.Format("{0}{1}", BundleCfg.relativePath, infoFileName));
            try
            {
                using (var stream = FileUtility.OpenFile(infoFile))
                {
                    if (null != stream)
                    {
                        using (StreamReader sr = new StreamReader(stream, Encoding.UTF8))
                        {
                            while (sr.Peek() > -1)
                            {
                                string line = sr.ReadLine();
                                if (line.Length == 0)
                                    continue;
                                BundleInfo bi = BundleInfo.ParseString(line);
                                if (null != bi)
                                    depMap[bi.path] = bi;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debugger.LogException(ex);
            }
        }
        public void Init(string depFile)//依赖信息文件
        {
            depMap.Clear();
            ParseBundleInfo(depFile);
        }
        public void Init(List<string> deps) //外部导入
        {
            depMap.Clear();
            for (int i = 0; i < deps.Count; ++i)
            {
                BundleInfo bi = BundleInfo.ParseString(deps[i]);
                if (null != bi)
                    depMap[bi.path] = bi;
            }
        }
        #endregion
        public BundleInfo GetInfo(string bundlePath, bool isManifest)
        {
            if (depMap.ContainsKey(bundlePath))
                return depMap[bundlePath];
            else if (isManifest)
            {
                BundleInfo bi = new BundleInfo();
                bi.depends = new string[] { };
                bi.mainName = "AssetBundleManifest";
                bi.type = AssetType.MANIFEST;
                depMap[bundlePath] = bi;
                return bi;
            }
            else
            {
                Debugger.LogWarning("{0} not find", bundlePath);
                return null;
            }
        }
    }
}


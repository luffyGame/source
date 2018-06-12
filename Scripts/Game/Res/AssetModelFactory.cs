using System;
using System.Collections.Generic;
using FrameWork;

namespace Game
{
    #region Define
    public enum AssetType
    {
        NONE = -1,
        MODEL_AVATAR = 0,
        MODEL_SCENEITEM = 1,
        MODEL_DUMMY = 2,
        MODEL_EQUIP = 3,
        MODEL_EFFECT = 4,
        UI_PANEL = 5,
        MODEL_VEHICLE = 6,
        HOME_BUILD = 7,
        MODEL_EXHIBIT = 8,
        TYPE_COUNT = 9,
    }
    public static class AssetModelFactory
    {
        private static readonly string[] assetRelativePath = { "character/", "sceneitem/", "dummy/", "equip/", "effect/", "panel/", "vehicle/","homebuild/","exhibition/",};
        private static readonly Dictionary<string, string>[] assetPathCache = new Dictionary<string, string>[(int)AssetType.TYPE_COUNT];
        public static string GetAssetPath(AssetType atype, string res)
        {
            if (string.IsNullOrEmpty(res) || atype == AssetType.NONE)
                return res;
            int itype = (int)atype;
            if (assetPathCache[itype] == null)
                assetPathCache[itype] = new Dictionary<string, string>();
            Dictionary<string, string> cache = assetPathCache[itype];
            if (cache.ContainsKey(res))
                return cache[res];
            else
            {
                string path = string.Format("{0}{1}.assetbundle", assetRelativePath[itype], res);
                cache.Add(res, path);
                return path;
            }
        }
        #endregion
        public static AssetModel CreateModel(AssetType atype = AssetType.NONE, string res = null)
        {
            return new AssetModel(GetAssetPath(atype, res));
        }
    }
}

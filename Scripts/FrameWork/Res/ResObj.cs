using System;
using System.Collections.Generic;

namespace FrameWork
{
    public enum EObjType
    {
        SOUND,
        SHADERS,
        ATLAS,
        FONT,
        SCRIPTABLE,
    }
    public static class ResObjUtil
    {
        private static readonly string[] OBJ_R_PATHS = 
        {
            "sound/",
            "",
            "atlas/",
            "font/",
            "scriptable",
        };
        public static string GetObjPath(EObjType otype, string path)
        {
            return string.Format("{0}{1}.assetbundle", OBJ_R_PATHS[(int)otype], path);
        }
    }
}

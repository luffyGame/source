using System;
using System.Collections.Generic;
using CSObjectWrapEditor;
using UnityEngine;
using UnityEngine.Events;
using XLua;

namespace Game
{
    public static class GenConfig
    {
        [GenPath]
        public static readonly string GenPath = Application.dataPath + "/Scripts/Game/XLuaExt/Gen/";
        [GenCodeMenu]
        public static void OnGenCode()
        {
            
        }
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using XLua;

namespace Game
{
    public static class UnityEngineGenList
    {
        [LuaCallCSharp]
        public static List<Type> LuaCallCSharp = new List<Type>()
        {
            typeof(UnityEngine.Object),
            typeof(GameObject),
            typeof(Transform),
            typeof(RectTransform),
            typeof(Component),
            //typeof(Button),
            //typeof(Button.ButtonClickedEvent),
            //typeof(Text),
            //typeof(Image),
            //typeof(Slider),
            //typeof(InputField),
            typeof(Time),
        };

        [CSharpCallLua]
        public static List<Type> CSharpCallLua = new List<Type>()
        {
            typeof(UnityAction),
            typeof(UnityAction<bool>),
            typeof(UnityAction<float>),
            typeof(UnityAction<string>),
        };

        [BlackList]
        public static List<List<string>> BlackList = new List<List<string>>()
        {
            new List<string>() {"UnityEngine.GameObject", "networkView" },
            new List<string>() {"UnityEngine.MonoBehaviour", "runInEditMode" },
            new List<string>() {"UnityEngine.UI.Text", "OnRebuildRequested" },
        };
    }
}
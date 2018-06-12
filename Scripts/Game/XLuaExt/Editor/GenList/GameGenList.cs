using System;
using System.Collections.Generic;
using FrameWork;
using UnityEngine;
using XLua;

namespace Game
{
    public static class GameGenList
    {
        [LuaCallCSharp]
        public static List<Type> LuaCallCSharp = new List<Type>()
        {
            typeof(DlgView),
            typeof(LuaInjector),
            typeof(LuaUtility),
            typeof(AssetModel),
            typeof(ObjBase),
            typeof(Sprite),
            typeof(SceneItem),
            typeof(FurnitureItem),
            typeof(BasicModel),
            typeof(Effect),
            typeof(Equip),
            typeof(JoyStick),
            typeof(BarManager),
            typeof(EventHandler),
            typeof(LuaItem),
            typeof(SimpleListViewItem<LuaInjector>),
            typeof(LoopList),
            typeof(LongPressEventTrigger),
        };

        [CSharpCallLua]
        public static List<Type> CSharpCallLua = new List<Type>()
        {
            typeof(Action),
            
            typeof(Action<int>),
            typeof(Action<int,int>),
            
            typeof(Action<float>),
            typeof(Action<float,float>),
            typeof(Action<float,float,float>),
            
            typeof(Action<string>),
            typeof(Action<string,string>),
            typeof(Action<string,bool>),
            
            typeof(Action<bool>),
            typeof(Action<bool,string>),
            
            typeof(Action<AssetModel>),
            typeof(Action<DlgView>),
            typeof(Action<ObjBase>),
            typeof(Action<BasicModel>),
            typeof(Action<Sprite>),
            typeof(Action<SceneItem>),
            typeof(Action<FurnitureItem>),
            typeof(Func<FurnitureItem,bool>),
            typeof(Func<FurnitureItem,int,int,bool>),
            
            typeof(Action<Effect>),
            typeof(Action<Equip>),
            typeof(SceneLoader.OnLevelLoad),
            typeof(Action<LuaItem>),
            
            typeof(Action<int,LuaItem>),
            
            typeof(Func<int, float, float, float, bool>),
        };

        [BlackList]
        public static List<List<string>> BlackList = new List<List<string>>()
        {

        };
    }
}
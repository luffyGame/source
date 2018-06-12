using System;
using System.Collections.Generic;
using FrameWork;
using UnityEngine;
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLua.LuaDLL.lua_CSFunction;

namespace XLua
{
    public partial class ObjectTranslator
    {
        //Component push
        class AsComponentPushWraper
        {
            static AsComponentPushWraper()
            {
                LuaEnv.AddIniter(Init);
            }
            
            static void Init(LuaEnv luaenv, ObjectTranslator translator)
            {
                translator.RegisterPush(translator.PushComponent);
            }
        }
        static AsComponentPushWraper s_supewraper_dump = new AsComponentPushWraper();
        private static Type pushType = typeof(Component);
        public void RegisterPush(Action<RealStatePtr, Component> push)
        {
            Action<RealStatePtr, Component> org_push;
            if (tryGetPushFuncByType(pushType, out org_push))
            {
                throw new InvalidOperationException("push or get of " + pushType + " has register!");
            }
            push_func_with_type.Add(pushType, push);
        }

        public void PushComponent(RealStatePtr L, Component o)
        {
            int typeid = GetTypeId(L, pushType);
            PushObject(L,o,typeid);
        }
    }
}
//#endif
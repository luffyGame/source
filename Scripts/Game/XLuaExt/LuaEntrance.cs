using System;
using System.Collections.Generic;
using System.IO;
using FrameWork;
using FrameWork.BaseUtil;
using UnityEngine;
using XLua;

namespace Game
{
    public class LuaEntrance : Singleton<LuaEntrance>
    {
        #region Var
        private static readonly string LUA_ROOT = System.IO.Path.Combine (Application.dataPath, "LuaRoot");
        private LuaEnv luaEnv;
        private Action luaStart, luaExit;
        private Action<float,float> luaUpdate;
        
        private LuaZip luaZip = new LuaZip();
        #endregion

        public void Init()
        {
            luaEnv = new LuaEnv();
            luaEnv.AddBuildin("rapidjson", XLua.LuaDLL.Lua.LoadRapidJson);
            luaEnv.AddBuildin("lpeg", XLua.LuaDLL.Lua.LoadLpeg);
            luaEnv.AddBuildin("protobuf.c", XLua.LuaDLL.Lua.LoadProtobufC);
#if UNITY_EDITOR
            luaEnv.AddLoader(LoadInLuaDir);
            luaEnv.DoString("print('Load in Dir')");
#else
            luaZip.LoadMain();
            luaEnv.AddLoader(luaZip.Load);
            luaEnv.DoString("print('Load in zip')");
#endif
            luaEnv.AddLoader(LoadLuaData);
        }

        public void Relsease()
        {
            if (null != luaEnv)
            {
                if (null != luaExit)
                {
                    luaExit();
                }

                luaStart = null;
                luaExit = null;
                luaUpdate = null;
                luaEnv.Dispose();
                luaEnv = null;
                luaZip.CloseZip();
            }
        }

        public void Update()
        {
            if (null != luaUpdate)
                luaUpdate(Time.deltaTime,Time.unscaledDeltaTime);
            if(null!=luaEnv)
                luaEnv.Tick();
        }

        public void Start()
        {
            if (null != luaEnv)
            {
                Debugger.Log("lua start");
                luaEnv.DoString("require ('app/main')");
                luaStart = luaEnv.Global.Get<Action>("Start");
                luaExit = luaEnv.Global.Get<Action>("Exit");
                luaUpdate = luaEnv.Global.Get<Action<float,float>>("Update");
                if (null != luaStart)
                    luaStart();
            }
        }

        private byte[] LoadInLuaDir(ref string filePath)
        {
            string path = string.Format("{0}/{1}.lua",LUA_ROOT,filePath.Replace('.','/'));
            if (File.Exists(path))
            {
                return File.ReadAllBytes(path);
            }

            return null;
        }

        private byte[] LoadLuaData(ref string filePath)
        {
            try
            {
                string dataFile = FileUtility.GetFileReadFullPath(filePath);
                using (Stream stream = FileUtility.OpenFile(dataFile))
                {
                    return stream.ReadAllBytes();
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
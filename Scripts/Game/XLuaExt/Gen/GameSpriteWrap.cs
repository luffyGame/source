#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
using RealStatePtr = UniLua.ILuaState;
using LuaCSFunction = UniLua.CSharpFunctionDelegate;
#else
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLua.LuaDLL.lua_CSFunction;
#endif

using XLua;
using System.Collections.Generic;


namespace XLua.CSObjectWrap
{
    using Utils = XLua.Utils;
    public class GameSpriteWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Game.Sprite);
			Utils.BeginObjectRegister(type, L, translator, 0, 27, 2, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Load", _m_Load);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Release", _m_Release);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "HasAni", _m_HasAni);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PlayAni", _m_PlayAni);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SimpleMove", _m_SimpleMove);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "BindFollowCallback", _m_BindFollowCallback);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "FollowInRandomDirection", _m_FollowInRandomDirection);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetSpeed", _m_SetSpeed);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "StartFollow", _m_StartFollow);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "StartFollowOffset", _m_StartFollowOffset);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CancelFollow", _m_CancelFollow);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "WearOn", _m_WearOn);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "WearOff", _m_WearOff);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "WearAllRemove", _m_WearAllRemove);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Dismember", _m_Dismember);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Ragdoll", _m_Ragdoll);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RecoverFromDeathShow", _m_RecoverFromDeathShow);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "BindUseChecker", _m_BindUseChecker);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetUsable", _m_SetUsable);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetMount", _m_GetMount);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CanSkillReach", _m_CanSkillReach);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetSkillReach", _m_GetSkillReach);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PlayTimedEffectAtPoint", _m_PlayTimedEffectAtPoint);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PlayTimedEffectAtMount", _m_PlayTimedEffectAtMount);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PlayWeaponEffect", _m_PlayWeaponEffect);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetWeaponFireMount", _m_GetWeaponFireMount);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Alive", _m_Alive);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "objType", _g_get_objType);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "info", _g_get_info);
            
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 1, 0, 0);
			
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 2 && (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
				{
					string bodyRes = LuaAPI.lua_tostring(L, 2);
					
					Game.Sprite __cl_gen_ret = new Game.Sprite(bodyRes);
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Game.Sprite constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Load(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Game.Sprite __cl_gen_to_be_invoked = (Game.Sprite)translator.FastGetCSObj(L, 1);
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 2&& translator.Assignable<System.Action>(L, 2)) 
                {
                    System.Action done = translator.GetDelegate<System.Action>(L, 2);
                    
                    __cl_gen_to_be_invoked.Load( done );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 1) 
                {
                    
                    __cl_gen_to_be_invoked.Load(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Game.Sprite.Load!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Release(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Game.Sprite __cl_gen_to_be_invoked = (Game.Sprite)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.Release(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_HasAni(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Game.Sprite __cl_gen_to_be_invoked = (Game.Sprite)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string act = LuaAPI.lua_tostring(L, 2);
                    
                        bool __cl_gen_ret = __cl_gen_to_be_invoked.HasAni( act );
                        LuaAPI.lua_pushboolean(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PlayAni(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Game.Sprite __cl_gen_to_be_invoked = (Game.Sprite)translator.FastGetCSObj(L, 1);
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 2&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    string act = LuaAPI.lua_tostring(L, 2);
                    
                        float __cl_gen_ret = __cl_gen_to_be_invoked.PlayAni( act );
                        LuaAPI.lua_pushnumber(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                if(__gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    string act = LuaAPI.lua_tostring(L, 2);
                    float fade = (float)LuaAPI.lua_tonumber(L, 3);
                    
                        float __cl_gen_ret = __cl_gen_to_be_invoked.PlayAni( act, fade );
                        LuaAPI.lua_pushnumber(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Game.Sprite.PlayAni!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SimpleMove(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Game.Sprite __cl_gen_to_be_invoked = (Game.Sprite)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    float x = (float)LuaAPI.lua_tonumber(L, 2);
                    float z = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    __cl_gen_to_be_invoked.SimpleMove( x, z );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_BindFollowCallback(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Game.Sprite __cl_gen_to_be_invoked = (Game.Sprite)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Action<float, float, float> notifyPosSet = translator.GetDelegate<System.Action<float, float, float>>(L, 2);
                    System.Action<float, float, float> notifyDirSet = translator.GetDelegate<System.Action<float, float, float>>(L, 3);
                    System.Action notifyMoverComp = translator.GetDelegate<System.Action>(L, 4);
                    
                    __cl_gen_to_be_invoked.BindFollowCallback( notifyPosSet, notifyDirSet, notifyMoverComp );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_FollowInRandomDirection(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Game.Sprite __cl_gen_to_be_invoked = (Game.Sprite)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    bool use = LuaAPI.lua_toboolean(L, 2);
                    
                    __cl_gen_to_be_invoked.FollowInRandomDirection( use );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetSpeed(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Game.Sprite __cl_gen_to_be_invoked = (Game.Sprite)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    float speed = (float)LuaAPI.lua_tonumber(L, 2);
                    
                    __cl_gen_to_be_invoked.SetSpeed( speed );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_StartFollow(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Game.Sprite __cl_gen_to_be_invoked = (Game.Sprite)translator.FastGetCSObj(L, 1);
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 5&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)) 
                {
                    float x = (float)LuaAPI.lua_tonumber(L, 2);
                    float y = (float)LuaAPI.lua_tonumber(L, 3);
                    float z = (float)LuaAPI.lua_tonumber(L, 4);
                    float arriveRange = (float)LuaAPI.lua_tonumber(L, 5);
                    
                    __cl_gen_to_be_invoked.StartFollow( x, y, z, arriveRange );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 3&& translator.Assignable<Game.ObjBase>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    Game.ObjBase obj = (Game.ObjBase)translator.GetObject(L, 2, typeof(Game.ObjBase));
                    float arriveRange = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    __cl_gen_to_be_invoked.StartFollow( obj, arriveRange );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Game.Sprite.StartFollow!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_StartFollowOffset(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Game.Sprite __cl_gen_to_be_invoked = (Game.Sprite)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Game.ObjBase obj = (Game.ObjBase)translator.GetObject(L, 2, typeof(Game.ObjBase));
                    float x = (float)LuaAPI.lua_tonumber(L, 3);
                    float y = (float)LuaAPI.lua_tonumber(L, 4);
                    float z = (float)LuaAPI.lua_tonumber(L, 5);
                    
                    __cl_gen_to_be_invoked.StartFollowOffset( obj, x, y, z );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CancelFollow(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Game.Sprite __cl_gen_to_be_invoked = (Game.Sprite)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.CancelFollow(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_WearOn(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Game.Sprite __cl_gen_to_be_invoked = (Game.Sprite)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int partId = LuaAPI.xlua_tointeger(L, 2);
                    string equipRes = LuaAPI.lua_tostring(L, 3);
                    System.Action<Game.ObjBase> cb = translator.GetDelegate<System.Action<Game.ObjBase>>(L, 4);
                    
                    __cl_gen_to_be_invoked.WearOn( partId, equipRes, cb );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_WearOff(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Game.Sprite __cl_gen_to_be_invoked = (Game.Sprite)translator.FastGetCSObj(L, 1);
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 2&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    int partId = LuaAPI.xlua_tointeger(L, 2);
                    
                    __cl_gen_to_be_invoked.WearOff( partId );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    int partId1 = LuaAPI.xlua_tointeger(L, 2);
                    int partId2 = LuaAPI.xlua_tointeger(L, 3);
                    
                    __cl_gen_to_be_invoked.WearOff( partId1, partId2 );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Game.Sprite.WearOff!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_WearAllRemove(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Game.Sprite __cl_gen_to_be_invoked = (Game.Sprite)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.WearAllRemove(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Dismember(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Game.Sprite __cl_gen_to_be_invoked = (Game.Sprite)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int boomPart = LuaAPI.xlua_tointeger(L, 2);
                    int power = LuaAPI.xlua_tointeger(L, 3);
                    int otherboomNum = LuaAPI.xlua_tointeger(L, 4);
                    float x = (float)LuaAPI.lua_tonumber(L, 5);
                    float y = (float)LuaAPI.lua_tonumber(L, 6);
                    float z = (float)LuaAPI.lua_tonumber(L, 7);
                    
                    __cl_gen_to_be_invoked.Dismember( boomPart, power, otherboomNum, x, y, z );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Ragdoll(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Game.Sprite __cl_gen_to_be_invoked = (Game.Sprite)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int boomPart = LuaAPI.xlua_tointeger(L, 2);
                    int power = LuaAPI.xlua_tointeger(L, 3);
                    float x = (float)LuaAPI.lua_tonumber(L, 4);
                    float y = (float)LuaAPI.lua_tonumber(L, 5);
                    float z = (float)LuaAPI.lua_tonumber(L, 6);
                    
                    __cl_gen_to_be_invoked.Ragdoll( boomPart, power, x, y, z );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RecoverFromDeathShow(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Game.Sprite __cl_gen_to_be_invoked = (Game.Sprite)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.RecoverFromDeathShow(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_BindUseChecker(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Game.Sprite __cl_gen_to_be_invoked = (Game.Sprite)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Action<int, int> onCheck = translator.GetDelegate<System.Action<int, int>>(L, 2);
                    
                    __cl_gen_to_be_invoked.BindUseChecker( onCheck );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetUsable(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Game.Sprite __cl_gen_to_be_invoked = (Game.Sprite)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    bool bUsable = LuaAPI.lua_toboolean(L, 2);
                    
                    __cl_gen_to_be_invoked.SetUsable( bUsable );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetMount(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Game.Sprite __cl_gen_to_be_invoked = (Game.Sprite)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int mountTag = LuaAPI.xlua_tointeger(L, 2);
                    
                        UnityEngine.Transform __cl_gen_ret = __cl_gen_to_be_invoked.GetMount( mountTag );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CanSkillReach(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Game.Sprite __cl_gen_to_be_invoked = (Game.Sprite)translator.FastGetCSObj(L, 1);
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 5&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)) 
                {
                    float tox = (float)LuaAPI.lua_tonumber(L, 2);
                    float toy = (float)LuaAPI.lua_tonumber(L, 3);
                    float toz = (float)LuaAPI.lua_tonumber(L, 4);
                    float skillRange = (float)LuaAPI.lua_tonumber(L, 5);
                    float x;
                    float y;
                    float z;
                    
                        bool __cl_gen_ret = __cl_gen_to_be_invoked.CanSkillReach( tox, toy, toz, skillRange, out x, out y, out z );
                        LuaAPI.lua_pushboolean(L, __cl_gen_ret);
                    LuaAPI.lua_pushnumber(L, x);
                        
                    LuaAPI.lua_pushnumber(L, y);
                        
                    LuaAPI.lua_pushnumber(L, z);
                        
                    
                    
                    
                    return 4;
                }
                if(__gen_param_count == 3&& translator.Assignable<Game.ObjBase>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    Game.ObjBase target = (Game.ObjBase)translator.GetObject(L, 2, typeof(Game.ObjBase));
                    float skillRange = (float)LuaAPI.lua_tonumber(L, 3);
                    float x;
                    float y;
                    float z;
                    
                        bool __cl_gen_ret = __cl_gen_to_be_invoked.CanSkillReach( target, skillRange, out x, out y, out z );
                        LuaAPI.lua_pushboolean(L, __cl_gen_ret);
                    LuaAPI.lua_pushnumber(L, x);
                        
                    LuaAPI.lua_pushnumber(L, y);
                        
                    LuaAPI.lua_pushnumber(L, z);
                        
                    
                    
                    
                    return 4;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Game.Sprite.CanSkillReach!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetSkillReach(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Game.Sprite __cl_gen_to_be_invoked = (Game.Sprite)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    float dirx = (float)LuaAPI.lua_tonumber(L, 2);
                    float diry = (float)LuaAPI.lua_tonumber(L, 3);
                    float dirz = (float)LuaAPI.lua_tonumber(L, 4);
                    float skillRange = (float)LuaAPI.lua_tonumber(L, 5);
                    float x;
                    float y;
                    float z;
                    
                    __cl_gen_to_be_invoked.GetSkillReach( dirx, diry, dirz, skillRange, out x, out y, out z );
                    LuaAPI.lua_pushnumber(L, x);
                        
                    LuaAPI.lua_pushnumber(L, y);
                        
                    LuaAPI.lua_pushnumber(L, z);
                        
                    
                    
                    
                    return 3;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PlayTimedEffectAtPoint(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Game.Sprite __cl_gen_to_be_invoked = (Game.Sprite)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string effectRes = LuaAPI.lua_tostring(L, 2);
                    
                    __cl_gen_to_be_invoked.PlayTimedEffectAtPoint( effectRes );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PlayTimedEffectAtMount(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Game.Sprite __cl_gen_to_be_invoked = (Game.Sprite)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string effectRes = LuaAPI.lua_tostring(L, 2);
                    int mount = LuaAPI.xlua_tointeger(L, 3);
                    bool mounted = LuaAPI.lua_toboolean(L, 4);
                    
                    __cl_gen_to_be_invoked.PlayTimedEffectAtMount( effectRes, mount, mounted );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PlayWeaponEffect(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Game.Sprite __cl_gen_to_be_invoked = (Game.Sprite)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string effectRes = LuaAPI.lua_tostring(L, 2);
                    int mount = LuaAPI.xlua_tointeger(L, 3);
                    bool mounted = LuaAPI.lua_toboolean(L, 4);
                    
                    __cl_gen_to_be_invoked.PlayWeaponEffect( effectRes, mount, mounted );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetWeaponFireMount(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Game.Sprite __cl_gen_to_be_invoked = (Game.Sprite)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        UnityEngine.Transform __cl_gen_ret = __cl_gen_to_be_invoked.GetWeaponFireMount(  );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Alive(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Game.Sprite __cl_gen_to_be_invoked = (Game.Sprite)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    bool alivable = LuaAPI.lua_toboolean(L, 2);
                    
                    __cl_gen_to_be_invoked.Alive( alivable );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_objType(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Game.Sprite __cl_gen_to_be_invoked = (Game.Sprite)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.objType);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_info(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Game.Sprite __cl_gen_to_be_invoked = (Game.Sprite)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.info);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        
        
		
		
		
		
    }
}

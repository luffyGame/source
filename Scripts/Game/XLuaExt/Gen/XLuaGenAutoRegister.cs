#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
using RealStatePtr = UniLua.ILuaState;
using LuaCSFunction = UniLua.CSharpFunctionDelegate;
#else
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLua.LuaDLL.lua_CSFunction;
#endif

using System;
using System.Collections.Generic;
using System.Reflection;


namespace XLua.CSObjectWrap
{
    public class XLua_Gen_Initer_Register__
	{
	    static XLua_Gen_Initer_Register__()
        {
		    XLua.LuaEnv.AddIniter((luaenv, translator) => {
			    
				translator.DelayWrapLoader(typeof(Game.DlgView), GameDlgViewWrap.__Register);
				
				translator.DelayWrapLoader(typeof(Game.LuaInjector), GameLuaInjectorWrap.__Register);
				
				translator.DelayWrapLoader(typeof(Game.LuaUtility), GameLuaUtilityWrap.__Register);
				
				translator.DelayWrapLoader(typeof(FrameWork.AssetModel), FrameWorkAssetModelWrap.__Register);
				
				translator.DelayWrapLoader(typeof(Game.ObjBase), GameObjBaseWrap.__Register);
				
				translator.DelayWrapLoader(typeof(Game.Sprite), GameSpriteWrap.__Register);
				
				translator.DelayWrapLoader(typeof(Game.SceneItem), GameSceneItemWrap.__Register);
				
				translator.DelayWrapLoader(typeof(Game.FurnitureItem), GameFurnitureItemWrap.__Register);
				
				translator.DelayWrapLoader(typeof(Game.BasicModel), GameBasicModelWrap.__Register);
				
				translator.DelayWrapLoader(typeof(Game.Effect), GameEffectWrap.__Register);
				
				translator.DelayWrapLoader(typeof(Game.Equip), GameEquipWrap.__Register);
				
				translator.DelayWrapLoader(typeof(FrameWork.JoyStick), FrameWorkJoyStickWrap.__Register);
				
				translator.DelayWrapLoader(typeof(FrameWork.BarManager), FrameWorkBarManagerWrap.__Register);
				
				translator.DelayWrapLoader(typeof(Game.EventHandler), GameEventHandlerWrap.__Register);
				
				translator.DelayWrapLoader(typeof(Game.LuaItem), GameLuaItemWrap.__Register);
				
				translator.DelayWrapLoader(typeof(FrameWork.SimpleListViewItem<Game.LuaInjector>), FrameWorkSimpleListViewItem_1_GameLuaInjector_Wrap.__Register);
				
				translator.DelayWrapLoader(typeof(Game.LoopList), GameLoopListWrap.__Register);
				
				translator.DelayWrapLoader(typeof(Game.LongPressEventTrigger), GameLongPressEventTriggerWrap.__Register);
				
				translator.DelayWrapLoader(typeof(UnityEngine.Object), UnityEngineObjectWrap.__Register);
				
				translator.DelayWrapLoader(typeof(UnityEngine.GameObject), UnityEngineGameObjectWrap.__Register);
				
				translator.DelayWrapLoader(typeof(UnityEngine.Transform), UnityEngineTransformWrap.__Register);
				
				translator.DelayWrapLoader(typeof(UnityEngine.RectTransform), UnityEngineRectTransformWrap.__Register);
				
				translator.DelayWrapLoader(typeof(UnityEngine.Component), UnityEngineComponentWrap.__Register);
				
				translator.DelayWrapLoader(typeof(UnityEngine.Time), UnityEngineTimeWrap.__Register);
				
				
				
			});
		}
		
		
	}
	
}
namespace XLua
{
	public partial class ObjectTranslator
	{
		static XLua.CSObjectWrap.XLua_Gen_Initer_Register__ s_gen_reg_dumb_obj = new XLua.CSObjectWrap.XLua_Gen_Initer_Register__();
		static XLua.CSObjectWrap.XLua_Gen_Initer_Register__ gen_reg_dumb_obj {get{return s_gen_reg_dumb_obj;}}
	}
	
	internal partial class InternalGlobals
    {
	    
	    static InternalGlobals()
		{
		    extensionMethodMap = new Dictionary<Type, IEnumerable<MethodInfo>>()
			{
			    
			};
			
			genTryArrayGetPtr = StaticLuaCallbacks.__tryArrayGet;
            genTryArraySetPtr = StaticLuaCallbacks.__tryArraySet;
		}
	}
}

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
    public class GameLuaUtilityWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Game.LuaUtility);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 145, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "LoadView", _m_LoadView_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ButtonBindOnClick", _m_ButtonBindOnClick_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ButtonUnBindOnClick", _m_ButtonUnBindOnClick_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "UiBindLongPress", _m_UiBindLongPress_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "UiUnBindLongPress", _m_UiUnBindLongPress_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "UiBindLongPressCancel", _m_UiBindLongPressCancel_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "UiUnBindLongPressCancel", _m_UiUnBindLongPressCancel_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "UiBindOnPointerDown", _m_UiBindOnPointerDown_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "UiBindOnPointerUp", _m_UiBindOnPointerUp_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SetButtonInteractable", _m_SetButtonInteractable_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SetBtnExtendInteractable", _m_SetBtnExtendInteractable_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "TextSetTxt", _m_TextSetTxt_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "TextAddTxt", _m_TextAddTxt_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ImgSetSprite", _m_ImgSetSprite_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ImgColorGray", _m_ImgColorGray_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ImgSetColor", _m_ImgSetColor_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ImgSetAlpha", _m_ImgSetAlpha_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ImgSetAmount", _m_ImgSetAmount_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SliderSetValue", _m_SliderSetValue_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "InputFieldGetText", _m_InputFieldGetText_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SingleToggleSetValue", _m_SingleToggleSetValue_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ScrollRectSetPos", _m_ScrollRectSetPos_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "JoystickBind", _m_JoystickBind_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "BindDragEvents", _m_BindDragEvents_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "BindOnItemAdd", _m_BindOnItemAdd_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "UnBindOnItemAdd", _m_UnBindOnItemAdd_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "BindOnItemRemove", _m_BindOnItemRemove_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "UnBindOnItemRemove", _m_UnBindOnItemRemove_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DynamicRemoveItem", _m_DynamicRemoveItem_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ReleaseLuaSimpleList", _m_ReleaseLuaSimpleList_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SetComponentEnabled", _m_SetComponentEnabled_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "BindLoopListEvent", _m_BindLoopListEvent_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "BindLoopListClick", _m_BindLoopListClick_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "UnBindLoopListEvent", _m_UnBindLoopListEvent_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "UnBindLoopListClick", _m_UnBindLoopListClick_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "LuaSimpleListInit", _m_LuaSimpleListInit_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SetLuaSimpleListCount", _m_SetLuaSimpleListCount_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Tip", _m_Tip_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Warning", _m_Warning_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "PopHp", _m_PopHp_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "RemoveHpPop", _m_RemoveHpPop_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "BubbleClear", _m_BubbleClear_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Rotate", _m_Rotate_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "TransformGetPos", _m_TransformGetPos_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "TransformSetPos", _m_TransformSetPos_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "TransformGetDir", _m_TransformGetDir_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ClonePosition", _m_ClonePosition_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "CalculateTransformDist", _m_CalculateTransformDist_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "TimeConvertSecondToHHMM", _m_TimeConvertSecondToHHMM_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DoScale", _m_DoScale_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DoAnimation", _m_DoAnimation_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DoAnimationByID", _m_DoAnimationByID_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DoRectTransformMove", _m_DoRectTransformMove_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DoMove", _m_DoMove_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SetScale", _m_SetScale_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "LoadScene", _m_LoadScene_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetAllBuildScene", _m_GetAllBuildScene_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "InjectSceneSet", _m_InjectSceneSet_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "InitGlobalSetting", _m_InitGlobalSetting_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetTerrainHeight", _m_GetTerrainHeight_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetNavMeshPos", _m_GetNavMeshPos_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "LoadSprite", _m_LoadSprite_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "LoadSceneItem", _m_LoadSceneItem_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "LoadBasicModel", _m_LoadBasicModel_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "LoadFurnitureItem", _m_LoadFurnitureItem_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "LoadEffect", _m_LoadEffect_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "CommonResCache", _m_CommonResCache_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DestorySplash", _m_DestorySplash_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ComponentGameObjVisible", _m_ComponentGameObjVisible_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ComponenRenderVisible", _m_ComponenRenderVisible_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "FollowCameraFollow", _m_FollowCameraFollow_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetFollowCameraRotY", _m_GetFollowCameraRotY_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SetBuildmodelOffset", _m_SetBuildmodelOffset_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "RecoverBuildmodelOffset", _m_RecoverBuildmodelOffset_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DragCameraFocus", _m_DragCameraFocus_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DragCameraSetPos", _m_DragCameraSetPos_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Follow3DBy2DFollowTrans", _m_Follow3DBy2DFollowTrans_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Follow3DBy2DFollowTransDelta", _m_Follow3DBy2DFollowTransDelta_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DebuggerLog", _m_DebuggerLog_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "MaskInput", _m_MaskInput_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "InjectTransChilds", _m_InjectTransChilds_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DestroyGameObject", _m_DestroyGameObject_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "RadarManagerMoveRadarPos", _m_RadarManagerMoveRadarPos_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "RadarManagerAddRadar", _m_RadarManagerAddRadar_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "RadarManagerRemoveRadar", _m_RadarManagerRemoveRadar_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "RadarManagerRotatePlayerIcon", _m_RadarManagerRotatePlayerIcon_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "RadarManagerChangeIcon", _m_RadarManagerChangeIcon_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "RadarManagerDestroyRadarItems", _m_RadarManagerDestroyRadarItems_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "RadarManagerOutBorder", _m_RadarManagerOutBorder_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "RadarManagerInitBorderRot", _m_RadarManagerInitBorderRot_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "RegisterPlayerTriggers", _m_RegisterPlayerTriggers_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "RegisterMonsterTriggers", _m_RegisterMonsterTriggers_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "CloneLuaItem", _m_CloneLuaItem_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SetRendererLayer", _m_SetRendererLayer_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetLayer", _m_GetLayer_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ShowEquipDuration", _m_ShowEquipDuration_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SetStorageCb", _m_SetStorageCb_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "StartLoad", _m_StartLoad_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "StartSave", _m_StartSave_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SetSyncUrl", _m_SetSyncUrl_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SetSyncCb", _m_SetSyncCb_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "HttpSync", _m_HttpSync_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "NavTransManagerInit", _m_NavTransManagerInit_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "NavTransManagerEnableTrans", _m_NavTransManagerEnableTrans_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "NavTransManagerTrans", _m_NavTransManagerTrans_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "NavTransManagerUnBindNotify", _m_NavTransManagerUnBindNotify_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "NavTransManagerTransSetSpeed", _m_NavTransManagerTransSetSpeed_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "NavTransManagerGetLength", _m_NavTransManagerGetLength_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "NavTransManagerMoveToTargetByTime", _m_NavTransManagerMoveToTargetByTime_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "NavTransManagerCalculatePath", _m_NavTransManagerCalculatePath_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "NavTransManagerBindPosNotify", _m_NavTransManagerBindPosNotify_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "NavTransManagerBindCompleteNotify", _m_NavTransManagerBindCompleteNotify_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "LineManagerDrawLine", _m_LineManagerDrawLine_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DrawLine", _m_DrawLine_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ActiveLine", _m_ActiveLine_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "BarManagerAddItem", _m_BarManagerAddItem_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "BarManagerAddItemByOffset", _m_BarManagerAddItemByOffset_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "BarManagerRemoveItem", _m_BarManagerRemoveItem_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "HintManagerAddHintItem", _m_HintManagerAddHintItem_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "HintManagerRemoveHintItem", _m_HintManagerRemoveHintItem_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "HintManagerPositionHint", _m_HintManagerPositionHint_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "PlaceDropItem", _m_PlaceDropItem_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "RemoveDropItem", _m_RemoveDropItem_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "BindBuildingStart", _m_BindBuildingStart_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "BindBuildingSelect", _m_BindBuildingSelect_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "BindBuildingUpdate", _m_BindBuildingUpdate_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "BindPlaceBuild", _m_BindPlaceBuild_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "BindBuildingDelete", _m_BindBuildingDelete_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "LocFurnitureItem", _m_LocFurnitureItem_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "BuilderSet", _m_BuilderSet_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "EnableBuilding", _m_EnableBuilding_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "CreateFurnitureItem", _m_CreateFurnitureItem_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "UpdateFurnitureItem", _m_UpdateFurnitureItem_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "PlaceFurniture", _m_PlaceFurniture_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "CancelBuilding", _m_CancelBuilding_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DeleteSelectBuilding", _m_DeleteSelectBuilding_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "UpdateBuilding", _m_UpdateBuilding_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "RotateFurniture", _m_RotateFurniture_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SelectFurniture", _m_SelectFurniture_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "AddPlaceItem", _m_AddPlaceItem_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "RefreshSceneFurnitures", _m_RefreshSceneFurnitures_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "RecoverSceneFurnitures", _m_RecoverSceneFurnitures_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "CanDeleteSelectBuilding", _m_CanDeleteSelectBuilding_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "RecoverSelectBuilding", _m_RecoverSelectBuilding_xlua_st_);
            
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "Game.LuaUtility does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadView_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string res = LuaAPI.lua_tostring(L, 1);
                    System.Action<Game.DlgView> onLoaded = translator.GetDelegate<System.Action<Game.DlgView>>(L, 2);
                    bool top = LuaAPI.lua_toboolean(L, 3);
                    
                    Game.LuaUtility.LoadView( res, onLoaded, top );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ButtonBindOnClick_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component but = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    UnityEngine.Events.UnityAction onClick = translator.GetDelegate<UnityEngine.Events.UnityAction>(L, 2);
                    
                    Game.LuaUtility.ButtonBindOnClick( but, onClick );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ButtonUnBindOnClick_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component but = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    UnityEngine.Events.UnityAction onClick = translator.GetDelegate<UnityEngine.Events.UnityAction>(L, 2);
                    
                    Game.LuaUtility.ButtonUnBindOnClick( but, onClick );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UiBindLongPress_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component but = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    UnityEngine.Events.UnityAction longPress = translator.GetDelegate<UnityEngine.Events.UnityAction>(L, 2);
                    
                    Game.LuaUtility.UiBindLongPress( but, longPress );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UiUnBindLongPress_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component but = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    UnityEngine.Events.UnityAction longPress = translator.GetDelegate<UnityEngine.Events.UnityAction>(L, 2);
                    
                    Game.LuaUtility.UiUnBindLongPress( but, longPress );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UiBindLongPressCancel_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component but = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    UnityEngine.Events.UnityAction longPress = translator.GetDelegate<UnityEngine.Events.UnityAction>(L, 2);
                    
                    Game.LuaUtility.UiBindLongPressCancel( but, longPress );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UiUnBindLongPressCancel_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component but = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    UnityEngine.Events.UnityAction longPress = translator.GetDelegate<UnityEngine.Events.UnityAction>(L, 2);
                    
                    Game.LuaUtility.UiUnBindLongPressCancel( but, longPress );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UiBindOnPointerDown_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component uiElement = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    System.Action onPointerDown = translator.GetDelegate<System.Action>(L, 2);
                    
                    Game.LuaUtility.UiBindOnPointerDown( uiElement, onPointerDown );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UiBindOnPointerUp_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component uiElement = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    System.Action onPointerUp = translator.GetDelegate<System.Action>(L, 2);
                    
                    Game.LuaUtility.UiBindOnPointerUp( uiElement, onPointerUp );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetButtonInteractable_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component button = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    bool interactable = LuaAPI.lua_toboolean(L, 2);
                    
                    Game.LuaUtility.SetButtonInteractable( button, interactable );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetBtnExtendInteractable_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component btnExtend = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    bool interactable = LuaAPI.lua_toboolean(L, 2);
                    
                    Game.LuaUtility.SetBtnExtendInteractable( btnExtend, interactable );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_TextSetTxt_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component text = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    string txt = LuaAPI.lua_tostring(L, 2);
                    
                    Game.LuaUtility.TextSetTxt( text, txt );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_TextAddTxt_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 3&& translator.Assignable<UnityEngine.Component>(L, 1)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)) 
                {
                    UnityEngine.Component text = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    string txt = LuaAPI.lua_tostring(L, 2);
                    bool newLine = LuaAPI.lua_toboolean(L, 3);
                    
                    Game.LuaUtility.TextAddTxt( text, txt, newLine );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 2&& translator.Assignable<UnityEngine.Component>(L, 1)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    UnityEngine.Component text = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    string txt = LuaAPI.lua_tostring(L, 2);
                    
                    Game.LuaUtility.TextAddTxt( text, txt );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Game.LuaUtility.TextAddTxt!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ImgSetSprite_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component img = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    string spriteRes = LuaAPI.lua_tostring(L, 2);
                    bool nativeSize = LuaAPI.lua_toboolean(L, 3);
                    
                    Game.LuaUtility.ImgSetSprite( img, spriteRes, nativeSize );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ImgColorGray_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component img = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    bool gray = LuaAPI.lua_toboolean(L, 2);
                    
                    Game.LuaUtility.ImgColorGray( img, gray );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ImgSetColor_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component img = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    uint color = LuaAPI.xlua_touint(L, 2);
                    
                    Game.LuaUtility.ImgSetColor( img, color );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ImgSetAlpha_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component img = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    float alpha = (float)LuaAPI.lua_tonumber(L, 2);
                    
                    Game.LuaUtility.ImgSetAlpha( img, alpha );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ImgSetAmount_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component img = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    float v = (float)LuaAPI.lua_tonumber(L, 2);
                    
                    Game.LuaUtility.ImgSetAmount( img, v );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SliderSetValue_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component slider = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    float v = (float)LuaAPI.lua_tonumber(L, 2);
                    
                    Game.LuaUtility.SliderSetValue( slider, v );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InputFieldGetText_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component input = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    
                        string __cl_gen_ret = Game.LuaUtility.InputFieldGetText( input );
                        LuaAPI.lua_pushstring(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SingleToggleSetValue_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component stogle = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    bool on = LuaAPI.lua_toboolean(L, 2);
                    
                    Game.LuaUtility.SingleToggleSetValue( stogle, on );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ScrollRectSetPos_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component scrollRect = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    float pos = (float)LuaAPI.lua_tonumber(L, 2);
                    bool isVertical = LuaAPI.lua_toboolean(L, 3);
                    bool needDelay = LuaAPI.lua_toboolean(L, 4);
                    
                    Game.LuaUtility.ScrollRectSetPos( scrollRect, pos, isVertical, needDelay );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_JoystickBind_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component joyStick = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    System.Action<float, float, float> mov = translator.GetDelegate<System.Action<float, float, float>>(L, 2);
                    System.Action begin = translator.GetDelegate<System.Action>(L, 3);
                    System.Action end = translator.GetDelegate<System.Action>(L, 4);
                    
                    Game.LuaUtility.JoystickBind( joyStick, mov, begin, end );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_BindDragEvents_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component eventHandler = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    System.Action onClick = translator.GetDelegate<System.Action>(L, 2);
                    System.Action onBeginDrag = translator.GetDelegate<System.Action>(L, 3);
                    System.Action<float, float> onDrag = translator.GetDelegate<System.Action<float, float>>(L, 4);
                    System.Action<float, float> onEndDrag = translator.GetDelegate<System.Action<float, float>>(L, 5);
                    System.Action onDrop = translator.GetDelegate<System.Action>(L, 6);
                    
                    Game.LuaUtility.BindDragEvents( eventHandler, onClick, onBeginDrag, onDrag, onEndDrag, onDrop );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_BindOnItemAdd_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    Game.LuaSimpleList eventHandler = (Game.LuaSimpleList)translator.GetObject(L, 1, typeof(Game.LuaSimpleList));
                    System.Action<Game.LuaItem> onItemAdd = translator.GetDelegate<System.Action<Game.LuaItem>>(L, 2);
                    
                    Game.LuaUtility.BindOnItemAdd( eventHandler, onItemAdd );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UnBindOnItemAdd_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    Game.LuaSimpleList eventHandler = (Game.LuaSimpleList)translator.GetObject(L, 1, typeof(Game.LuaSimpleList));
                    
                    Game.LuaUtility.UnBindOnItemAdd( eventHandler );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_BindOnItemRemove_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    Game.LuaSimpleList eventHandler = (Game.LuaSimpleList)translator.GetObject(L, 1, typeof(Game.LuaSimpleList));
                    System.Action<Game.LuaItem> onItemRemove = translator.GetDelegate<System.Action<Game.LuaItem>>(L, 2);
                    
                    Game.LuaUtility.BindOnItemRemove( eventHandler, onItemRemove );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UnBindOnItemRemove_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    Game.LuaSimpleList eventHandler = (Game.LuaSimpleList)translator.GetObject(L, 1, typeof(Game.LuaSimpleList));
                    System.Action<Game.LuaItem> onItemRemove = translator.GetDelegate<System.Action<Game.LuaItem>>(L, 2);
                    
                    Game.LuaUtility.UnBindOnItemRemove( eventHandler, onItemRemove );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DynamicRemoveItem_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    Game.LuaSimpleList eventHandler = (Game.LuaSimpleList)translator.GetObject(L, 1, typeof(Game.LuaSimpleList));
                    Game.LuaItem item = (Game.LuaItem)translator.GetObject(L, 2, typeof(Game.LuaItem));
                    
                    Game.LuaUtility.DynamicRemoveItem( eventHandler, item );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReleaseLuaSimpleList_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    Game.LuaSimpleList eventHandler = (Game.LuaSimpleList)translator.GetObject(L, 1, typeof(Game.LuaSimpleList));
                    
                    Game.LuaUtility.ReleaseLuaSimpleList( eventHandler );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetComponentEnabled_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component component = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    bool enabled = LuaAPI.lua_toboolean(L, 2);
                    
                    Game.LuaUtility.SetComponentEnabled( component, enabled );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_BindLoopListEvent_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    Game.LoopList eventHandler = (Game.LoopList)translator.GetObject(L, 1, typeof(Game.LoopList));
                    System.Action<int, Game.LuaItem> onSetValue = translator.GetDelegate<System.Action<int, Game.LuaItem>>(L, 2);
                    
                    Game.LuaUtility.BindLoopListEvent( eventHandler, onSetValue );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_BindLoopListClick_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    Game.LoopList eventHandler = (Game.LoopList)translator.GetObject(L, 1, typeof(Game.LoopList));
                    System.Action<int> onClick = translator.GetDelegate<System.Action<int>>(L, 2);
                    
                    Game.LuaUtility.BindLoopListClick( eventHandler, onClick );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UnBindLoopListEvent_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    Game.LoopList eventHandler = (Game.LoopList)translator.GetObject(L, 1, typeof(Game.LoopList));
                    
                    Game.LuaUtility.UnBindLoopListEvent( eventHandler );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UnBindLoopListClick_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    Game.LoopList eventHandler = (Game.LoopList)translator.GetObject(L, 1, typeof(Game.LoopList));
                    
                    Game.LuaUtility.UnBindLoopListClick( eventHandler );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LuaSimpleListInit_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component luaSimpleList = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    
                    Game.LuaUtility.LuaSimpleListInit( luaSimpleList );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetLuaSimpleListCount_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component luaSimpleList = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    int count = LuaAPI.xlua_tointeger(L, 2);
                    
                    Game.LuaUtility.SetLuaSimpleListCount( luaSimpleList, count );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Tip_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component bubble = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    string text = LuaAPI.lua_tostring(L, 2);
                    
                    Game.LuaUtility.Tip( bubble, text );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Warning_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component bubble = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    string text = LuaAPI.lua_tostring(L, 2);
                    
                    Game.LuaUtility.Warning( bubble, text );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PopHp_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component bubble = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    int whoId = LuaAPI.xlua_tointeger(L, 2);
                    UnityEngine.Transform who = (UnityEngine.Transform)translator.GetObject(L, 3, typeof(UnityEngine.Transform));
                    int hpType = LuaAPI.xlua_tointeger(L, 4);
                    int hp = LuaAPI.xlua_tointeger(L, 5);
                    
                    Game.LuaUtility.PopHp( bubble, whoId, who, hpType, hp );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RemoveHpPop_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component bubble = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    int whoId = LuaAPI.xlua_tointeger(L, 2);
                    
                    Game.LuaUtility.RemoveHpPop( bubble, whoId );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_BubbleClear_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component bubble = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    
                    Game.LuaUtility.BubbleClear( bubble );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Rotate_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Transform transform = (UnityEngine.Transform)translator.GetObject(L, 1, typeof(UnityEngine.Transform));
                    float angle = (float)LuaAPI.lua_tonumber(L, 2);
                    
                    Game.LuaUtility.Rotate( transform, angle );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_TransformGetPos_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Transform transform = (UnityEngine.Transform)translator.GetObject(L, 1, typeof(UnityEngine.Transform));
                    float x;
                    float y;
                    float z;
                    
                    Game.LuaUtility.TransformGetPos( transform, out x, out y, out z );
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
        static int _m_TransformSetPos_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 4&& translator.Assignable<UnityEngine.Transform>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    UnityEngine.Transform transform = (UnityEngine.Transform)translator.GetObject(L, 1, typeof(UnityEngine.Transform));
                    float x = (float)LuaAPI.lua_tonumber(L, 2);
                    float y = (float)LuaAPI.lua_tonumber(L, 3);
                    float z = (float)LuaAPI.lua_tonumber(L, 4);
                    
                    Game.LuaUtility.TransformSetPos( transform, x, y, z );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 2&& translator.Assignable<UnityEngine.Component>(L, 1)&& translator.Assignable<UnityEngine.Component>(L, 2)) 
                {
                    UnityEngine.Component targetTrans = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    UnityEngine.Component fromTrans = (UnityEngine.Component)translator.GetObject(L, 2, typeof(UnityEngine.Component));
                    
                    Game.LuaUtility.TransformSetPos( targetTrans, fromTrans );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Game.LuaUtility.TransformSetPos!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_TransformGetDir_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Transform transform = (UnityEngine.Transform)translator.GetObject(L, 1, typeof(UnityEngine.Transform));
                    float x;
                    float y;
                    float z;
                    
                    Game.LuaUtility.TransformGetDir( transform, out x, out y, out z );
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
        static int _m_ClonePosition_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Transform transform = (UnityEngine.Transform)translator.GetObject(L, 1, typeof(UnityEngine.Transform));
                    UnityEngine.Transform src = (UnityEngine.Transform)translator.GetObject(L, 2, typeof(UnityEngine.Transform));
                    
                    Game.LuaUtility.ClonePosition( transform, src );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CalculateTransformDist_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Transform transA = (UnityEngine.Transform)translator.GetObject(L, 1, typeof(UnityEngine.Transform));
                    UnityEngine.Transform transB = (UnityEngine.Transform)translator.GetObject(L, 2, typeof(UnityEngine.Transform));
                    
                        float __cl_gen_ret = Game.LuaUtility.CalculateTransformDist( transA, transB );
                        LuaAPI.lua_pushnumber(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_TimeConvertSecondToHHMM_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    float seconds = (float)LuaAPI.lua_tonumber(L, 1);
                    
                        string __cl_gen_ret = Game.LuaUtility.TimeConvertSecondToHHMM( seconds );
                        LuaAPI.lua_pushstring(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DoScale_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component component = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    float x = (float)LuaAPI.lua_tonumber(L, 2);
                    float y = (float)LuaAPI.lua_tonumber(L, 3);
                    float z = (float)LuaAPI.lua_tonumber(L, 4);
                    float duration = (float)LuaAPI.lua_tonumber(L, 5);
                    DG.Tweening.TweenCallback callback = translator.GetDelegate<DG.Tweening.TweenCallback>(L, 6);
                    
                    Game.LuaUtility.DoScale( component, x, y, z, duration, callback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DoAnimation_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component component = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    
                    Game.LuaUtility.DoAnimation( component );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DoAnimationByID_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component component = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    string animID = LuaAPI.lua_tostring(L, 2);
                    
                    Game.LuaUtility.DoAnimationByID( component, animID );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DoRectTransformMove_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component component = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    float x = (float)LuaAPI.lua_tonumber(L, 2);
                    float y = (float)LuaAPI.lua_tonumber(L, 3);
                    float duration = (float)LuaAPI.lua_tonumber(L, 4);
                    
                    Game.LuaUtility.DoRectTransformMove( component, x, y, duration );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DoMove_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 6&& translator.Assignable<UnityEngine.Component>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& translator.Assignable<DG.Tweening.TweenCallback>(L, 6)) 
                {
                    UnityEngine.Component component = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    float x = (float)LuaAPI.lua_tonumber(L, 2);
                    float y = (float)LuaAPI.lua_tonumber(L, 3);
                    float z = (float)LuaAPI.lua_tonumber(L, 4);
                    float duration = (float)LuaAPI.lua_tonumber(L, 5);
                    DG.Tweening.TweenCallback callback = translator.GetDelegate<DG.Tweening.TweenCallback>(L, 6);
                    
                    Game.LuaUtility.DoMove( component, x, y, z, duration, callback );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 5&& translator.Assignable<UnityEngine.Component>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)) 
                {
                    UnityEngine.Component component = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    float x = (float)LuaAPI.lua_tonumber(L, 2);
                    float y = (float)LuaAPI.lua_tonumber(L, 3);
                    float z = (float)LuaAPI.lua_tonumber(L, 4);
                    float duration = (float)LuaAPI.lua_tonumber(L, 5);
                    
                    Game.LuaUtility.DoMove( component, x, y, z, duration );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Game.LuaUtility.DoMove!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetScale_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component component = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    float x = (float)LuaAPI.lua_tonumber(L, 2);
                    float y = (float)LuaAPI.lua_tonumber(L, 3);
                    float z = (float)LuaAPI.lua_tonumber(L, 4);
                    
                    Game.LuaUtility.SetScale( component, x, y, z );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadScene_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 3&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& translator.Assignable<FrameWork.SceneLoader.OnLevelLoad>(L, 2)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)) 
                {
                    string levelName = LuaAPI.lua_tostring(L, 1);
                    FrameWork.SceneLoader.OnLevelLoad onLoad = translator.GetDelegate<FrameWork.SceneLoader.OnLevelLoad>(L, 2);
                    bool isAdded = LuaAPI.lua_toboolean(L, 3);
                    
                    Game.LuaUtility.LoadScene( levelName, onLoad, isAdded );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 2&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& translator.Assignable<FrameWork.SceneLoader.OnLevelLoad>(L, 2)) 
                {
                    string levelName = LuaAPI.lua_tostring(L, 1);
                    FrameWork.SceneLoader.OnLevelLoad onLoad = translator.GetDelegate<FrameWork.SceneLoader.OnLevelLoad>(L, 2);
                    
                    Game.LuaUtility.LoadScene( levelName, onLoad );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Game.LuaUtility.LoadScene!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetAllBuildScene_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    XLua.LuaTable table = (XLua.LuaTable)translator.GetObject(L, 1, typeof(XLua.LuaTable));
                    
                    Game.LuaUtility.GetAllBuildScene( table );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InjectSceneSet_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    XLua.LuaTable luaTable = (XLua.LuaTable)translator.GetObject(L, 1, typeof(XLua.LuaTable));
                    
                    Game.LuaUtility.InjectSceneSet( luaTable );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InitGlobalSetting_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.GameObject cameraGo = (UnityEngine.GameObject)translator.GetObject(L, 1, typeof(UnityEngine.GameObject));
                    FrameWork.CameraFollow cameraFollow = (FrameWork.CameraFollow)translator.GetObject(L, 2, typeof(FrameWork.CameraFollow));
                    
                    Game.LuaUtility.InitGlobalSetting( cameraGo, cameraFollow );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetTerrainHeight_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    float x = (float)LuaAPI.lua_tonumber(L, 1);
                    float z = (float)LuaAPI.lua_tonumber(L, 2);
                    
                        float __cl_gen_ret = Game.LuaUtility.GetTerrainHeight( x, z );
                        LuaAPI.lua_pushnumber(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetNavMeshPos_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    float x = (float)LuaAPI.lua_tonumber(L, 1);
                    float y = (float)LuaAPI.lua_tonumber(L, 2);
                    float z = (float)LuaAPI.lua_tonumber(L, 3);
                    float ox;
                    float oy;
                    float oz;
                    
                    Game.LuaUtility.GetNavMeshPos( x, y, z, out ox, out oy, out oz );
                    LuaAPI.lua_pushnumber(L, ox);
                        
                    LuaAPI.lua_pushnumber(L, oy);
                        
                    LuaAPI.lua_pushnumber(L, oz);
                        
                    
                    
                    
                    return 3;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadSprite_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string res = LuaAPI.lua_tostring(L, 1);
                    System.Action<Game.Sprite> onLoaded = translator.GetDelegate<System.Action<Game.Sprite>>(L, 2);
                    
                    Game.LuaUtility.LoadSprite( res, onLoaded );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadSceneItem_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string res = LuaAPI.lua_tostring(L, 1);
                    System.Action<Game.SceneItem> onLoaded = translator.GetDelegate<System.Action<Game.SceneItem>>(L, 2);
                    
                    Game.LuaUtility.LoadSceneItem( res, onLoaded );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadBasicModel_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string res = LuaAPI.lua_tostring(L, 1);
                    int modelType = LuaAPI.xlua_tointeger(L, 2);
                    System.Action<Game.BasicModel> onLoaded = translator.GetDelegate<System.Action<Game.BasicModel>>(L, 3);
                    
                    Game.LuaUtility.LoadBasicModel( res, modelType, onLoaded );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadFurnitureItem_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string res = LuaAPI.lua_tostring(L, 1);
                    int layer = LuaAPI.xlua_tointeger(L, 2);
                    System.Action<Game.FurnitureItem> onLoaded = translator.GetDelegate<System.Action<Game.FurnitureItem>>(L, 3);
                    
                    Game.LuaUtility.LoadFurnitureItem( res, layer, onLoaded );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadEffect_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string res = LuaAPI.lua_tostring(L, 1);
                    int etype = LuaAPI.xlua_tointeger(L, 2);
                    System.Action<Game.Effect> onLoaded = translator.GetDelegate<System.Action<Game.Effect>>(L, 3);
                    
                    Game.LuaUtility.LoadEffect( res, etype, onLoaded );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CommonResCache_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    System.Action cb = translator.GetDelegate<System.Action>(L, 1);
                    string[] atlas = translator.GetParams<string>(L, 2);
                    
                    Game.LuaUtility.CommonResCache( cb, atlas );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DestorySplash_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    Game.LuaUtility.DestorySplash(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ComponentGameObjVisible_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component comp = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    bool bVisible = LuaAPI.lua_toboolean(L, 2);
                    
                    Game.LuaUtility.ComponentGameObjVisible( comp, bVisible );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ComponenRenderVisible_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component comp = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    bool bVisible = LuaAPI.lua_toboolean(L, 2);
                    
                    Game.LuaUtility.ComponenRenderVisible( comp, bVisible );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_FollowCameraFollow_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component cameraFollow = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    Game.Sprite sprite = (Game.Sprite)translator.GetObject(L, 2, typeof(Game.Sprite));
                    
                    Game.LuaUtility.FollowCameraFollow( cameraFollow, sprite );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetFollowCameraRotY_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component cameraFollow = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    
                        float __cl_gen_ret = Game.LuaUtility.GetFollowCameraRotY( cameraFollow );
                        LuaAPI.lua_pushnumber(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetBuildmodelOffset_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component cameraFollow = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    
                    Game.LuaUtility.SetBuildmodelOffset( cameraFollow );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RecoverBuildmodelOffset_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component cameraFollow = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    
                    Game.LuaUtility.RecoverBuildmodelOffset( cameraFollow );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DragCameraFocus_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component dragCameraFollow = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    float x = (float)LuaAPI.lua_tonumber(L, 2);
                    float y = (float)LuaAPI.lua_tonumber(L, 3);
                    float z = (float)LuaAPI.lua_tonumber(L, 4);
                    
                    Game.LuaUtility.DragCameraFocus( dragCameraFollow, x, y, z );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DragCameraSetPos_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component dragCameraFollow = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    float x = (float)LuaAPI.lua_tonumber(L, 2);
                    float y = (float)LuaAPI.lua_tonumber(L, 3);
                    float z = (float)LuaAPI.lua_tonumber(L, 4);
                    
                    Game.LuaUtility.DragCameraSetPos( dragCameraFollow, x, y, z );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Follow3DBy2DFollowTrans_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 4&& translator.Assignable<UnityEngine.Component>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    UnityEngine.Component followScrpit = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    float x = (float)LuaAPI.lua_tonumber(L, 2);
                    float y = (float)LuaAPI.lua_tonumber(L, 3);
                    float z = (float)LuaAPI.lua_tonumber(L, 4);
                    
                    Game.LuaUtility.Follow3DBy2DFollowTrans( followScrpit, x, y, z );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 2&& translator.Assignable<UnityEngine.Component>(L, 1)&& translator.Assignable<UnityEngine.Transform>(L, 2)) 
                {
                    UnityEngine.Component followScrpit = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    UnityEngine.Transform trans = (UnityEngine.Transform)translator.GetObject(L, 2, typeof(UnityEngine.Transform));
                    
                    Game.LuaUtility.Follow3DBy2DFollowTrans( followScrpit, trans );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Game.LuaUtility.Follow3DBy2DFollowTrans!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Follow3DBy2DFollowTransDelta_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component followScrpit = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    UnityEngine.Transform trans = (UnityEngine.Transform)translator.GetObject(L, 2, typeof(UnityEngine.Transform));
                    float x = (float)LuaAPI.lua_tonumber(L, 3);
                    float y = (float)LuaAPI.lua_tonumber(L, 4);
                    float z = (float)LuaAPI.lua_tonumber(L, 5);
                    
                    Game.LuaUtility.Follow3DBy2DFollowTransDelta( followScrpit, trans, x, y, z );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DebuggerLog_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string msg = LuaAPI.lua_tostring(L, 1);
                    
                    Game.LuaUtility.DebuggerLog( msg );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_MaskInput_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    bool enable = LuaAPI.lua_toboolean(L, 1);
                    
                    Game.LuaUtility.MaskInput( enable );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InjectTransChilds_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    XLua.LuaTable self = (XLua.LuaTable)translator.GetObject(L, 1, typeof(XLua.LuaTable));
                    UnityEngine.Transform trans = (UnityEngine.Transform)translator.GetObject(L, 2, typeof(UnityEngine.Transform));
                    
                    Game.LuaUtility.InjectTransChilds( self, trans );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DestroyGameObject_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string objName = LuaAPI.lua_tostring(L, 1);
                    
                    Game.LuaUtility.DestroyGameObject( objName );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RadarManagerMoveRadarPos_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component radarManager = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    int id = LuaAPI.xlua_tointeger(L, 2);
                    float x = (float)LuaAPI.lua_tonumber(L, 3);
                    float z = (float)LuaAPI.lua_tonumber(L, 4);
                    float dirx = (float)LuaAPI.lua_tonumber(L, 5);
                    float diry = (float)LuaAPI.lua_tonumber(L, 6);
                    
                    Game.LuaUtility.RadarManagerMoveRadarPos( radarManager, id, x, z, dirx, diry );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RadarManagerAddRadar_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component radarManager = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    int id = LuaAPI.xlua_tointeger(L, 2);
                    float x = (float)LuaAPI.lua_tonumber(L, 3);
                    float z = (float)LuaAPI.lua_tonumber(L, 4);
                    string imgName = LuaAPI.lua_tostring(L, 5);
                    
                    Game.LuaUtility.RadarManagerAddRadar( radarManager, id, x, z, imgName );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RadarManagerRemoveRadar_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component radarManager = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    int id = LuaAPI.xlua_tointeger(L, 2);
                    
                    Game.LuaUtility.RadarManagerRemoveRadar( radarManager, id );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RadarManagerRotatePlayerIcon_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component radarManager = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    float dirx = (float)LuaAPI.lua_tonumber(L, 2);
                    float diry = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    Game.LuaUtility.RadarManagerRotatePlayerIcon( radarManager, dirx, diry );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RadarManagerChangeIcon_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component radarManager = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    int id = LuaAPI.xlua_tointeger(L, 2);
                    string iconName = LuaAPI.lua_tostring(L, 3);
                    
                    Game.LuaUtility.RadarManagerChangeIcon( radarManager, id, iconName );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RadarManagerDestroyRadarItems_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component radarManager = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    
                    Game.LuaUtility.RadarManagerDestroyRadarItems( radarManager );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RadarManagerOutBorder_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component radarManager = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    float x = (float)LuaAPI.lua_tonumber(L, 2);
                    float z = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    Game.LuaUtility.RadarManagerOutBorder( radarManager, x, z );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RadarManagerInitBorderRot_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component radarManager = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    float x = (float)LuaAPI.lua_tonumber(L, 2);
                    float z = (float)LuaAPI.lua_tonumber(L, 3);
                    float offsetX = (float)LuaAPI.lua_tonumber(L, 4);
                    float offsetZ = (float)LuaAPI.lua_tonumber(L, 5);
                    
                    Game.LuaUtility.RadarManagerInitBorderRot( radarManager, x, z, offsetX, offsetZ );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RegisterPlayerTriggers_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component trigger = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    System.Action<int, int> enterCallBack = translator.GetDelegate<System.Action<int, int>>(L, 2);
                    System.Action<int, int> exitCallBack = translator.GetDelegate<System.Action<int, int>>(L, 3);
                    
                    Game.LuaUtility.RegisterPlayerTriggers( trigger, enterCallBack, exitCallBack );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RegisterMonsterTriggers_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component trigger = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    System.Action<int, int> enterCallBack = translator.GetDelegate<System.Action<int, int>>(L, 2);
                    System.Action<int, int> exitCallBack = translator.GetDelegate<System.Action<int, int>>(L, 3);
                    
                    Game.LuaUtility.RegisterMonsterTriggers( trigger, enterCallBack, exitCallBack );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CloneLuaItem_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    Game.LuaItem template = (Game.LuaItem)translator.GetObject(L, 1, typeof(Game.LuaItem));
                    System.Action<Game.LuaItem> callBack = translator.GetDelegate<System.Action<Game.LuaItem>>(L, 2);
                    
                        Game.LuaItem __cl_gen_ret = Game.LuaUtility.CloneLuaItem( template, callBack );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetRendererLayer_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Transform root = (UnityEngine.Transform)translator.GetObject(L, 1, typeof(UnityEngine.Transform));
                    int layer = LuaAPI.xlua_tointeger(L, 2);
                    
                    Game.LuaUtility.SetRendererLayer( root, layer );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetLayer_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Transform root = (UnityEngine.Transform)translator.GetObject(L, 1, typeof(UnityEngine.Transform));
                    
                        int __cl_gen_ret = Game.LuaUtility.GetLayer( root );
                        LuaAPI.xlua_pushinteger(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ShowEquipDuration_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component equipDurationShow = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    int index = LuaAPI.xlua_tointeger(L, 2);
                    bool damaged = LuaAPI.lua_toboolean(L, 3);
                    
                    Game.LuaUtility.ShowEquipDuration( equipDurationShow, index, damaged );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetStorageCb_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 2&& translator.Assignable<System.Action<string, string>>(L, 1)&& translator.Assignable<System.Action<string, bool>>(L, 2)) 
                {
                    System.Action<string, string> loadCb = translator.GetDelegate<System.Action<string, string>>(L, 1);
                    System.Action<string, bool> saveCb = translator.GetDelegate<System.Action<string, bool>>(L, 2);
                    
                    Game.LuaUtility.SetStorageCb( loadCb, saveCb );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 1&& translator.Assignable<System.Action<string, string>>(L, 1)) 
                {
                    System.Action<string, string> loadCb = translator.GetDelegate<System.Action<string, string>>(L, 1);
                    
                    Game.LuaUtility.SetStorageCb( loadCb );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 0) 
                {
                    
                    Game.LuaUtility.SetStorageCb(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Game.LuaUtility.SetStorageCb!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_StartLoad_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string file = LuaAPI.lua_tostring(L, 1);
                    
                    Game.LuaUtility.StartLoad( file );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_StartSave_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string file = LuaAPI.lua_tostring(L, 1);
                    string data = LuaAPI.lua_tostring(L, 2);
                    
                    Game.LuaUtility.StartSave( file, data );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetSyncUrl_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string url = LuaAPI.lua_tostring(L, 1);
                    
                    Game.LuaUtility.SetSyncUrl( url );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetSyncCb_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 1&& translator.Assignable<System.Action<bool, string>>(L, 1)) 
                {
                    System.Action<bool, string> syncCb = translator.GetDelegate<System.Action<bool, string>>(L, 1);
                    
                    Game.LuaUtility.SetSyncCb( syncCb );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 0) 
                {
                    
                    Game.LuaUtility.SetSyncCb(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Game.LuaUtility.SetSyncCb!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_HttpSync_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string data = LuaAPI.lua_tostring(L, 1);
                    
                    Game.LuaUtility.HttpSync( data );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_NavTransManagerInit_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component navTransManager = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    UnityEngine.Transform trans = (UnityEngine.Transform)translator.GetObject(L, 2, typeof(UnityEngine.Transform));
                    
                    Game.LuaUtility.NavTransManagerInit( navTransManager, trans );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_NavTransManagerEnableTrans_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component navTransManager = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    bool enabled = LuaAPI.lua_toboolean(L, 2);
                    
                    Game.LuaUtility.NavTransManagerEnableTrans( navTransManager, enabled );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_NavTransManagerTrans_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component navTransManager = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    float x = (float)LuaAPI.lua_tonumber(L, 2);
                    float y = (float)LuaAPI.lua_tonumber(L, 3);
                    float z = (float)LuaAPI.lua_tonumber(L, 4);
                    float speed = (float)LuaAPI.lua_tonumber(L, 5);
                    System.Action completeAction = translator.GetDelegate<System.Action>(L, 6);
                    
                    Game.LuaUtility.NavTransManagerTrans( navTransManager, x, y, z, speed, completeAction );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_NavTransManagerUnBindNotify_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component navTransManager = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    
                    Game.LuaUtility.NavTransManagerUnBindNotify( navTransManager );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_NavTransManagerTransSetSpeed_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component navTransManager = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    float speed = (float)LuaAPI.lua_tonumber(L, 2);
                    
                    Game.LuaUtility.NavTransManagerTransSetSpeed( navTransManager, speed );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_NavTransManagerGetLength_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 4&& translator.Assignable<UnityEngine.Component>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    UnityEngine.Component navTransManager = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    float x = (float)LuaAPI.lua_tonumber(L, 2);
                    float y = (float)LuaAPI.lua_tonumber(L, 3);
                    float z = (float)LuaAPI.lua_tonumber(L, 4);
                    
                        float __cl_gen_ret = Game.LuaUtility.NavTransManagerGetLength( navTransManager, x, y, z );
                        LuaAPI.lua_pushnumber(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                if(__gen_param_count == 3&& translator.Assignable<UnityEngine.Component>(L, 1)&& translator.Assignable<UnityEngine.Transform>(L, 2)&& translator.Assignable<UnityEngine.Transform>(L, 3)) 
                {
                    UnityEngine.Component navTransManager = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    UnityEngine.Transform from = (UnityEngine.Transform)translator.GetObject(L, 2, typeof(UnityEngine.Transform));
                    UnityEngine.Transform to = (UnityEngine.Transform)translator.GetObject(L, 3, typeof(UnityEngine.Transform));
                    
                        float __cl_gen_ret = Game.LuaUtility.NavTransManagerGetLength( navTransManager, from, to );
                        LuaAPI.lua_pushnumber(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Game.LuaUtility.NavTransManagerGetLength!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_NavTransManagerMoveToTargetByTime_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component navTransManager = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    UnityEngine.Transform from = (UnityEngine.Transform)translator.GetObject(L, 2, typeof(UnityEngine.Transform));
                    UnityEngine.Transform to = (UnityEngine.Transform)translator.GetObject(L, 3, typeof(UnityEngine.Transform));
                    float time = (float)LuaAPI.lua_tonumber(L, 4);
                    float totalTime = (float)LuaAPI.lua_tonumber(L, 5);
                    
                    Game.LuaUtility.NavTransManagerMoveToTargetByTime( navTransManager, from, to, time, totalTime );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_NavTransManagerCalculatePath_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component navTransManager = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    UnityEngine.Transform to = (UnityEngine.Transform)translator.GetObject(L, 2, typeof(UnityEngine.Transform));
                    float time = (float)LuaAPI.lua_tonumber(L, 3);
                    float speed = (float)LuaAPI.lua_tonumber(L, 4);
                    
                        bool __cl_gen_ret = Game.LuaUtility.NavTransManagerCalculatePath( navTransManager, to, time, speed );
                        LuaAPI.lua_pushboolean(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_NavTransManagerBindPosNotify_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component navTransManager = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    System.Action<float, float, float> mov = translator.GetDelegate<System.Action<float, float, float>>(L, 2);
                    
                    Game.LuaUtility.NavTransManagerBindPosNotify( navTransManager, mov );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_NavTransManagerBindCompleteNotify_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component navTransManager = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    System.Action completed = translator.GetDelegate<System.Action>(L, 2);
                    
                    Game.LuaUtility.NavTransManagerBindCompleteNotify( navTransManager, completed );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LineManagerDrawLine_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component lineManager = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    UnityEngine.Transform from = (UnityEngine.Transform)translator.GetObject(L, 2, typeof(UnityEngine.Transform));
                    UnityEngine.Transform to = (UnityEngine.Transform)translator.GetObject(L, 3, typeof(UnityEngine.Transform));
                    int index = LuaAPI.xlua_tointeger(L, 4);
                    
                    Game.LuaUtility.LineManagerDrawLine( lineManager, from, to, index );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DrawLine_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component lineManager = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    UnityEngine.Transform start = (UnityEngine.Transform)translator.GetObject(L, 2, typeof(UnityEngine.Transform));
                    UnityEngine.Transform end = (UnityEngine.Transform)translator.GetObject(L, 3, typeof(UnityEngine.Transform));
                    int index = LuaAPI.xlua_tointeger(L, 4);
                    
                    Game.LuaUtility.DrawLine( lineManager, start, end, index );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ActiveLine_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component lineManager = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    bool enabled = LuaAPI.lua_toboolean(L, 2);
                    int index = LuaAPI.xlua_tointeger(L, 3);
                    
                    Game.LuaUtility.ActiveLine( lineManager, enabled, index );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_BarManagerAddItem_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component barManager = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    int ownerId = LuaAPI.xlua_tointeger(L, 2);
                    UnityEngine.Transform trans = (UnityEngine.Transform)translator.GetObject(L, 3, typeof(UnityEngine.Transform));
                    
                        Game.LuaInjector __cl_gen_ret = Game.LuaUtility.BarManagerAddItem( barManager, ownerId, trans );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_BarManagerAddItemByOffset_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component barManager = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    int ownerId = LuaAPI.xlua_tointeger(L, 2);
                    UnityEngine.Transform trans = (UnityEngine.Transform)translator.GetObject(L, 3, typeof(UnityEngine.Transform));
                    float x = (float)LuaAPI.lua_tonumber(L, 4);
                    float y = (float)LuaAPI.lua_tonumber(L, 5);
                    float z = (float)LuaAPI.lua_tonumber(L, 6);
                    
                        Game.LuaInjector __cl_gen_ret = Game.LuaUtility.BarManagerAddItemByOffset( barManager, ownerId, trans, x, y, z );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_BarManagerRemoveItem_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component barManager = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    int ownerId = LuaAPI.xlua_tointeger(L, 2);
                    
                    Game.LuaUtility.BarManagerRemoveItem( barManager, ownerId );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_HintManagerAddHintItem_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component hintManager = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    int ownerId = LuaAPI.xlua_tointeger(L, 2);
                    UnityEngine.Transform transRoot = (UnityEngine.Transform)translator.GetObject(L, 3, typeof(UnityEngine.Transform));
                    
                        Game.LuaInjector __cl_gen_ret = Game.LuaUtility.HintManagerAddHintItem( hintManager, ownerId, transRoot );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_HintManagerRemoveHintItem_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component hintManager = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    int ownerId = LuaAPI.xlua_tointeger(L, 2);
                    
                    Game.LuaUtility.HintManagerRemoveHintItem( hintManager, ownerId );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_HintManagerPositionHint_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component hintManager = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    UnityEngine.UI.Image image = (UnityEngine.UI.Image)translator.GetObject(L, 2, typeof(UnityEngine.UI.Image));
                    UnityEngine.Transform targetPos = (UnityEngine.Transform)translator.GetObject(L, 3, typeof(UnityEngine.Transform));
                    
                    Game.LuaUtility.HintManagerPositionHint( hintManager, image, targetPos );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PlaceDropItem_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component placeMngr = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    UnityEngine.Transform modelRes = (UnityEngine.Transform)translator.GetObject(L, 2, typeof(UnityEngine.Transform));
                    int posIndex = LuaAPI.xlua_tointeger(L, 3);
                    
                    Game.LuaUtility.PlaceDropItem( placeMngr, modelRes, posIndex );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RemoveDropItem_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Component placeMngr = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    UnityEngine.Transform modelRes = (UnityEngine.Transform)translator.GetObject(L, 2, typeof(UnityEngine.Transform));
                    
                    Game.LuaUtility.RemoveDropItem( placeMngr, modelRes );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_BindBuildingStart_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    System.Action<Game.FurnitureItem> onBuildingStart = translator.GetDelegate<System.Action<Game.FurnitureItem>>(L, 1);
                    
                    Game.LuaUtility.BindBuildingStart( onBuildingStart );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_BindBuildingSelect_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    System.Action<int> onBuildingSelect = translator.GetDelegate<System.Action<int>>(L, 1);
                    
                    Game.LuaUtility.BindBuildingSelect( onBuildingSelect );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_BindBuildingUpdate_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    System.Func<Game.FurnitureItem, bool> onBuildingUpdate = translator.GetDelegate<System.Func<Game.FurnitureItem, bool>>(L, 1);
                    
                    Game.LuaUtility.BindBuildingUpdate( onBuildingUpdate );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_BindPlaceBuild_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    System.Func<Game.FurnitureItem, int, int, bool> onPlaced = translator.GetDelegate<System.Func<Game.FurnitureItem, int, int, bool>>(L, 1);
                    
                    Game.LuaUtility.BindPlaceBuild( onPlaced );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_BindBuildingDelete_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    System.Action<int> onBuildingDelete = translator.GetDelegate<System.Action<int>>(L, 1);
                    
                    Game.LuaUtility.BindBuildingDelete( onBuildingDelete );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LocFurnitureItem_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    Game.FurnitureItem furnitureItem = (Game.FurnitureItem)translator.GetObject(L, 1, typeof(Game.FurnitureItem));
                    int dir = LuaAPI.xlua_tointeger(L, 2);
                    int index = LuaAPI.xlua_tointeger(L, 3);
                    
                    Game.LuaUtility.LocFurnitureItem( furnitureItem, dir, index );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_BuilderSet_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 1&& translator.Assignable<UnityEngine.Component>(L, 1)) 
                {
                    UnityEngine.Component PlaceMgr = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    
                    Game.LuaUtility.BuilderSet( PlaceMgr );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 0) 
                {
                    
                    Game.LuaUtility.BuilderSet(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Game.LuaUtility.BuilderSet!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_EnableBuilding_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    bool bEnable = LuaAPI.lua_toboolean(L, 1);
                    
                    Game.LuaUtility.EnableBuilding( bEnable );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CreateFurnitureItem_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string furnitureName = LuaAPI.lua_tostring(L, 1);
                    int layer = LuaAPI.xlua_tointeger(L, 2);
                    
                    Game.LuaUtility.CreateFurnitureItem( furnitureName, layer );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UpdateFurnitureItem_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string furnitureName = LuaAPI.lua_tostring(L, 1);
                    int layer = LuaAPI.xlua_tointeger(L, 2);
                    
                    Game.LuaUtility.UpdateFurnitureItem( furnitureName, layer );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PlaceFurniture_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    Game.LuaUtility.PlaceFurniture(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CancelBuilding_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    Game.LuaUtility.CancelBuilding(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DeleteSelectBuilding_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    Game.LuaUtility.DeleteSelectBuilding(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UpdateBuilding_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    Game.LuaUtility.UpdateBuilding(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RotateFurniture_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    Game.LuaUtility.RotateFurniture(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SelectFurniture_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    Game.FurnitureItem furnitureItem = (Game.FurnitureItem)translator.GetObject(L, 1, typeof(Game.FurnitureItem));
                    
                    Game.LuaUtility.SelectFurniture( furnitureItem );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddPlaceItem_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    Game.FurnitureItem furnitureItem = (Game.FurnitureItem)translator.GetObject(L, 1, typeof(Game.FurnitureItem));
                    
                    Game.LuaUtility.AddPlaceItem( furnitureItem );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RefreshSceneFurnitures_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    bool isFurniture = LuaAPI.lua_toboolean(L, 1);
                    
                    Game.LuaUtility.RefreshSceneFurnitures( isFurniture );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RecoverSceneFurnitures_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    Game.LuaUtility.RecoverSceneFurnitures(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CanDeleteSelectBuilding_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                        bool __cl_gen_ret = Game.LuaUtility.CanDeleteSelectBuilding(  );
                        LuaAPI.lua_pushboolean(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RecoverSelectBuilding_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    Game.LuaUtility.RecoverSelectBuilding(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}

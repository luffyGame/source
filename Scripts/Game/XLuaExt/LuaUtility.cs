using System;
using System.Collections.Generic;
using DG.Tweening;
using FrameWork;
using FrameWork.Ui;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using XLua;

namespace Game
{
    public static class LuaUtility
    {
        #region UI

        public static void LoadView(string res, Action<DlgView> onLoaded, bool top)
        {
            ViewLocator.Instance.LoadView(res, onLoaded, top ? 0 : 1);
        }

        public static void ButtonBindOnClick(Component but, UnityAction onClick)
        {
            ((Button) but).onClick.AddListener(onClick);
        }

        public static void ButtonUnBindOnClick(Component but, UnityAction onClick)
        {
            Button button = (Button) but;
            if(null!=onClick)
                button.onClick.RemoveListener(onClick);
            //onclick缓存bug
            button.onClick.RemoveAllListeners();
            button.onClick.Invoke();
        }

        //添加长按、弹起的事件注册和注销
        public static void UiBindLongPress(Component but, UnityAction longPress)
        {
            LongPressEventTrigger button = but as LongPressEventTrigger;
            button.onLongPress.AddListener(longPress);
        }

        public static void UiUnBindLongPress(Component but, UnityAction longPress)
        {
            LongPressEventTrigger button = but as LongPressEventTrigger;
            if(null!=longPress)
                button.onLongPress.RemoveListener(longPress);
            button.onLongPress.RemoveAllListeners();
            button.onLongPress.Invoke();
        }

        public static void UiBindLongPressCancel(Component but, UnityAction longPress)
        {
            LongPressEventTrigger button = but as LongPressEventTrigger;
            button.onLongPressCancel.AddListener(longPress);
        }

        public static void UiUnBindLongPressCancel(Component but, UnityAction longPress)
        {
            LongPressEventTrigger button = but as LongPressEventTrigger;
            if(null!=longPress)
                button.onLongPressCancel.RemoveListener(longPress);
            button.onLongPressCancel.RemoveAllListeners();
            button.onLongPressCancel.Invoke();
        }

        public static void UiBindOnPointerDown(Component uiElement, Action onPointerDown)
        {
            UiEventHandler uiHandler = (UiEventHandler) uiElement;
            uiHandler.onPointerDown = onPointerDown;
        }

        public static void UiBindOnPointerUp(Component uiElement, Action onPointerUp)
        {
            UiEventHandler uiHandler = (UiEventHandler) uiElement;
            uiHandler.onPointerUp = onPointerUp;
        }

        /// <summary>
        /// 设置Button的可交互状态
        /// </summary>
        public static void SetButtonInteractable(Component button, bool interactable)
        {
            ((Button) button).interactable = interactable;
        }
        /// <summary>
        /// 设置Button拓展的可交互状态
        /// </summary>
        public static void SetBtnExtendInteractable(Component btnExtend, bool interactable)
        {
            var btnExt = btnExtend.GetComponent<BtnExtend>();
            btnExt.IsInteractable = interactable;
        }

        public static void TextSetTxt(Component text, string txt)
        {
            ((Text) text).text = txt;
        }

        public static void TextAddTxt(Component text, string txt, bool newLine = true)
        {
            Text _text = (Text) text;
            _text.text += newLine ? "\n" + txt : txt;
        }

        public static void ImgSetSprite(Component img, string spriteRes, bool nativeSize)
        {
            Image image = (Image) img;
            image.sprite = CommonResMgr.Instance.GetSprite(spriteRes);
            if (nativeSize)
                image.SetNativeSize();
        }

        public static void ImgColorGray(Component img, bool gray)
        {
            Image image = (Image) img;
            if (gray)
                image.color = Color.gray;
            else
            {
                image.color = Color.white;
            }
        }

        public static void ImgSetColor(Component img, uint color)
        {
            ((Image) img).color = FrameWork.Utils.Parse_RGBA_Color(color);
        }

        public static void ImgSetAlpha(Component img, float alpha)
        {
            var image = img as Image;
            var color = image.color;
            color.a = alpha;
            image.color = color;
        }

        public static void ImgSetAmount(Component img, float v)
        {
            var image = img as Image;
            image.fillAmount = v;
        }

        public static void SliderSetValue(Component slider, float v)
        {
            ((Slider) slider).value = v;
        }

        public static string InputFieldGetText(Component input)
        {
            return ((InputField) input).text;
        }

        public static void SingleToggleSetValue(Component stogle, bool on)
        {
            ((SingleToggle) stogle).Turn(on);
        }

        public static void ScrollRectSetPos(Component scrollRect, float pos, bool isVertical, bool needDelay)
        {
            ScrollRect _scroll = (ScrollRect) scrollRect;
            if (needDelay)
            {
                _scroll.StartCoroutine(FrameWork.Utils.DelayAction(() =>
                {
                    if (isVertical)
                        _scroll.verticalNormalizedPosition = pos;
                    else
                    {
                        _scroll.horizontalNormalizedPosition = pos;
                    }
                }));
            }
            else
            {
                if (isVertical)
                    _scroll.verticalNormalizedPosition = pos;
                else
                {
                    _scroll.horizontalNormalizedPosition = pos;
                }
            }
        }

        public static void JoystickBind(Component joyStick, Action<float, float, float> mov, Action begin, Action end)
        {
            JoyStick js = (JoyStick) joyStick;
            js.move = mov;
            js.begin = begin;
            js.end = end;
        }

       
        /// <summary>
        /// 绑定拖拽相关事件
        /// </summary>
        public static void BindDragEvents(Component eventHandler, Action onClick, Action onBeginDrag,
            Action<float, float> onDrag, Action<float,float> onEndDrag, Action onDrop)
        {
            var eventHdl = eventHandler as EventHandler;
            eventHdl.onBeginDrag = onBeginDrag;
            eventHdl.onDrag = onDrag;
            eventHdl.onEndDrag = onEndDrag;
            eventHdl.onDrop = onDrop;
            eventHdl.onClick = onClick;
        }

        /// <summary>
        /// 绑定Item添加事件
        /// </summary>
        public static void BindOnItemAdd(LuaSimpleList eventHandler, Action<LuaItem> onItemAdd)
        {
            eventHandler.onItemAdd = onItemAdd;
        }

        public static void UnBindOnItemAdd(LuaSimpleList eventHandler)
        {
            eventHandler.onItemAdd = null;
        }

        /// <summary>
        /// 绑定Item移除事件
        /// </summary>
        public static void BindOnItemRemove(LuaSimpleList eventHandler, Action<LuaItem> onItemRemove)
        {
            eventHandler.onItemRemove = onItemRemove;
        }

        /// <summary>
        /// 解绑Item移除
        /// </summary>
        /// <param name="eventHandler"></param>
        /// <param name="onItemRemove"></param>
        public static void UnBindOnItemRemove(LuaSimpleList eventHandler, Action<LuaItem> onItemRemove)
        {
            eventHandler.onItemRemove = null;
        }

        /// <summary>
        /// 强制移除Item
        /// </summary>
        /// <param name="eventHandler"></param>
        /// <param name="item"></param>
        public static void DynamicRemoveItem(LuaSimpleList eventHandler, LuaItem item)
        {
            eventHandler.DynamicRemoveItem(item);
        }

        /// <summary>
        /// 释放LuaSimpleList，移除所有LuaItem
        /// </summary>
        /// <param name="eventHandler"></param>
        public static void ReleaseLuaSimpleList(LuaSimpleList eventHandler)
        {
            eventHandler.count = 0;
            eventHandler.Init();
        }

       

        /// <summary>
        /// 设置Component的激活状态
        /// </summary>
        public static void SetComponentEnabled(Component component, bool enabled)
        {
            var behaviour = component as Behaviour;
            behaviour.enabled = enabled;
        }

        /// <summary>
        /// 绑定循环列表赋值
        /// </summary>
        public static void BindLoopListEvent(LoopList eventHandler, Action<int, LuaItem> onSetValue)
        {
            eventHandler.onSetValue = onSetValue;
        }

        /// <summary>
        /// 绑定循环列表点击事件
        /// </summary>
        public static void BindLoopListClick(LoopList eventHandler, Action<int> onClick)
        {
            eventHandler.onClick = onClick;
        }

        /// <summary>
        /// 解绑循环列表赋值
        /// </summary>
        public static void UnBindLoopListEvent(LoopList eventHandler)
        {
            eventHandler.onSetValue = null;
        }

        /// <summary>
        /// 解绑循环列表的点击事件
        /// </summary>
        public static void UnBindLoopListClick(LoopList eventHandler)
        {
            eventHandler.onClick = null;
        }

        /// <summary>
        /// Lua背包初始化
        /// </summary>
        public static void LuaSimpleListInit(Component luaSimpleList)
        {
            luaSimpleList.GetComponent<LuaSimpleList>().Init();
        }

        /// <summary>
        /// 设置Lua背包的格子数量
        /// </summary>
        public static void SetLuaSimpleListCount(Component luaSimpleList, int count)
        {
            luaSimpleList.GetComponent<LuaSimpleList>().count = count;
        }

        #region bubble ui

        public static void Tip(Component bubble, string text)
        {
            ((BubbleText) bubble).Tip(text);
        }

        public static void Warning(Component bubble, string text)
        {
            ((BubbleText) bubble).Warning(text);
        }

        public static void PopHp(Component bubble, int whoId, Transform who, int hpType, int hp)
        {
            ((BubbleText) bubble).PopHp(whoId, who, hpType, hp);
        }

        public static void RemoveHpPop(Component bubble, int whoId)
        {
            ((BubbleText) bubble).RemoveHp(whoId);
        }

        public static void BubbleClear(Component bubble)
        {
            ((BubbleText) bubble).Clear();
        }

       

        #endregion

        #endregion

        #region Transform

        public static void Rotate(Transform transform, float angle)
        {
            var euler = transform.localEulerAngles;
            euler.y += angle;
            transform.localEulerAngles = euler;
        }

        public static void TransformGetPos(Transform transform, out float x, out float y, out float z)
        {
            Vector3 pos = transform.position;
            x = pos.x;
            y = pos.y;
            z = pos.z;
        }

        public static void TransformSetPos(Transform transform, float x, float y, float z)
        {
            transform.position = new Vector3(x, y, z);
        }

        public static void TransformGetDir(Transform transform, out float x, out float y, out float z)
        {
            Vector3 dir = transform.forward;
            x = dir.x;
            y = dir.y;
            z = dir.z;
        }

        public static void ClonePosition(Transform transform, Transform src)
        {
            transform.position = new Vector3(src.position.x, src.position.y, src.position.z);
        }

        public static float CalculateTransformDist(Transform transA, Transform transB)
        {
            float deltaX = transA.position.x - transB.position.x;
            float deltaY = transA.position.y - transB.position.y;
            float deltaZ = transA.position.z - transB.position.z;

            return Mathf.Sqrt(deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ);
        }

        //根据fromTrans设置位置targetTrans位置
        public static void TransformSetPos(Component targetTrans, Component fromTrans)
        {
            targetTrans.transform.position = fromTrans.transform.position;
        }

        #endregion

        #region Math

        public static string TimeConvertSecondToHHMM(float seconds)
        {
            TimeSpan time = TimeSpan.FromSeconds(seconds);
            return string.Format("{0:D2}h:{1:D2}m:{2:D2}s", time.Hours, time.Minutes, time.Seconds);
        }

        #endregion

        #region DoTween

        public static void DoScale(Component component, float x, float y, float z, float duration,
            TweenCallback callback)
        {
            if (callback != null)
            {
                component.transform.DOScale(new Vector3(x, y, z), duration).OnComplete(callback);
            }
            else
            {
                component.transform.DOScale(new Vector3(x, y, z), duration);
            }
        }

        public static void DoAnimation(Component component)
        {
            var doTweenAnim = component.GetComponent<DOTweenAnimation>();
            doTweenAnim.DORestart(true);
        }

        public static void DoAnimationByID(Component component, string animID)
        {
            var doTweenAnim = component.GetComponent<DOTweenAnimation>();
            doTweenAnim.DORestartById(animID);
        }


        public static void DoRectTransformMove(Component component, float x, float y, float duration)
        {
            var rectTransform = component.GetComponent<RectTransform>();
            rectTransform.DOAnchorPos(new Vector2(x, y), duration).SetEase(Ease.Linear);
        }

        public static void DoMove(Component component, float x, float y, float z, float duration,
            TweenCallback callback = null)
        {
            if (callback != null)
            {
                component.transform.DOMove(new Vector3(x, y, z), duration).OnComplete(callback);
            }
            else
            {
                component.transform.DOMove(new Vector3(x, y, z), duration);
            }
        }

        public static void SetScale(Component component, float x, float y, float z)
        {
            component.transform.localScale = new Vector3(x, y, z);
        }

        #endregion

        #region Scene

        public static void LoadScene(string levelName, SceneLoader.OnLevelLoad onLoad, bool isAdded = true)
        {
            SceneLoader.Instance.AsnycLoadLevel(levelName, onLoad, isAdded);
        }

        public static void GetAllBuildScene(LuaTable table)
        {
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; ++i)
            {
                string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
                int lastSlash = scenePath.LastIndexOf("/");
                string name = scenePath.Substring(lastSlash + 1, scenePath.LastIndexOf(".") - lastSlash - 1);
                table.Set(name, true);
            }
        }

        public static void InjectSceneSet(LuaTable luaTable)
        {
            GameObject luaObj = GameObject.FindGameObjectWithTag("LuaInject");
            if (null != luaObj)
            {
                LuaInjector injector = luaObj.GetComponent<LuaInjector>();
                if (null != injector)
                    injector.Inject(luaTable);
            }
        }

        public static void InitGlobalSetting(GameObject cameraGo, CameraFollow cameraFollow)
        {
            Global.Instance.SetMainCamera(cameraGo);
            Global.Instance.MainCameraFollow = cameraFollow;
        }

        public static float GetTerrainHeight(float x, float z)
        {
            return Terrain.activeTerrain.SampleHeight(new Vector3(x, 0f, z));
        }

        public static void GetNavMeshPos(float x, float y, float z, out float ox, out float oy, out float oz)
        {
            NavMeshHit navHit;
            if (NavMesh.SamplePosition(new Vector3(x, y, z), out navHit, 10, -1))
            {
                Vector3 pos = navHit.position;
                ox = pos.x;
                oy = pos.y;
                oz = pos.z;
            }
            else
            {
                ox = x;
                oy = y;
                oz = z;
            }
        }

        #endregion

        #region Object

        public static void LoadSprite(string res, Action<Sprite> onLoaded)
        {
            ObjLocator.Instance.LoadSprite(res, onLoaded);
        }

        public static void LoadSceneItem(string res, Action<SceneItem> onLoaded)
        {
            ObjLocator.Instance.LoadSceneItem(res, onLoaded);
        }

        public static void LoadBasicModel(string res,int modelType, Action<BasicModel> onLoaded)
        {
            ObjLocator.Instance.LoadBasicModel(res, (AssetType)modelType, onLoaded);
        }
        
        public static void LoadFurnitureItem(string res, int layer, Action<FurnitureItem> onLoaded)
        {
            ObjLocator.Instance.LoadFurnitureItem(res, layer, onLoaded);
        }

        public static void LoadEffect(string res, int etype, Action<Effect> onLoaded)
        {
            Effect effect = new Effect(res, etype);
            effect.Load(() => { onLoaded(effect); });
        }
        #endregion

        #region Utils

        public static void CommonResCache(Action cb, params string[] atlas)
        {
            CommonResMgr.Instance.CacheAtlas(atlas);
            CommonResMgr.Instance.Cache(cb);
        }

        public static void DestorySplash()
        {
            ((GameApp) GameApp.Instance).DestroySplash();
        }

        public static void ComponentGameObjVisible(Component comp, bool bVisible)
        {
            comp.gameObject.SetActive(bVisible);
        }

        public static void ComponenRenderVisible(Component comp, bool bVisible)
        {
            comp.gameObject.GetComponent<Renderer>().enabled = bVisible;
        }

        public static void FollowCameraFollow(Component cameraFollow, Sprite sprite)
        {
            CameraFollow cf = (CameraFollow) cameraFollow;
            cf.followTarget = sprite != null ? sprite.GetRootTrans() : null;
            cf.Loc();
        }

        public static float GetFollowCameraRotY(Component cameraFollow)
        {
            return ((CameraFollow) cameraFollow).yRot;
        }

        public static void SetBuildmodelOffset(Component cameraFollow)
        {
            ((CameraFollow)cameraFollow).SetBuildModelView();
        }

        public static void RecoverBuildmodelOffset(Component cameraFollow)
        {
            ((CameraFollow)cameraFollow).RecoverBuildModelView();
        }

        public static void DragCameraFocus(Component dragCameraFollow, float x, float y, float z)
        {
            ((CameraDrag) dragCameraFollow).Focus(x, y, z);
        }

        public static void DragCameraSetPos(Component dragCameraFollow, float x, float y, float z)
        {
            ((CameraDrag) dragCameraFollow).SetPos(x, y, z);
        }

        public static void Follow3DBy2DFollowTrans(Component followScrpit, float x, float y, float z)
        {
            Follow3DBy2D.FollowPos(followScrpit.gameObject, new Vector3(x, y, z));
        }

        public static void Follow3DBy2DFollowTrans(Component followScrpit, Transform trans)
        {
            Follow3DBy2D.FollowTrans(followScrpit.gameObject, trans, Vector3.zero);
        }

        public static void Follow3DBy2DFollowTransDelta(Component followScrpit, Transform trans, float x, float y,
            float z)
        {
            Follow3DBy2D.FollowTrans(followScrpit.gameObject, trans, new Vector3(x, y, z));
        }

        public static void DebuggerLog(string msg)
        {
            Debugger.Log(msg);
        }

        public static void MaskInput(bool enable)
        {
            Global.Instance.UiTopMask.SetActive(enable);
        }

        public static void InjectTransChilds(LuaTable self, Transform trans)
        {
            int count = trans.childCount;
            for (int i = 0; i < count; ++i)
            {
                Transform child = trans.GetChild(i);
                self.Set(child.name, child);
            }
        }

        public static void DestroyGameObject(string objName)
        {
            GameObject go = GameObject.Find(objName);
            if (go != null)
            {
                GameObject.Destroy(go);
            }
        }

       

        public static void RadarManagerMoveRadarPos(Component radarManager, int id, float x, float z, float dirx,
            float diry)
        {
            ((RadarManager) radarManager).MoveRadarPos(id, x, z, dirx, diry);
        }

        public static void RadarManagerAddRadar(Component radarManager, int id, float x, float z, string imgName)
        {
            ((RadarManager) radarManager).AddRadar(id, x, z, imgName);
        }

        public static void RadarManagerRemoveRadar(Component radarManager, int id)
        {
            ((RadarManager) radarManager).RemoveRadar(id);
        }

        public static void RadarManagerRotatePlayerIcon(Component radarManager, float dirx, float diry)
        {
            ((RadarManager) radarManager).RotatePlayerIcon(dirx, diry);
        }

        public static void RadarManagerChangeIcon(Component radarManager, int id, string iconName)
        {
            ((RadarManager) radarManager).ChangeIcon(id, iconName);
        }

        public static void RadarManagerDestroyRadarItems(Component radarManager)
        {
            ((RadarManager) radarManager).DestroyRadarItems();
        }

        public static void RadarManagerOutBorder(Component radarManager, float x, float z)
        {
            ((RadarManager) radarManager).OutBorder(x, z);
        }

        public static void RadarManagerInitBorderRot(Component radarManager, float x, float z, float offsetX,
            float offsetZ)
        {
            ((RadarManager) radarManager).InitBorderRot(x, z, offsetX, offsetZ);
        }

        

        public static void RegisterPlayerTriggers(Component trigger, Action<int, int> enterCallBack,
            Action<int, int> exitCallBack)
        {
            var triggerManager = trigger as EventTriggerManager;
            if (triggerManager != null)
            {
                foreach (var v in triggerManager.playerTriggers)
                {
                    v.enterEvent = enterCallBack;
                    v.exitEvent = exitCallBack;
                }
            }
        }

        public static void RegisterMonsterTriggers(Component trigger, Action<int, int> enterCallBack,
            Action<int, int> exitCallBack)
        {
            var triggerManager = trigger as EventTriggerManager;
            if (triggerManager != null)
            {
                foreach (var v in triggerManager.monsterTriggers)
                {
                    v.enterEvent = enterCallBack;
                    v.exitEvent = exitCallBack;
                }
            }
        }

     

        public static LuaItem CloneLuaItem(LuaItem template,Action<LuaItem> callBack)
        {
            var luaItem = CachedClone.Clone(template.gameObject).GetComponent<LuaItem>();
            if (callBack != null)
            {
                callBack(luaItem);
            }

            return luaItem;
        }

        public static void SetRendererLayer(Transform root,int layer)
        {
            var allRenderers = root.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in allRenderers)
            {
                renderer.gameObject.layer = layer;
            }
        }

        public static int GetLayer(Transform root)
        {
            return root.gameObject.layer;
        }

        public static void ShowEquipDuration(Component equipDurationShow, int index, bool damaged)
        {
            ((EquipDurationShow)equipDurationShow).ShowEquipDuration(index, damaged);
        }

        #endregion

        #region Storage
        public static void SetStorageCb(Action<string, string> loadCb = null, Action<string, bool> saveCb = null)
        {
            if (null == Storage.Instance)
                return;
            Storage.Instance.loadCb = loadCb;
            Storage.Instance.saveCb = saveCb;
        }

        public static void StartLoad(string file)
        {
            Storage.Instance.StartLoad(file);
        }

        public static void StartSave(string file, string data)
        {
            Storage.Instance.StartSave(file,data);
        }

        public static void SetSyncUrl(string url)
        {
            Storage.Instance.httpSyncUrl = url;
        }

        public static void SetSyncCb(Action<bool, string> syncCb = null)
        {
            Storage.Instance.syncDone = syncCb;
        }

        public static void HttpSync(string data)
        {
            Storage.Instance.Sync(data);
        }
        #endregion


        #region worldMap
        //navManager
        public static void NavTransManagerInit(Component navTransManager, Transform trans)
        {
            ((NavTransManager)navTransManager).Init(trans);
        }

        public static void NavTransManagerEnableTrans(Component navTransManager, bool enabled)
        {
            ((NavTransManager)navTransManager).EnableTrans(enabled);
        }

        public static void NavTransManagerTrans(Component navTransManager, float x, float y, float z, float speed, Action completeAction)
        {
            ((NavTransManager)navTransManager).notifyMoveCompleted = completeAction;
            ((NavTransManager)navTransManager).MoveToTargetBySpeed(new Vector3(x, y, z), speed);
        }

        public static void NavTransManagerUnBindNotify(Component navTransManager)
        {
            ((NavTransManager)navTransManager).UnBindNotify();
        }
        public static void NavTransManagerTransSetSpeed(Component navTransManager, float speed)
        {
            ((NavTransManager)navTransManager).SetSpeed(speed);
        }

        public static float NavTransManagerGetLength(Component navTransManager, float x, float y, float z)
        {
            return ((NavTransManager)navTransManager).GetPathLength(new Vector3(x, y, z));
        }

        public static float NavTransManagerGetLength(Component navTransManager, Transform from, Transform to)
        {
            return ((NavTransManager)navTransManager).GetPathLength(from, to);
        }

        public static void NavTransManagerMoveToTargetByTime(Component navTransManager, Transform from, Transform to, float time, float totalTime)
        {
            ((NavTransManager)navTransManager).MoveToTargetByTime(from, to, time, totalTime);
        }

        public static bool NavTransManagerCalculatePath(Component navTransManager, Transform to, float time, float speed)
        {
            return ((NavTransManager)navTransManager).SetTransPos(to, time, speed);
        }

        public static void NavTransManagerBindPosNotify(Component navTransManager, Action<float, float, float> mov)
        {
            ((NavTransManager)navTransManager).BindMoveCallback(mov);
        }

        public static void NavTransManagerBindCompleteNotify(Component navTransManager, Action completed)
        {
            ((NavTransManager)navTransManager).BindOnCompleteCallBack(completed);
        }

        //line
        public static void LineManagerDrawLine(Component lineManager, Transform from, Transform to, int index)
        {
            ((LineManager)lineManager).DrawPathLine(from, to, index);
        }

        public static void DrawLine(Component lineManager, Transform start, Transform end, int index)
        {
            ((LineManager)lineManager).DrawLine(start, end, index);
        }

        public static void ActiveLine(Component lineManager, bool enabled, int index)
        {
            ((LineManager)lineManager).ActiveLine(enabled, index);
        }

        //3dUI
        public static LuaInjector BarManagerAddItem(Component barManager, int ownerId, Transform trans)
        {
            Transform barItem = ((BarManager)barManager).AddBar(ownerId, trans, Vector3.zero);
            return barItem.GetComponent<LuaInjector>();
        }

        public static LuaInjector BarManagerAddItemByOffset(Component barManager, int ownerId, Transform trans, float x,
            float y, float z)
        {
            Transform barItem = ((BarManager)barManager).AddBar(ownerId, trans, new Vector3(x, y, z));
            return barItem.GetComponent<LuaInjector>();
        }

        public static void BarManagerRemoveItem(Component barManager, int ownerId)
        {
            ((BarManager)barManager).RemoveBar(ownerId);
        }

        public static LuaInjector HintManagerAddHintItem(Component hintManager, int ownerId, Transform transRoot)
        {
            Transform hintItem = ((HintManager)hintManager).AddHintItem(ownerId, transRoot);
            return hintItem.GetComponent<LuaInjector>();
        }

        public static void HintManagerRemoveHintItem(Component hintManager, int ownerId)
        {
            ((HintManager)hintManager).RemoveHintItem(ownerId);
        }

        public static void HintManagerPositionHint(Component hintManager, Image image, Transform targetPos)
        {
            ((HintManager)hintManager).PositionHint((Image)image, targetPos);
        }

        //drop
        public static void PlaceDropItem(Component placeMngr, Transform modelRes, int posIndex)
        {
            ((mapPlaceManager)placeMngr).PlaceDropItem(modelRes, posIndex);
        }

        public static void RemoveDropItem(Component placeMngr, Transform modelRes)
        {
            ((mapPlaceManager)placeMngr).RemoveDropItem(modelRes);
        }

        #endregion

        #region 建造

        /// <summary>
        /// 建造开始回调
        /// </summary>
        public static void BindBuildingStart(Action<FurnitureItem> onBuildingStart)
        {
            BuilderHelper.Instance.onBuildingStart = onBuildingStart;
        }

        /// <summary>
        /// 建造选中回调
        /// </summary>
        /// <param name="onBuildingSelect"></param>
        public static void BindBuildingSelect(Action<int> onBuildingSelect)
        {
            BuilderHelper.Instance.onBuildingSelect = onBuildingSelect;
        }

        /// <summary>
        /// 建筑升级回调
        /// </summary>
        /// <param name="onBuildingUpdate"></param>
        public static void BindBuildingUpdate(Func<FurnitureItem,bool> onBuildingUpdate)
        {
            BuilderHelper.Instance.onBuildingUpdate = onBuildingUpdate;
        }

        /// <summary>
        /// 建造完成回调
        /// </summary>
        public static void BindPlaceBuild(Func<FurnitureItem, int, int, bool> onPlaced)
        {
            BuilderHelper.Instance.onFurniturePlaced = onPlaced;
        }

        /// <summary>
        /// 建造删除回调
        /// </summary>
        /// <param name="onBuildingSelect"></param>
        public static void BindBuildingDelete(Action<int> onBuildingDelete)
        {
            BuilderHelper.Instance.onBuildingDelete = onBuildingDelete;
        }

        public static void LocFurnitureItem(FurnitureItem furnitureItem, int dir, int index)
        {
            BuilderHelper.Instance.LocFurniture(furnitureItem, dir, index);
        }

        public static void BuilderSet(Component PlaceMgr = null)
        {
            BuilderHelper.Instance.SetBuilderHelper((PlacementManager)PlaceMgr);
        }

        public static void EnableBuilding(bool bEnable)
        {
            BuilderHelper.Instance.EnableBuilding(bEnable);
        }

        public static void CreateFurnitureItem(string furnitureName, int layer)
        {
            BuilderHelper.Instance.BuildingFurniture(furnitureName, layer);
        }

        public static void UpdateFurnitureItem(string furnitureName, int layer)
        {
            BuilderHelper.Instance.UpdateSelectFurniture(furnitureName, layer);
        }

        public static void PlaceFurniture()
        {
            BuilderHelper.Instance.CompleteFurniture();
        }

        public static void CancelBuilding()
        {
            BuilderHelper.Instance.CancelBuilding();
        }

        public static void DeleteSelectBuilding()
        {
            BuilderHelper.Instance.DeleteSelectBuilding();
        }

        public static void UpdateBuilding()
        {
            //BuilderHelper.Instance.UpdateBuilding();
        }

        public static void RotateFurniture()
        {
            BuilderHelper.Instance.RotateFurniture();
        }

        public static void SelectFurniture(FurnitureItem furnitureItem)
        {
            BuilderHelper.Instance.SelectFurniture(furnitureItem);
        }

        /// <summary>
        /// LoadfurnitureItem数据，填入到placeItem中
        /// </summary>
        /// <param name="furnitureItem"></param>
        public static void AddPlaceItem(FurnitureItem furnitureItem)
        {
            BuilderHelper.Instance.PlaceBuildItem(furnitureItem);
        }

        /// <summary>
        /// 刷新场景中furnitures状态
        /// </summary>
        /// /// <param name="isFurniture">显示建筑还是工作台</param>
        public static void RefreshSceneFurnitures(bool isFurniture)
        {
            BuilderHelper.Instance.RefreshSceneFurnitures(isFurniture);
        }

        public static void RecoverSceneFurnitures()
        {
            BuilderHelper.Instance.RecoverSceneFurnitures();
        }

        public static bool CanDeleteSelectBuilding()
        {
            return BuilderHelper.Instance.CanDeleteSelectBuilding();
        }

        public static void RecoverSelectBuilding()
        {
            BuilderHelper.Instance.RecoverSelectFurniture();
        }

        #endregion
    }
}
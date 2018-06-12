using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameWork;
using Lean.Touch;

namespace Game
{
    public enum FurnitureType
    {
        ground,
        wall,
        furniture,
    }

    public enum FurnitureDirect : int
    {
        UNDEFINED = -1,
        east = 0,
        sourth,
        west,
        north,
        COUNT,
    }
    /// <summary>
    /// 创建的方法：
    /// </summary>
    public class BuilderHelper : SingleBehavior<BuilderHelper>
    {
        public PlacementManager placeMgr;//建筑定位
        private FurnitureItem buildingFurniture;//正在建的模型
        private FurnitureItem selectFurniture;//选中的模型
        private FurnitureItem lastFurniture;//上一个的建筑,用于创建当前模型的资源和依据上一个来定位当前位置
        public List<FurnitureItem> sceneFurnitures = new List<FurnitureItem>();
        public List<FurnitureItem> sceneBuildings = new List<FurnitureItem>();
        public bool isSelectBuilding = true; //当前操作目标: furniture/building

        //collider detect
        public List<FurnitureInfo> colliderFurnitures = new List<FurnitureInfo>();
        //trigger状态改变则刷新Building状态
        public bool TriggerStateChanged { get; set; }

        //进入/走出/关闭触发
        private int obstacleCollider = 0;
        public int ObstacleCollider {
            get { return obstacleCollider; }
            set
            {
                obstacleCollider = value > 0 ? value : 0;
            }
        }
        //state shader
        [System.NonSerialized]
        public Shader placingShader;
        [System.NonSerialized]
        public Shader placedShader;

        public Func<FurnitureItem, int,int,bool> onFurniturePlaced;
        public Action<FurnitureItem> onBuildingStart;
        public Action<int> onBuildingSelect;
        public Action<int> onBuildingDelete;
        public Func<FurnitureItem,bool> onBuildingUpdate;
        private bool isBuilingCreate;

        #region UnityEvent

        public override void OnInit()
        {
            placingShader = Shader.Find("xcyy/build/alphaColor");
            placedShader = Shader.Find("Standard");
        }

        public override void OnQuit()
        {
            CancelBuilding();
            sceneBuildings.Clear();
            sceneBuildings = null;
            sceneFurnitures.Clear();
            sceneFurnitures = null;
        }

        public void Update()
        {
            //检测建筑状态
            if (TriggerStateChanged)
            {
                DoBuildingCheck();
                TriggerStateChanged = false;
            }
        }

        #endregion

        #region FurnitureCollider

        private bool CanPlaceFurniture(FurnitureItem furniture)
        {
            if (null == furniture || ObstacleCollider > 0)
                return false;
            FurnitureInfo fi = furniture.TheInfo;
            bool canPlaceItem = false;
            switch (fi.furnitureType)
            {
                case FurnitureType.wall:
                    foreach (var temp in colliderFurnitures)
                    {
                        if (temp.furnitureType == FurnitureType.wall)
                        {
                            int tempNum = temp.direction - fi.direction;
                            if (tempNum % 2 == 0)
                            {
                                return false;
                            }
                        }

                        if (temp.furnitureType == FurnitureType.ground)
                        {
                            canPlaceItem = fi.layer >= temp.layer;
                        }
                    }
                    break;

                case FurnitureType.ground:
                    foreach (var temp in colliderFurnitures)
                    {
                        if (temp.furnitureType == FurnitureType.ground || temp.furnitureType == FurnitureType.furniture)
                        {
                            return false;
                        }
                    }

                    canPlaceItem = true;
                    break;

                case FurnitureType.furniture:
                    int floorCount = 0;
                    foreach (var temp in colliderFurnitures)
                    {
                        if (temp.furnitureType == FurnitureType.furniture || temp.furnitureType == FurnitureType.wall)
                        {
                            return false;
                        }

                        if (temp.furnitureType == FurnitureType.ground)
                        {
                            if (fi.layer > temp.layer)
                                return false;
                            floorCount++;
                        }
                    }
                    canPlaceItem = (fi.furnitureSpace <= floorCount);
                    break;
            }
            return canPlaceItem;
        }

        public void AddColliderFurniture(FurnitureInfo fi)
        {
            if(!colliderFurnitures.Contains(fi))
                colliderFurnitures.Add(fi);
        }

        public void RemoveColliderFurniture(FurnitureInfo fi)
        {
            colliderFurnitures.Remove(fi);
        }

        #endregion

        #region 初始化
        //初始化放置位置
        public void SetBuilderHelper(PlacementManager placeMgr)
        {
            this.placeMgr = placeMgr;
        }

        public void EnableBuilding(bool bEnable)
        {
            if (bEnable)
                LeanTouch.OnFingerDown += OnFingerDown;
            else
            {
                LeanTouch.OnFingerDown -= OnFingerDown;
                CancelBuilding();
            }
        }

        private void OnFingerDown(LeanFinger finger)
        {
            if(LeanTouch.GuiInUse)
                return;
            /*
            {
#if (UNITY_IPHONE || UNITY_ANDROID)
                if ((Input.touchCount > 0) && UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                {
                    return;
                }
#endif

#if UNITY_EDITOR
                if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
                {
                    return;
                }
#endif
            }*/
            if (isBuilingCreate)
            {
                if(buildingFurniture==null)
                    return;
                Ray ray = Global.Instance.MainCamera.ScreenPointToRay(finger.ScreenPosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 300f, Const.LAYER_BUILD_GROUND_MASK))
                {
                    MoveFurniture(hit.point);
                }
            }
            else
            {
                Ray ray = Global.Instance.MainCamera.ScreenPointToRay(finger.ScreenPosition);
                RaycastHit hit;
                //修改，回调到lua
                if (Physics.Raycast(ray, out hit, 300f, isSelectBuilding ? Const.LAYER_BUILDING_COLLIDER_MASK : Const.LAYER_FURNITURE_COLLIDER_MASK))
                {
                    if (onBuildingSelect != null)
                    {
                        var furnitureCollider = hit.transform.GetComponent<FurnitureCollider>();
                        if(furnitureCollider)
                        {
                            onBuildingSelect(furnitureCollider.info.id);
                        }
                    }
                }
            }
        }
        

        #endregion
        #region 建造接口
        //lua端调用，建造
        public void BuildingFurniture(string resName,int layer)
        {
            if(null!=buildingFurniture)
            {
                CancelBuilding();
            }
            isBuilingCreate = true;
            ObstacleCollider = 0;
            ObjLocator.Instance.LoadFurnitureItem(resName,layer,this.OnBuildingLoaded);
        }

        public void RotateFurniture()
        {
            if(null!= buildingFurniture)
            {
                //改为延迟执行判断碰撞
                buildingFurniture.RotateClockwise();
            }
        }

        //lua端设置位置
        public void LocFurniture(FurnitureItem fi, int dir, int index)
        {
            fi.Direction = dir;
            SetFurnitureIndex(fi,index);
        }
        
        public void CompleteFurniture()
        {
            if (CanPlaceFurniture(buildingFurniture))
            {
                colliderFurnitures.Clear();
                buildingFurniture.IsBuilding = false;
                SetBuildingState(true);
                buildingFurniture.PosIndex = placeMgr.GetPosIndex(buildingFurniture.GetPos());
                PlaceBuildItem(buildingFurniture);
                bool canCreateNext = CompletePlaced();
                if (isBuilingCreate&&canCreateNext)
                {
                    CreateNextFurniture();
                }
                else
                {
                    isBuilingCreate = false;
                    buildingFurniture = null;
                    lastFurniture = null;
                }
            }
        }
      
        #endregion
        #region CreateFurniture
        private void OnBuildingLoaded(FurnitureItem furnitureItem)
        {
            buildingFurniture = furnitureItem;
            ReadyForbuild();
            InitBuildingState();
            DoBuildingCheck();
        }

        private void ReadyForbuild()
        {
            if (null == buildingFurniture)
                return;
            int direction = buildingFurniture.Direction;
            int posIndex;
            //wall rotate
            if (buildingFurniture.TheInfo.furnitureType == FurnitureType.wall)
            {
                if(lastFurniture != null)
                {
                    FurnitureDirect wallDir;
                    posIndex = placeMgr.GetNearestWallIndex(lastFurniture, out wallDir);
                    direction = (int) wallDir;
                }
                else
                {
                    posIndex = placeMgr.GetDefaultIndex();
                }
            }
            else
            {
                posIndex = lastFurniture != null ? placeMgr.GetNearestIndex(lastFurniture.PosIndex)
                    : placeMgr.GetDefaultIndex();
            }

            buildingFurniture.Direction = direction;
            SetFurnitureIndex(buildingFurniture, posIndex);
        }
        private void CreateNextFurniture()
        {
            lastFurniture = buildingFurniture;
            buildingFurniture = null;
            FurnitureInfo info = lastFurniture.TheInfo;
            if (info.furnitureType == FurnitureType.ground || info.furnitureType == FurnitureType.wall)
            {
                BuildingFurniture(lastFurniture.res,lastFurniture.Layer);
            }
        }
        
        private bool CompletePlaced()
        {
            if(null == buildingFurniture)
                return false;
            if (onFurniturePlaced != null)
            {
                return onFurniturePlaced(buildingFurniture, buildingFurniture.Direction, buildingFurniture.PosIndex);
            }

            return false;
        }
        #endregion

        #region 修改建造

        public void DeleteSelectBuilding()
        {
            colliderFurnitures.Clear();
            if (selectFurniture == null || isBuilingCreate)
                return;
           
            isBuilingCreate = false;
            if(onBuildingDelete != null)
            {
                onBuildingDelete(selectFurniture.TheInfo.id);
            }
            placeMgr.RemovePlaceItem(selectFurniture);
            selectFurniture.Release();
            selectFurniture = null;
        }

        public void CancelBuilding()
        {
            colliderFurnitures.Clear();
            if (buildingFurniture == null)
                return;
            if (isBuilingCreate)
                buildingFurniture.Release();

            isBuilingCreate = false;
            buildingFurniture = null;
            lastFurniture = null;
        }

        public void SelectFurniture(FurnitureItem furnitureItem)
        {
            if(selectFurniture != null)
            {
                SetSelectBuildState(false, true);
            }
            selectFurniture = furnitureItem;
            isBuilingCreate = false;
            SetSelectBuildState(true,true);
        }

        /// <summary>
        /// 升级家具
        /// </summary>
        public void UpdateSelectFurniture(string resName, int layer)
        {
            if (selectFurniture == null) return;
            BuildingFurnitureUpdate(resName, layer);
        }

        /// <summary>
        /// 升级，直接替换
        /// </summary>
        /// <param name="resName"></param>
        /// <param name="layer"></param>
        public void BuildingFurnitureUpdate(string resName, int layer)
        {
            ObjLocator.Instance.LoadFurnitureItem(resName, layer, this.OnBuildingUpdate);
        }

        private void OnBuildingUpdate(FurnitureItem furnitureItem)
        {
            if (selectFurniture == null) return;

            //可以升级
            if (CanUpdateBuilding(furnitureItem))
            {
                //replace
                furnitureItem.TheInfo.id = selectFurniture.TheInfo.id;
                furnitureItem.Direction = selectFurniture.Direction;
                furnitureItem.TheInfo.furnitureType = selectFurniture.TheInfo.furnitureType;
                var posIndex = selectFurniture.PosIndex;
                SetFurnitureIndex(furnitureItem, posIndex);
                placeMgr.ReplacePlaceItem(selectFurniture, furnitureItem);
                sceneBuildings.Remove(selectFurniture);
                sceneBuildings.Add(furnitureItem);
                selectFurniture.Release();
                selectFurniture = null;
                furnitureItem.TheInfo.SetState(true, true);
            }
            else
            {
                furnitureItem.Release();
                furnitureItem = null;
            }
        }

        public void RecoverSelectFurniture()
        {
            if(selectFurniture != null)
            {
                SetSelectBuildState(false, true);
                selectFurniture = null;
            }
        }

        private void MoveFurniture(Vector3 hitPos)
        {
            int index = placeMgr.GetPosIndex(hitPos);
            switch (buildingFurniture.TheInfo.furnitureType)
            {
                case FurnitureType.wall:
                    FurnitureDirect tempDir = placeMgr.GetTargetWallDir(index, hitPos);
                    buildingFurniture.Direction = (int)tempDir;
                    break;
            }

            SetFurnitureIndex(buildingFurniture, index);
        }

        //放置Building，记录格子信息及场景现存列表中
        public void PlaceBuildItem(FurnitureItem furnitureItem)
        {
            placeMgr.PlaceItem(furnitureItem);
            if(furnitureItem.IsFurniture)
            {
                sceneFurnitures.Add(furnitureItem);
            }
            else
            {
                sceneBuildings.Add(furnitureItem);
            }
        }

        #endregion

        #region 状态检测

        public bool CanDeleteSelectBuilding()
        {
            if (selectFurniture == null) return false;
            return CanDeleteBuilding(selectFurniture);
        }

        private bool CanDeleteBuilding(FurnitureItem furnitureItem)
        {
            if (furnitureItem.TheInfo.furnitureType == FurnitureType.ground)
            {
                //有1以上的furniture则有家具
                return placeMgr.FurnitureCount(furnitureItem.PosIndex) <= 1;
            }
            return true;
        }

        private bool CanUpdateBuilding(FurnitureItem furnitureItem)
        {
            if (null == selectFurniture)
                return false;
            if (onBuildingUpdate != null)
            {
                return onBuildingUpdate(furnitureItem);
            }

            return false;
        }

        public void RefreshSceneFurnitures(bool isBuilding)
        {
            isSelectBuilding = isBuilding;
            selectFurniture = null;
            foreach (var furniture in sceneFurnitures)
            {
                furniture.TheInfo.SetState(true, !isBuilding);
            }

            foreach(var building in sceneBuildings)
            {
                building.TheInfo.SetState(true, isBuilding);
            }
        }

        public void RecoverSceneFurnitures()
        {
            foreach(var furniture in sceneFurnitures)
            {
                furniture.TheInfo.SetState(true, true);
            }

            foreach(var building in sceneBuildings)
            {
                building.TheInfo.SetState(true, true);
            }
        }

        private void SetBuildingState(bool isPlaced, bool canPlaced = true)
        {
            if (null != buildingFurniture)
            {
                buildingFurniture.TheInfo.SetState(isPlaced, canPlaced);
                Debug.Log("BuildingState:" + isPlaced + "CanPlaceD:" + canPlaced);
            }
        }

        /// <summary>
        /// 设置选中/上次选中家具状态
        /// </summary>
        /// isSelected: 是否选中，canPlaced: true为默认，false为置灰
        private void SetSelectBuildState(bool isSelected, bool canPlaced)
        {
            //暂时设为绿色
            if(null != selectFurniture)
            {
                selectFurniture.TheInfo.SetState(!isSelected, canPlaced);
            }
        }

        private void SetFurnitureIndex(FurnitureItem furniture, int index)
        {
            if (null != furniture)
            {
                furniture.PosIndex = index;
                furniture.SetPosAligned(placeMgr.GetIndexPos(index));
            }
        }

        private void DoBuildingCheck()
        {
            SetBuildingState(false, CanPlaceFurniture(buildingFurniture));
        }

        private void InitBuildingState()
        {
            buildingFurniture.IsBuilding = true;
            if (onBuildingStart != null)
            {
                onBuildingStart(buildingFurniture);
            }
        }
        #endregion
    }
}
local ViewMap = class("ViewMap",require("app.ui.UiView"))
ViewMap.res = "dlgmap"

local Const = Const
local _G = _G
local pairs = _G.pairs
local CfgData = _G.CfgData
local time = os.time
local date = os.date
local floor = math.floor
local random = math.random
local insert = table.insert
local bind = require("xlua.util").bind
local UpdateBeat = UpdateBeat
local Timer = Timer
local Global = Global
local StorageEvent = StorageEvent
local LuaUtility = CS.Game.LuaUtility
local getGlobalMap = Global.GetGlobalMap
local getSystem = Global.GetSystem
local hostSceneId
local ViewMapMoveUI
local sceneStageTimes
local host
local isMoving
local hostMoveTransRoot
local zombieMoveTransRoot

--===========================
---local function
--场景loc位置
local function getSceneTrans(sceneId)
    local sceneInfo = _G.CfgData:GetScene()
    local scene = sceneInfo[sceneId]
    local loc = _G.SceneEnv.loc
    return loc[scene.scencePicture]
end

local function canSceneOpen(stageId)
    local currentTime = getSystem():CurrentTime()
    for id,stageTime in pairs(sceneStageTimes) do
        if stageTime.stageId == stageId then
            local tempStartStr = stageTime.startDate
            local tempEndStr = stageTime.endDate
            local startTime = {year=tempStartStr[1],month=tempStartStr[2],day=tempStartStr[3],hour=tempStartStr[4],min=tempStartStr[5],sec=tempStartStr[6]}
            local endTime = {year=tempEndStr[1],month=tempEndStr[2],day=tempEndStr[3],hour=tempEndStr[4],min=tempEndStr[5],sec=tempEndStr[6]}
            if currentTime > time(startTime) and currentTime < time(endTime) then
                return true
            end
        end
    end
    return false
end

local function locPointEventPos(sceneItem,posIndex)
    --local trans = stageId.resId
    --1:模型， 2:位置
    LuaUtility.PlaceDropItem(_G.SceneEnv.mapPlaceManager, getSceneTrans(sceneItem.id), posIndex)
end

local function removePointEvent(sceneItem)
    LuaUtility.RemoveDropItem(_G.SceneEnv.mapPlaceManager,  getSceneTrans(sceneItem.id))
end

--获取游戏时间所在概率区间
local function getRandomIndex(gameTime, gameTimeRange)
    for index,time in pairs(gameTimeRange) do
        if gameTime < time then
            return index - 1
        end
    end
    return #gameTimeRange
end

--随机掉落点： randomLocations--cfg.locationRandom, posExistPosIndexs -- 已经存在的位置
local function getRandomPosIndex(randomLocations,posExistPosIndexs)
    local filterLocation = {}
    for _,location in ipairs(randomLocations) do
        if not posExistPosIndexs[location.indexId] then
            insert(filterLocation,location.indexId)
        end
    end

    print("<color=red>filterLocation::posExistPosIndexs</color>", valStr(filterLocation), valStr(posExistPosIndexs))

    local filterLength = #filterLocation
    if filterLength > 0 then
        local randomIndex = random(1,filterLength)
        --posExistPosIndexs[randomIndex]=true
        insert(posExistPosIndexs,randomIndex)
        return randomIndex
    end
end

--===========================
local ViewDropEventTime = class("ViewDropEventTime",require("app.ui.UiHandler"))

function ViewDropEventTime:Load(sceneItem, injector)
    injector:Inject(self)
    self.sceneItem = sceneItem
    self.stageId = sceneItem.stageId
    getGlobalMap():AddListenCountDownListener(self.stageId,self.DropTimeCountDown,self)
    getGlobalMap():AddFinishCountDownListener(self.stageId,self.FinishDropTime,self)
    getGlobalMap():StartPointEventCountDown(self.stageId)
    LuaUtility.Follow3DBy2DFollowTransDelta(self.text,getSceneTrans(sceneItem.id),0,6,0)
    LuaUtility.SetComponentEnabled(self.text, true)

end

function ViewDropEventTime:Release()
    getGlobalMap():StopPointEventCountDown(self.stageId)
end

function ViewDropEventTime:DropTimeCountDown(time)
    LuaUtility.TextSetTxt(self.text, date("%Mm%Ss", floor(time)))
end

function ViewDropEventTime:FinishDropTime()
    LuaUtility.SetComponentEnabled(self.text, false)
    removePointEvent(self.sceneItem)
end

--===========================

local ViewSceneItem = class("ViewSceneItem",require("app.ui.UiHandler"))

function ViewSceneItem:Load(scene,injector, lineManager, hostTransManager)
    self.scene = scene
    self.viewMngr = _G.ViewManager
    self.lineManager = lineManager
    self.hostTransManager = hostTransManager
    injector:Inject(self)
    LuaUtility.ImgSetSprite(self.icon,self.scene.icon2D,true)
    self:RegisterButtonClick(self.but,self.OnClick,scene.id)
end

function ViewSceneItem:Release()
    self:UnregisterButtonClick(self.but,self.scene.id)
    self.scene = nil
end

function ViewSceneItem:OnClick()
    ---draw line
    ---todo: pos use loc , need change
    if isMoving then
        return
    end

    if self.scene.id == hostSceneId and self.scene.stageId then
        SceneLoader:LoadStageScene(self.scene.id)
        getGlobalMap():SetInMap(false)
        return
    end
    self.viewMngr:OpenViewWith(ViewMapMoveUI, ViewMapMoveUI.Init, self.scene, self.hostTransManager)
end

--===========================
local ViewHostItem = class("ViewHostItem")
ViewHostItem.id = -1
function ViewHostItem:Load(injector)
    injector:Inject(self)
    LuaUtility.ImgSetSprite(self.icon,"sign_self",true)
end
--===========================
local ViewZombieMoveItem = class("ViewMoveItem")
ViewZombieMoveItem.id = -2
function ViewZombieMoveItem:Load(injector, iconId)
    injector:Inject(self)
    LuaUtility.ImgSetSprite(self.icon,iconId,true)
end
--===========================
local ViewHintItem = class("ViewHintItem", require("app.ui.UiHandler"))
ViewHintItem.id = -3

function ViewHintItem:ctor(dlg)
    self.dlg = dlg
end

function ViewHintItem:Load(injector, hintManager)
    self.hintManager = hintManager
    injector:Inject(self)

    LuaUtility.ImgSetSprite(self.icon, "base_self", true)
    self:RegisterButtonClick(self.but,self.OnFocusClick)
    self.updateHandle = UpdateBeat:RegisterListener(self.Update,self)
end

function ViewHintItem:OnFocusClick()
    --todo: temp use jiayuan location1
    local marker = _G.SceneEnv.marker
    local homeSceneData = _G.CfgData:GetScene(1)
    local homeTrans = marker[homeSceneData.scencePicture]
    _G.SceneEnv:CameraFocus(homeTrans)
end

function ViewHintItem:Remove()
    self:UnregisterButtonClick(self.but)
    UpdateBeat:RemoveListener(self.updateHandle)
    self.updateHandle = nil
    self.dlg = nil
end

function ViewHintItem:Update()
    --todo: temp use jiayuan location1
    local marker = _G.SceneEnv.marker
    local homeSceneData = CfgData:GetScene(1)
    local homeTrans = marker[homeSceneData.scencePicture]
    LuaUtility.HintManagerPositionHint(self.hintManager, self.icon, homeTrans)
end

--===========================
---Init
function ViewMap:OnOpen()
    isMoving = false
    host = HostPlayer
    ViewMapMoveUI = _G.ViewMapMoveUI
    hostMoveTransRoot = _G.SceneEnv.hostMoveTransRoot
    zombieMoveTransRoot = _G.SceneEnv.zombieMoveTransRoot
    hostSceneId = getGlobalMap():GetHostEnterScene()
    sceneStageTimes = CfgData:GetStageTime()
    local sceneInfo = CfgData:GetScene()
    --已有掉落位置
    self.pointEventPosIndex = getGlobalMap():GetPointEventPos()
    print("PointEventPosIndeX COunt:",#self.pointEventPosIndex)
    getGlobalMap():SetInMap(true)
    self:BindButtonEvents()
    self:InitBtnStates()
    self:InitSceneViewItems(sceneInfo)
    self:SetHostMoveEvent(sceneInfo)
    self:SetZombieMoveEvent()
    self:SetCameraFocusPos()
    self.updateHandle = UpdateBeat:RegisterListener(self.Update,self)
    end

function ViewMap:OnClose()
    host = nil
    UpdateBeat:RemoveListener(self.updateHandle)
    self.updateHandle = nil
    self:UnBindButtonEvents()
    self:ReleaseSceneViewItems()
    self:ReleaseZombieEvents()
    self:ReleaseHostEvents()
    getGlobalMap():SetLastTime()
    Global:FireEvent(StorageEvent.INST_SAVE)
end

function ViewMap:Update()
    if not isMoving or self.moveSpeed == nil or self.moveSpeed > Const.MapMoveSpeed.WALK then
        return
    end

    if self.currentBubbleTime > self.bubbleDelta then
        self.movebubble:SetActive(true)
        if self.currentDisappearTime > self.disappearDelta then
            self.currentBubbleTime = 0
            self.currentDisappearTime = 0
            self.movebubble:SetActive(false)
        end
        self.currentDisappearTime = self.currentDisappearTime + Timer.deltaTime
    else
        self.currentBubbleTime = self.currentBubbleTime + Timer.deltaTime
    end

end

function ViewMap:SetCameraFocusPos()
    if hostMoveTransRoot then
        _G.SceneEnv:CameraSetPos(hostMoveTransRoot)
    else
        if hostSceneId then
            _G.SceneEnv:CameraSetPos(getSceneTrans(hostSceneId))
        end
    end
end

--===========================
---hostEvent
function ViewMap:InitHostMoveTransRoot(isHostMoving)
    LuaUtility.NavTransManagerInit(self.hostTransManager, hostMoveTransRoot)
    if not isHostMoving then
        LuaUtility.ClonePosition(hostMoveTransRoot, getSceneTrans(hostSceneId))
    else
        local x,y,z = getGlobalMap():GetCurrentPos()
        LuaUtility.TransformSetPos(hostMoveTransRoot,x,y,z)
    end
end

function ViewMap:SetHostMovePath(targetSceneId)
    if targetSceneId then
        local arriveTarget = LuaUtility.NavTransManagerCalculatePath(self.hostTransManager,
                getSceneTrans(targetSceneId),getGlobalMap():GetPassTime(),getGlobalMap():GetMoveSpeed())
        if arriveTarget then
            isMoving = false
            local targetSceneId = getGlobalMap():GetTargetScene()
            hostSceneId = targetSceneId
            getGlobalMap():SetLastScene(hostSceneId)
            getGlobalMap():SetTargetScene(nil)
            getGlobalMap():SetMoveSpeed(nil)
            self:RefreshBtnStates()
        else
            self:MoveToTarget(targetSceneId,getGlobalMap():GetMoveSpeed() or Const.MapMoveSpeed.WALK)
        end
    end
end

function ViewMap:MoveToTarget(targetSceneId, speed)
    --if not isMoving then
    local sceneInfo = _G.CfgData:GetScene()
    self.targetScene = sceneInfo[targetSceneId]
    self.moveSpeed = speed
    local destTrans = getSceneTrans(targetSceneId)
    local x,y,z = LuaUtility.TransformGetPos(destTrans)
    --notify
    getGlobalMap():SetTargetScene(targetSceneId)
    getGlobalMap():SetMoveSpeed(speed)
    LuaUtility.NavTransManagerBindPosNotify(self.hostTransManager, function(x,y,z) getGlobalMap():SetCurrentPos(x,y,z) end)
    LuaUtility.LineManagerDrawLine(self.lineManager,getSceneTrans(hostSceneId),destTrans,Const.LineRenderType.HOST)
    LuaUtility.NavTransManagerTrans(self.hostTransManager, x, y, z, self.moveSpeed,
            function()
                LuaUtility.ActiveLine(self.lineManager, false,Const.LineRenderType.HOST)
                isMoving = false
                self.currentBubbleTime = 0
                self.currentDisappearTime = 0
                self.movebubble:SetActive(false)
                hostSceneId = targetSceneId
                getGlobalMap():SetLastScene(hostSceneId)
                getGlobalMap():SetTargetScene(nil)
                getGlobalMap():SetMoveSpeed(nil)
                LuaUtility.NavTransManagerBindPosNotify(self.hostTransManager,nil)
                self:RefreshBtnStates()
                self.moveSpeed = nil
            end)

    isMoving = true
    self:RefreshBtnStates()
    self.moveTargetDist = LuaUtility.NavTransManagerGetLength(self.hostTransManager, x, y, z)
    getGlobalMap():HostMoveTime(self.moveTargetDist * (1/self.moveSpeed))
    -- end
end

function ViewMap:SetHostMoveEvent(sceneInfo)
    local targetSceneId = getGlobalMap():GetTargetScene()
    self:InitHostMoveTransRoot(targetSceneId~=nil)
    self:SetHostMovePath(targetSceneId)
end

function ViewMap:ReleaseHostEvents()
    LuaUtility.NavTransManagerUnBindNotify(self.hostTransManager)
end

--===========================
---drop空投等固定点事件
function ViewMap:InitViewDropTime(sceneItem)
    if not self.timeViewItems then
        self.timeViewItems = {}
        self.timeViewIndex = 0
    end

    self.timeViewIndex = self.timeViewIndex + 1
    local injector = LuaUtility.BarManagerAddItem(self.time,self.timeViewIndex,getSceneTrans(hostSceneId))
    local item = ViewDropEventTime.new()
    item:Load(sceneItem,injector)
    insert(self.timeViewItems,item)
end

function ViewMap:SetOnePointEvent(sceneItem,randomPosIndex)
    locPointEventPos(sceneItem,randomPosIndex)
    self:InitViewDropTime(sceneItem)
end

function ViewMap:SetPointEvents(sceneItem)
    --判断是否存在
    local stageId = sceneItem.stageId
    --随机掉落位置及时间
    local pointEvent = getGlobalMap():GetPointEvent(stageId)
    --新档没有事件则直接创建事件
    if not pointEvent then
        --默认randomIndex为1
        local locationRandomType = CfgData:GetStage(stageId).locationRandom[1]
        local randomLocations = CfgData:GetRandomLocationIndex(locationRandomType)
        print("Create!!!<color=green> randomPosIndex:::</color>",randomPosIndex)
        print("Create!!!<color=green>randomLocations,pointEventPosIndex</color>",valStr(randomLocations),valStr(self.pointEventPosIndex))
        getGlobalMap():AddPointEvent(stageId,getSystem():CurrentTime(),0,nil,false,0)
        --self:SetOnePointEvent(sceneItem,randomPosIndex)
        return true
    else
        --判断是否时间锁住
        --若锁住判断是否主角在当前场景
        --在则直接显示，并返回
        --否则解锁
        local isTimeLock = pointEvent:GetTimeLock()
        if isTimeLock then
            if hostSceneId == sceneItem.id then
                self:SetOnePointEvent(sceneItem,getGlobalMap():PointEventGetPosIndex(stageId))
                return true
            else
                pointEvent:SetTimeLock(false)
                pointEvent:SetLastOpenTime(getSystem():CurrentTime())
                pointEvent:SetRandomTime(0)
            end
        end

        local currentTime = getSystem():CurrentTime()
        local pointEventCloseTime = getGlobalMap():PointEventGetCloseTime(stageId)
        ---玩家数据
        --当日已开启数
        local dayOpenNum = getGlobalMap():PointEventGetDayOpenNum(stageId)
        local stage = CfgData:GetStage(stageId)

        --todo:主角level,openNum为每日最大掉落数量,获取游戏时间,随机掉落概率
        --是否已经发生事件
        if getSystem():CurrentTime() - pointEventCloseTime < 0 then
            self:SetOnePointEvent(sceneItem,getGlobalMap():PointEventGetPosIndex(stageId))
            return true
        end
        --判断前置条件
        local passTime = currentTime - pointEventCloseTime
         if host.dataModel:GetLevel() < stage.openLevel or
                passTime < stage.inputCD or dayOpenNum >= stage.stageMax then
            return false
         end

        --触发几率
        local randomIndex = getRandomIndex(getSystem():GetGameTime(),stage.gameTime)
        if random(0,100) > stage.triggerRate[randomIndex] then
            return false
        end

        --初始化掉落参数
        local closeTimeRange = stage.closeTime[randomIndex]
        local randomCloseTime = random(closeTimeRange[1],closeTimeRange[2])
        local locationRandomType = stage.locationRandom[randomIndex]
        local randomLocations = CfgData:GetRandomLocationIndex(locationRandomType)
        local randomPosIndex = getRandomPosIndex(randomLocations,self.pointEventPosIndex)
        print("<color=red>randomPosIndex::</color>",randomPosIndex)
        print("EXSIST!!<color=green>randomLocations,pointEvents</color>",valStr(randomLocations),valStr(self.pointEventPosIndex))
        ---随机时间按分钟算 randomCloseTime*60
        pointEvent:Init(stageId,getSystem():CurrentTime(),randomCloseTime*60,randomPosIndex,false,dayOpenNum+1)
        self:SetOnePointEvent(sceneItem,randomPosIndex)
        return true
    end
        return false
end

--===========================
---路径类事件，目前只有尸潮
--尸潮袭家
function ViewMap:ZombieInHome()
    getGlobalMap():SetZombieInHomeTime(getSystem():CurrentTime())
    getGlobalMap():ZombieAttackTime(Const.ZombieInHomeTime)
end

--尸潮移动
function ViewMap:MoveZombie(time)
    LuaUtility.NavTransManagerEnableTrans(self.zombieTransManager,true)
    local from = getSceneTrans(9)
    local to = getSceneTrans(1)
    LuaUtility.LineManagerDrawLine(self.lineManager,from,to,Const.LineRenderType.ZOMBIE)
    LuaUtility.NavTransManagerMoveToTargetByTime(self.zombieTransManager,from,to,time,Const.ZombieMoveTime)
end

--移动监听事件
function ViewMap:ListenZombieMoveDelta(time)
    if not self.isZombieMoving then
        self.isZombieMoving = true
        self:MoveZombie(Const.ZombieMoveTime-time)
        LuaUtility.SetComponentEnabled(self.zombieTime, true)
        LuaUtility.Follow3DBy2DFollowTransDelta(self.zombieTime,zombieMoveTransRoot,0,6,0)
    end
    LuaUtility.TextSetTxt(self.zombieTime, date("%Mm%Ss", floor(time)))
end

function ViewMap:FinishZombieMove()
    self.isZombieMoving = false
    LuaUtility.ActiveLine(self.lineManager, false,Const.LineRenderType.ZOMBIE)
    LuaUtility.NavTransManagerEnableTrans(self.zombieTransManager,false)
    self:ReleaseMoveItem()
    self:ZombieInHome()
    getGlobalMap():SetZombieInHomeTime(getSystem():CurrentTime())
end

--袭家监听事件
function ViewMap:ListenZombieAttackDelta(time)
    if not self.isZombieInHome then
        self.isZombieInHome = true
        LuaUtility.SetComponentEnabled(self.zombieTime, true)
    end
    LuaUtility.TextSetTxt(self.zombieTime, date("%Mm%Ss", floor(time)))
end

function ViewMap:FinishZombieAttack()
    self.isZombieInHome = false
    LuaUtility.SetComponentEnabled(self.zombieTime, false)
end

function ViewMap:SetZombieMoveEvent()
    LuaUtility.NavTransManagerInit(self.zombieTransManager, zombieMoveTransRoot)
    self:InitViewZombieMoveItem()
    --first launch
    local lastZombieTime = getGlobalMap():GetLastZombieTime()
    if lastZombieTime == nil then
        lastZombieTime = getSystem():CurrentTime()
        getGlobalMap():SetLastZombieTime(lastZombieTime)
    end
    getGlobalMap():StartZombieCountDownTime()
    getGlobalMap():ZombieAddMoveTimeListener(self.ListenZombieMoveDelta,self)
    getGlobalMap():ZombieAddFinishMoveListener(self.FinishZombieMove,self)
    getGlobalMap():ZombieAddAttackTimeListener(self.ListenZombieAttackDelta,self)
    getGlobalMap():ZombieAddFinishAttackListener(self.FinishZombieAttack,self)
end

function ViewMap:ReleaseZombieEvents()
    self:ReleaseMoveItem()
    getGlobalMap():ZombieRemoveFinishMoveListener()
    getGlobalMap():ZombieRemoveMoveTimeListener()
    getGlobalMap():ZombieRemoveAttackTimeListener()
    getGlobalMap():ZombieRemoveFinishAttackListener()
    getGlobalMap():ReleaseZombieCountDownEvent()
    LuaUtility.NavTransManagerUnBindNotify(self.zombieTransManager)
end

--===========================
---Btn Events
function ViewMap:EnterButtonClick()
    SceneLoader:LoadStageScene(hostSceneId)
    getGlobalMap():SetInMap(false)
end

function ViewMap:SpeedButtonClick()
    LuaUtility.NavTransManagerTransSetSpeed(self.hostTransManager, Const.MapMoveSpeed.RUN)
    self.movebubble:SetActive(false)
    self.moveSpeed = Const.MapMoveSpeed.RUN
    getGlobalMap():SetMoveSpeed(self.moveSpeed)
    getGlobalMap():HostMoveTime(self.moveTargetDist * (1/self.moveSpeed))
    host.dataModel:CosumePower(1)
    self:RefreshBtnStates()
    self.backhome:SetActive(false)
end

function ViewMap:BackhomeButtonClick()
    if hostSceneId then
        self:MoveToTarget(hostSceneId, Const.MapMoveSpeed.RUN)
        self.backhome:SetActive(false)
        self.speed:SetActive(false)
        getGlobalMap():SetTargetScene(hostSceneId)
    end
end

function ViewMap:HideWarnBtns()
    self.enter:SetActive(false)
    self.speed:SetActive(false)
    self.backhome:SetActive(false)
end

function ViewMap:BindButtonEvents()
    self:RegisterButtonClick(self.enterBtn, self.EnterButtonClick)
    self:RegisterButtonClick(self.speedBtn, self.SpeedButtonClick)
    self:RegisterButtonClick(self.backhomeBtn, self.BackhomeButtonClick)
end

function ViewMap:UnBindButtonEvents()
    self:UnregisterButtonClick(self.enterBtn)
    self:UnregisterButtonClick(self.speedBtn)
    self:UnregisterButtonClick(self.backhomeBtn)
end

function ViewMap:InitBtnStates()
    self.speed:SetActive(false)
    self.backhome:SetActive(false)
    self.enter:SetActive(true)
    if hostSceneId then
        local x,y,z = LuaUtility.TransformGetPos(getSceneTrans(hostSceneId))
        LuaUtility.Follow3DBy2DFollowTrans(self.enterFollow,getSceneTrans(hostSceneId))
    end
end

function ViewMap:RefreshBtnStates()
    --enter
    if isMoving then
        self.enter:SetActive(false)
        self.speed:SetActive(host.dataModel.power>0 and self.moveSpeed<Const.MapMoveSpeed.RUN)
        self.speed:SetActive(self.moveSpeed<Const.MapMoveSpeed.RUN)
        self.backhome:SetActive(self.moveSpeed<Const.MapMoveSpeed.RUN)
        local loc = _G.SceneEnv.loc
        local destTrans = loc[self.targetScene.scencePicture]
        LuaUtility.Follow3DBy2DFollowTrans(self.hintlist, destTrans)
    else
        self.speed:SetActive(false)
        self.backhome:SetActive(false)
        self.enter:SetActive(true)
        local loc = _G.SceneEnv.loc
        local sceneInfo = _G.CfgData:GetScene()

        if hostSceneId then
            local scene = sceneInfo[hostSceneId]
            local locTrans = loc[scene.scencePicture]
            local x,y,z = LuaUtility.TransformGetPos(locTrans)
            LuaUtility.Follow3DBy2DFollowTrans(self.enterFollow,x, y, z)
        end
    end
end

--===========================
---场景3dUI
function ViewMap:InitSceneViewItems(sceneInfo)
    self:InitSceneItems(sceneInfo)
    self:InitHintItems(sceneInfo)
    self:InitSelfItem()
    self:InitMoveBubbleParam()
end

function ViewMap:ReleaseSceneViewItems()
    self:ReleaseSceneItems()
    self:ReleaseDropTimeItems()
    self:ReleaseHostItem()
    self:ReleaseHintItem()
    self:ReleaseMoveItem()
end

function ViewMap:InitMoveBubbleParam()
    self.currentBubbleTime = 0
    self.bubbleDelta = 4
    self.currentDisappearTime = 0
    self.disappearDelta = 2
    self.movebubble:SetActive(false)
    LuaUtility.Follow3DBy2DFollowTransDelta(self.movebubbleFollow, hostMoveTransRoot, -5, 10, 0)
end

function ViewMap:InitSelfItem()
    --offset
    local x,y,z = 0,6,0
    local injector = LuaUtility.BarManagerAddItemByOffset(self.bar,ViewHostItem.id,hostMoveTransRoot, x, y, z)
    self.hostViewItem = ViewHostItem.new()
    self.hostViewItem:Load(injector)
end

function ViewMap:InitViewZombieMoveItem(iconId)
    --判断是否移动，设置位置
    local x,y,z = 0,6,0
    local injector = LuaUtility.BarManagerAddItemByOffset(self.bar, ViewZombieMoveItem.id,zombieMoveTransRoot, x, y, z)
    self.zombieViewItem = ViewZombieMoveItem.new()
    self.zombieViewItem:Load(injector,"sign_enemy")
end

function ViewMap:InitSceneItems(sceneInfo)
    local marker = _G.SceneEnv.marker
    self.locItems = {}
    for id,scene in pairs(sceneInfo) do
        --空投
        if scene.openCondition == Const.SceneOpenCondition.DROP then
            if not self:SetPointEvents(scene) then
                goto continue
            end
        end
        --限时副本
        if scene.openCondition == Const.SceneOpenCondition.TIMESTAGE then
            if not canSceneOpen(scene.stageId) then
                LuaUtility.ComponentGameObjVisible(marker[scene.scencePicture].parent, false)
                goto continue
            end
        end

        if scene.scencePicture then
            local markTrans = marker[scene.scencePicture]
            if markTrans then
                local injector = LuaUtility.BarManagerAddItem(self.bar,id,markTrans)
                local item = ViewSceneItem.new()
                item:Load(scene,injector, self.lineManager, self.hostTransManager)
                self.locItems[id] = item
            end
        end
        ::continue::
    end
end

function ViewMap:InitHintItems(sceneInfo)
    local injector = LuaUtility.HintManagerAddHintItem(self.hintManager, ViewHintItem.id, self.hints)
    self.hintItem = ViewHintItem.new(self)
    self.hintItem:Load(injector, self.hintManager)
end

function ViewMap:ReleaseSceneItems()
    if self.locItems then
        for id,item in pairs(self.locItems) do
            item:Release()
            LuaUtility.BarManagerRemoveItem(self.bar,id)
        end
        self.locItems = nil
    end
end

function ViewMap:ReleaseDropTimeItems()
    if self.timeViewItems then
        for id,timeViewItem in pairs(self.timeViewItems) do
            timeViewItem:Release()
            LuaUtility.BarManagerRemoveItem(self.time,id)
        end
        self.timeViewItems = nil
    end
end

function ViewMap:ReleaseHostItem()
    if self.hostViewItem then
        LuaUtility.BarManagerRemoveItem(self.bar,ViewHostItem.id)
        self.hostViewItem = nil
    end
end

function ViewMap:ReleaseHintItem()
    if self.hintItem then
        self.hintItem:Remove()
        LuaUtility.HintManagerRemoveHintItem(self.hintManager, ViewHintItem.id)
        self.hintItem = nil
    end
end

function ViewMap:ReleaseMoveItem()
    if self.zombieViewItem then
        LuaUtility.BarManagerRemoveItem(self.bar, ViewZombieMoveItem.id)
        self.zombieViewItem = nil
    end
end

_G.ViewMap = ViewMap


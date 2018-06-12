local ViewMapMoveUI =class("ViewMapMoveUI",require("app.ui.UiView"))
ViewMapMoveUI.res = "dlgmoveui"
local _G = _G
local date = os.date
local floor = math.floor
local pairs = pairs
local getGlobalMap = Global.GetGlobalMap
local LuaUtility = CS.Game.LuaUtility
local MapMoveSpeed = Const.MapMoveSpeed
local getSystem = Global.GetSystem
local host

--===========================

function ViewMapMoveUI:Init(scene, navmeshManager)
    self.scene = scene
    self.sceneId = scene.id
    self.navmeshManager = navmeshManager
end

function ViewMapMoveUI:OnOpen()
    self.canRide = false
    self.moveRate = 1
    self.viewmap = ViewManager:GetView(ViewMap)
    --todo : compute time
    local loc = _G.SceneEnv.loc
    local destTrans = loc[self.scene.scencePicture]
    --local playerTrans = _G.SceneEnv.moveTrans
    --local dist = LuaUtility.CalculateTransformDist(playerTrans, destTrans)
    local x,y,z = LuaUtility.TransformGetPos(destTrans)
    local dist = LuaUtility.NavTransManagerGetLength(self.navmeshManager, x, y, z)
    local runTime = dist * (1/MapMoveSpeed.RUN)
    local walkTime = dist * (1/(MapMoveSpeed.WALK * self.moveRate))
    host = HostPlayer
    LuaUtility.TextSetTxt(self.runtime, date("%Mm:%Ss", floor(runTime)))
    --compute walk speed
    local moverateInfo = _G.CfgData:GetMoveRate()
    local walkNum = host.dataModel.walkNum
    for _,rate in pairs(moverateInfo) do
        if (walkNum <= rate.numUp or rate.numUp == -1) and dist < rate.lengthUp then
            self.moveRate = rate.moveRate
            break
        end
    end

    LuaUtility.TextSetTxt(self.walktime, date("%Mm:%Ss", floor(walkTime)))
    LuaUtility.TextSetTxt(self.runenergy, host.dataModel:GetPower())
    --触发副本存在时间
    local stageCloseTime = getGlobalMap():PointEventGetCloseTime(self.scene.stageId)
    local stageExistTime
    if stageCloseTime then
        stageExistTime = stageCloseTime - getSystem():CurrentTime()
    end

    print("IF stageExistTime Exist:!!!",stageExistTime)
    self.isTriggerStage = (stageExistTime ~= nil)
    --有体力且触发副本到达时间小于存在时间
    print("<color=red>ReachTime,runTime,walkTime:::</color>",self.scene.stageId,stageExistTime,runTime,walkTime)
    local runCanReach = (host.dataModel:GetPower()>0) and (not stageExistTime or stageExistTime>runTime)
    local walkCanReach = not stageExistTime or stageExistTime>walkTime
    LuaUtility.SetButtonInteractable(self.run, runCanReach)
    LuaUtility.SetButtonInteractable(self.walk, walkCanReach)
    LuaUtility.SetButtonInteractable(self.ride,self.canRide)
    self:RegisterButtonClick(self.back,self.OnBackClick)
    self:RegisterButtonClick(self.ride,self.OnRideClick)
    self:RegisterButtonClick(self.run,self.OnRunClick)
    self:RegisterButtonClick(self.walk, self.OnWalkClick)
end

function ViewMapMoveUI:OnClose()
    self:UnregisterButtonClick(self.back)
    self:UnregisterButtonClick(self.ride)
    self:UnregisterButtonClick(self.run)
    self:UnregisterButtonClick(self.walk)
end

function ViewMapMoveUI:OnBackClick()
    self:Close()
end

function ViewMapMoveUI:OnRideClick()
    self.viewmap:MoveToTarget(self.sceneId, MapMoveSpeed.RIDE)
    self:Close()
end

function ViewMapMoveUI:OnRunClick()
    --todo: run energy consume
    host.dataModel:CosumePower(1)
    --触发副本，上时间锁
    if self.isTriggerStage then
        getGlobalMap():SetTimeLock(self.scene.stageId,true)
    end
    self.viewmap:MoveToTarget(self.sceneId, MapMoveSpeed.RUN)
    self:Close()
end


function ViewMapMoveUI:OnWalkClick()
    self.viewmap:MoveToTarget(self.sceneId, MapMoveSpeed.WALK * self.moveRate)
    if self.isTriggerStage then
        getGlobalMap():SetTimeLock(self.scene.stageId,true)
    end
    host.dataModel:AddWalkNum(1)
    self:Close()
end

_G.ViewMapMoveUI = ViewMapMoveUI

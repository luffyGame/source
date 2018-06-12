---
--- Created by wangliang.
--- DateTime: 2018/5/18 下午12:10
--[[大地图存储数据，包括
   1.玩家数据
   2.大地图上发生的事件:
        b.固定点事件：空难、突然的掉落啥的
        c.尸潮：带路径显示
]]--
local HostMapData = require("app.module.globalmap.HostMapData")
local PathMapData = require("app.module.globalmap.PathMapData")
local PointMapData = require("app.module.globalmap.PointMapData")

local tostring,tonumber = tostring,tonumber
local getSystem = Global.GetSystem
local insert = table.insert

local GlobalMap = {}
extendMethod(GlobalMap,require("app.base.Savable"))

function GlobalMap:Create()
    self.host = HostMapData.new()
    self.pathEvent = PathMapData.new()
    self.pointEvents = {}
    self.pointEventTags = {}
end

function GlobalMap:Import(data)
    self.pointEvents = {}
    self.pointEventTags = {}
    --host
    self.host = HostMapData.new()
    if data.host then
        self.host:Import(data.host)
    end
    --event
    self.pathEvent = PathMapData.new()
    if data.event then
        self.pathEvent:Import(data.event)
    end

    if data.pointEvents then
        for _,event in pairs(data.pointEvents) do
            local eventData = PointMapData.new()
            eventData:Import(event)
            self.pointEvents[tonumber(eventData.tid)] = eventData
            eventData:BindChangeNotify(self.PointEventDataChange,self)
        end
    end
    self.lastPointEvents = data.pointEvents
end

function GlobalMap:Export(modified)
    local data = self.lastSave or {}
    local modData
    local host,hostMod = self.host:Export(modified)
    data.host = host
    if hostMod then
        if not modData then modData={} end
        modData[host] = hostMod
    end

    local event,eventMod = self.pathEvent:Export(modified)
    data.event = event
    if eventMod then
        if not modData then modData={} end
        modData[event]=eventMod
    end

    --pointEvents
    local pointEventDatas = self.lastPointEvents
    if not pointEventDatas then
        pointEventDatas = {}
    end

    local pointEventMod
    for tid,changed in pairs(self.pointEventTags) do
        if changed then
            self.pointEventTags[tid] = false
            local pointEvent = self.pointEvents[tid]
            local pointEventData,mod = pointEvent:Export(modified)
            local tidStr = tostring(tid)
            pointEventDatas[tidStr] = pointEventData
            if mod then
                if not pointEventMod then pointEventMod = {} end
                pointEventMod[tidStr] = mod
            end
        end
    end

    data.pointEvents = pointEventDatas
    if pointEventMod then
        if not modData then modData={} end
        modData[pointEventMod] = pointEventMod
    end

    return data,modData
end

function GlobalMap:PostLoadData()
    self.host:BindChangeNotify(self.MarkDirty,self)
    self.pathEvent:BindChangeNotify(self.MarkDirty,self)
    --self.event:PostImport()
end

function GlobalMap:GetHostEnterScene()
    return self.host:GetEnterScene()
end

--================================================
--掉落等固定点事件

function GlobalMap:AddPointEvent(tid,lastOpenTime,randomTime,posIndex,timeLock,dayOpenNum)
    self:MarkDirty()
    local PointEventData = PointMapData.new()
    PointEventData:Init(tid,lastOpenTime,randomTime,posIndex,timeLock,dayOpenNum)
    self.pointEvents[tid] = PointEventData
    self.pointEventTags[tid] = true
end

function GlobalMap:PointEventDataChange(PointEventData)
    local tid = PointEventData.tid
    if tid then
        self:MarkDirty()
        self.pointEventTags[tonumber(tid)] = true
    end
end

--事件监听
function GlobalMap:StartPointEventCountDown(tid)
    self.pointEvents[tid]:StartPointEventCountDown()
end

function GlobalMap:StopPointEventCountDown(tid)
    self.pointEvents[tid]:StopPointEventCountDown()
end


function GlobalMap:AddListenCountDownListener(tid,listener,owner)
    self.pointEvents[tid]:AddListenCountDownListener(listener,owner)
end

function GlobalMap:AddFinishCountDownListener(tid,listener,owner)
    self.pointEvents[tid]:AddFinishCountDownListener(listener,owner)
end

function GlobalMap:GetPointEvent(tid)
    print("<color=red>pointEventsTID!!!:</color>",valStr(self.pointEvents[tid]))

    return self.pointEvents[tid]
end

function GlobalMap:PointEventGetDayOpenNum(tid)
    return self.pointEvents[tid]:GetDayOpenNum(tid)
end

function GlobalMap:PointEventGetPosIndex(tid)
    return self.pointEvents[tid].posIndex
end

function GlobalMap:PointEventGetCloseTime(tid)
    if self.pointEvents and self.pointEvents[tid] and
            self.pointEvents[tid].lastOpenTime and self.pointEvents[tid].randomTime then
        return self.pointEvents[tid].lastOpenTime + self.pointEvents[tid].randomTime
    end
end

function GlobalMap:GetPointEvents()
    return self.pointEvents
end

function GlobalMap:GetPointEventPos()
    local posIndexs = {}
    for _,event in pairs(self.pointEvents) do
        if event.posIndex then
            posIndexs[event.posIndex] = true
        end
    end
    return posIndexs
end

function GlobalMap:SetTimeLock(tid,timeLock)
    self.pointEvents[tid]:SetTimeLock(timeLock)
end
--================================================

function GlobalMap:StartZombieCountDownTime()
    self.pathEvent:StartZombieCountDownTime()
end

function GlobalMap:ZombieAttackTime(totalSeconds)
    self.pathEvent:ZombieAttackTime(totalSeconds)
end

function GlobalMap:ZombieAddAttackTimeListener(listener,owner)
    self.pathEvent:AddAttackTimeListener(listener,owner)
end

function GlobalMap:ZombieRemoveAttackTimeListener()
    self.pathEvent:RemoveAttackTimeListener()
end

function GlobalMap:ZombieAddFinishAttackListener(listener,owner)
    self.pathEvent:AddFinishAttackListener(listener,owner)
end

function GlobalMap:ZombieRemoveFinishAttackListener()
    self.pathEvent:RemoveFinishAttackListener()
end

function GlobalMap:ZombieMoveTime(totalSeconds)
    self.pathEvent:ZombieMoveTime(totalSeconds)
end

function GlobalMap:ZombieAddMoveTimeListener(listener,owner)
    self.pathEvent:AddMoveTimeListener(listener,owner)
end

function GlobalMap:ZombieAddFinishMoveListener(listener,owner)
    self.pathEvent:AddFinishMoveListener(listener,owner)
end

function GlobalMap:ZombieRemoveMoveTimeListener()
    self.pathEvent:RemoveMoveTimeListener()
end

function GlobalMap:ZombieRemoveFinishMoveListener()
    self.pathEvent:RemoveFinishMoveListener()
end

function GlobalMap:HostMoveTime(totalSeconds)
    self.host:HostMoveTime(totalSeconds)
end

function GlobalMap:RemoveHostMoveTime()
    self.host:RemoveMoveTime()
end

function GlobalMap:AddMoveTimeListener(listener,owner)
    self.host:AddMoveTimeListener(listener,owner)
end

function GlobalMap:AddFinishMoveListener(listener,owner)
    self.host:AddFinishMoveListener(listener,owner)
end

function GlobalMap:RemoveMoveTimeListener()
    self.host:RemoveMoveTimeListener()
end

function GlobalMap:RemoveFinishMoveListener()
    self.host:RemoveFinishMoveListener()
end

--================================================

function GlobalMap:SetLastScene(sceneId)
    self.host:SetLastScene(sceneId)
end

function GlobalMap:SetLastStage(stageId)
    self.host:SetLastStage(stageId)
end

function GlobalMap:SetTargetScene(sceneId)
    self.host:SetTargetScene(sceneId)
end

function GlobalMap:SetTargetStage(stageId)
    --self.host:SetTargetScene(stageId)
end

function GlobalMap:SetCurrentPos(x,y,z)
    self.host:SetCurrentPos(x,y,z)
end

function GlobalMap:SetMoveSpeed(moveSpeed)
    self.host:SetMoveSpeed(moveSpeed)
end

function GlobalMap:SetLastTime()
    self.host:SetLastTime()
end

function GlobalMap:SetInMap(isInMap)
    self.host:SetInMap(isInMap)
end
--================================================

function GlobalMap:GetLastScene()
    return self.host:GetLastScene()
end

function GlobalMap:GetLastStage()
    return self.host:GetLastStage()
end

function GlobalMap:GetTargetScene()
    return self.host:GetTargetScene()
end

function GlobalMap:GetTargetStage()
    return self.host:GetTargetStage()
end

function GlobalMap:GetCurrentPos()
    return self.host:GetCurrentPos()
end

function GlobalMap:GetMoveSpeed()
    return self.host:GetMoveSpeed()
end

function GlobalMap:GetLastTime()
    return self.host:GetLastTime()
end

function GlobalMap:GetPassTime()
    return getSystem():CurrentTime() - self.host:GetLastTime()
end

function GlobalMap:GetInMap()
    return self.host:GetInMap()
end

--================================================

function GlobalMap:SetLastZombieTime(lastZombieTime)
    self.pathEvent:SetLastZombieTime(lastZombieTime)
end

function GlobalMap:SetZombieInHomeTime(inHomeTime)
    self.pathEvent:SetZombieInHomeTime(inHomeTime)
end


function GlobalMap:GetLastZombieTime()
    return self.pathEvent:GetLastZombieTime()
end

function GlobalMap:GetZombieInHomeTime()
    return self.pathEvent:GetZombieInHomeTime()
end

function GlobalMap:RemoveZombieAttackTime()
    self.pathEvent:RemoveZombieAttackTime()
end

function GlobalMap:RemoveZombieMoveTime()
    self.pathEvent:RemoveZombieMoveTime()
end

function GlobalMap:ReleaseZombieCountDownEvent()
    self.pathEvent:RemoveZombieMoveTime()
    self.pathEvent:RemoveZombieAttackTime()
end
--================================================

_G.GlobalMap = GlobalMap
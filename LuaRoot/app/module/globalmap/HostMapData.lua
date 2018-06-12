---
--- Created by wangliang.
--- DateTime: 2018/5/18 下午12:23
---
--1.当前所处场景、关卡、
--2.玩家跑路：路径起点终点，路径当前位置，移动速度

local HostMapData = class("HostMapData",require("app.base.DataModel"))
local hostmap_saveFields = {"stayInMap","lastScene","lastStage","targetScene","targetStage","lastTime","moveSpeed","posX","posY","posZ"}

local CfgData = CfgData
local getSystem = Global.GetSystem
local Timer = Timer

function HostMapData:MarkSave()
    self:MarkFieldSave(hostmap_saveFields)
end

function HostMapData:PostImport()

end

function HostMapData:GetEnterScene()
    if self.lastScene then
        return self.lastScene
    else
        return CfgData:GetSystemParam().birthScence
    end
end

function HostMapData:Export(modified)
    local data,mod = HostMapData.super.Export(self,modified)
    return data,mod
end


function HostMapData:HostMoveTime(totalSeconds)
    if self.timer then
        self.timer:Stop()
        self.timer = nil
    end
    self.totalSeconds = totalSeconds
    self.timer = Timer.Once(totalSeconds,self.FinishHostMove,self):RegisterCd(self.ListenMoveDelta,self):Start()
end

function HostMapData:RemoveMoveTime()
    if self.timer then
        self.timer:Stop()
        self.timer = nil
    end
end

function HostMapData:FinishHostMove()
    self.totalSeconds=nil
    if self.finishMoveListener then
        self.finishMoveListener(self.finishMoveOwner)
    end
end

function HostMapData:ListenMoveDelta(time)
    if self.moveTimeListener then
        self.moveTimeListener(self.moveTimeListenerOwner, self.totalSeconds-time)
    end
end
function HostMapData:AddFinishMoveListener(listener,owner)
    self.finishMoveOwner = owner
    self.finishMoveListener = listener
end

function HostMapData:RemoveFinishMoveListener()
    self.finishMoveListener = nil
end

function HostMapData:AddMoveTimeListener(listener,owner)
    self.moveTimeListenerOwner = owner
    self.moveTimeListener = listener
end

function HostMapData:RemoveMoveTimeListener()
    self.moveTimeListener = nil
end

--================================================

function HostMapData:SetLastScene(sceneId)
    self:SetValue("lastScene",sceneId,true)
end

function HostMapData:SetLastStage(stageId)
    self:SetValue("lastStage",stageId,true)
end

function HostMapData:SetTargetScene(sceneId)
    self:SetValue("targetScene",sceneId,true)
end

function HostMapData:SetTargetStage(stageId)
    self:SetValue("targetStage",stageId,true)
end

function HostMapData:SetCurrentPos(x,y,z)
    self:SetValue("posX",x,true)
    self:SetValue("posY",y,true)
    self:SetValue("posZ",z,true)
end

function HostMapData:SetMoveSpeed(speed)
    self:SetValue("moveSpeed",speed,true)
end

function HostMapData:SetLastTime()
    self:SetValue("lastTime",getSystem():CurrentTime(),true)
end

function HostMapData:SetInMap(inMap)
    self:SetValue("stayInMap",inMap,true)
end

--================================================

function HostMapData:GetLastScene()
    return self.lastScene
end

function HostMapData:GetLastStage()
    return self.lastStage
end

function HostMapData:GetTargetScene()
    return self.targetScene
end

function HostMapData:GetTargetStage()
    return self.targetStage
end

function HostMapData:GetCurrentPos()
    return self.posX,self.posY,self.posZ
end

function HostMapData:GetMoveSpeed()
    return self.moveSpeed
end

function HostMapData:GetLastTime()
    return self.lastTime
end

function HostMapData:GetInMap(inMap)
    return self.stayInMap
end

return HostMapData
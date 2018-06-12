---
--- Description 空气墙
--- Created by SunShubin.
--- DateTime: 2018/6/05 11:31 AM
---
local SceneItem_AirWall = class("SceneItem_Ceiling",require("app.object.entity.SceneItem"))
extendMethod(SceneItem_AirWall,require("app.object.datamodel.extends.TriggerConditionExtend"))
local EventTrigger = _G.EventTrigger

local listenEnter
local listenExit

function SceneItem_AirWall:EnterStage()
    self:Load()
    if self:GetTriggerConditions() then
        local triggerID = self:GetTriggerConditions()[1].triggerParam
        listenEnter = EventTrigger.PlayerEnter..triggerID
        listenExit = EventTrigger.PlayerExit..triggerID
        EventTrigger:AddEventListener(listenEnter,self.OnPlayerEnter,self)
        EventTrigger:AddEventListener(listenExit,self.OnPlayerExit,self)
    end

end

function SceneItem_AirWall:OnPlayerEnter()
    self.view:ColliderEnable(false)
end

function SceneItem_AirWall:OnPlayerExit()
    self.view:ColliderEnable(true)
end

function SceneItem_AirWall:Release()
    SceneItem_AirWall.super.Release(self)
    if self:GetTriggerConditions() then
        EventTrigger:RemoveEventListener(listenEnter,self.OnPlayerEnter,self)
        EventTrigger:RemoveEventListener(listenExit,self.OnPlayerExit,self)
    end
end

function SceneItem_AirWall:IsUseSelectable()
    return false
end

return SceneItem_AirWall



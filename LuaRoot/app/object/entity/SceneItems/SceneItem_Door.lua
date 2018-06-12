---
--- Description 门
--- Created by SunShubin.
--- DateTime: 2018/6/02 11:31 AM
---
local SceneItem_Door = class("SceneItem_Door",require("app.object.entity.SceneItem"))
extendMethod(SceneItem_Door,require("app.object.datamodel.extends.TriggerConditionExtend"))
local EventTrigger = _G.EventTrigger
local listenEnter
local listenExit
function SceneItem_Door:EnterStage()
    self:Load()
    if self:GetTriggerConditions() then
        local triggerID = self:GetTriggerConditions()[1].triggerParam
        listenEnter = EventTrigger.PlayerEnter..triggerID
        listenExit = EventTrigger.PlayerExit..triggerID
        EventTrigger:AddEventListener(listenEnter,self.OnPlayerEnter,self)
        EventTrigger:AddEventListener(listenExit,self.OnPlayerExit,self)
        self.canUse = false
    else
        self.canUse = true
    end
end

function SceneItem_Door:OnLoaded()
    SceneItem_Door.super.OnLoaded(self)
    self.view:SetUsable(self:IsUseSelectable())
end

function SceneItem_Door:OnPlayerEnter()
    self.canUse = true
end

function SceneItem_Door:OnPlayerExit()
    self.canUse = false
end

function SceneItem_Door:Release()
    SceneItem_Door.super.Release(self)
    if self:GetTriggerConditions() then
        EventTrigger:RemoveEventListener(listenEnter,self.OnPlayerEnter,self)
        EventTrigger:RemoveEventListener(listenExit,self.OnPlayerExit,self)
    end
end

function SceneItem_Door:IsUseSelectable()
    return true
end

function SceneItem_Door:OnAction(target)
    print("onAction",self.canUse)
    if self.canUse then
        self:OnDeadBy(target)
        self:Dead()
    else

    end
end

function SceneItem_Door:GetActType()
    return _G.Const.PlayerCmdType.OPENDOOR
end

function SceneItem_Door:Dead()
    SceneItem_Door.super.Dead(self)
    if self.view then
        self.view:PlayDefaultAnim()
    end
    self:FireExecuteEvents()
end

return SceneItem_Door

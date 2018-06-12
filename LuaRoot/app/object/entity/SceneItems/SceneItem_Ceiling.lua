---
--- Description 天花板
--- Created by SunShubin.
--- DateTime: 2018/6/02 11:31 AM
---
local SceneItem_Ceiling = class("SceneItem_Ceiling",require("app.object.entity.SceneItem"))
extendMethod(SceneItem_Ceiling,require("app.object.datamodel.extends.TriggerConditionExtend"))
local EventTrigger = _G.EventTrigger
local listenMsg

function SceneItem_Ceiling:EnterStage()
    self:Load()

    if self:GetTriggerConditions() then
        local triggerID = self:GetTriggerConditions()[1].triggerParam
        listenMsg = EventTrigger.AreaOpen..triggerID
        EventTrigger:AddEventListener(listenMsg,self.OnAreaOpen,self)
    end
end

function SceneItem_Ceiling:OnAreaOpen()
    self.view:SetVisible(false)
end

function SceneItem_Ceiling:Release()
    SceneItem_Ceiling.super.Release(self)
    if self:GetTriggerConditions() then
        EventTrigger:RemoveEventListener(listenMsg,self.OnAreaOpen,self)
    end
end

function SceneItem_Ceiling:IsUseSelectable()
    return false
end

return SceneItem_Ceiling



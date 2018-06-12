---
--- Description 管道
--- Created by SunShubin.
--- DateTime: 2018/5/31 10:16 AM
---
local EventTrigger = _G.EventTrigger
local SceneItem_Pipeline = class("SceneItem_Pipeline",require("app.object.entity.SceneItem"))
extendMethod(SceneItem_Pipeline,require("app.object.datamodel.extends.TriggerConditionExtend"))
local listenMsg
function SceneItem_Pipeline:EnterStage()
    self:Load()
    if self:GetTriggerConditions() then
        local triggerID = self:GetTriggerConditions()[1].triggerParam
        listenMsg = EventTrigger.PlayerExit..triggerID
        EventTrigger:AddEventListener(listenMsg,self.ListenPlayerExit,self)
    end
end

function SceneItem_Pipeline:Release()
    EventTrigger:RemoveEventListener(listenMsg,self.ListenPlayerExit,self)
    SceneItem_Pipeline.super.Release(self)
    self.target = nil
end

function SceneItem_Pipeline:OnAction(target)
    self.target = target
    target:SetConstraintDir(self:GetDir(),true)
    target:SetPos(self:GetPos())
    self:FireExecuteEvents()
end

function SceneItem_Pipeline:ListenPlayerExit()
    if self.target then
        self.target:CancelConstraint()
        self.target = nil
    end
end


function SceneItem_Pipeline:IsUseSelectable()
    return self.dataModel:IsUseSelectable()
end

function SceneItem_Pipeline:GetActType()
    return _G.Const.PlayerCmdType.OPERATEITEM
end

return SceneItem_Pipeline

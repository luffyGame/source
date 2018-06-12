---
--- Description 遥控的物体
--- Created by SunShubin.
--- DateTime: 2018/5/31 10:16 AM
---
local SceneItem_RemoteCtrl = class("SceneItem_RemoteCtrl",require("app.object.entity.SceneItem"))
extendMethod(SceneItem_RemoteCtrl,require("app.object.datamodel.extends.TriggerConditionExtend"))
local EventTrigger = _G.EventTrigger
local listenMsg
function SceneItem_RemoteCtrl:EnterStage()
    self:Load()
    if self:GetTriggerConditions() then
        local triggerID = self:GetTriggerConditions()[1].triggerParam
        listenMsg = EventTrigger.OperateItem..triggerID
        EventTrigger:AddEventListener(listenMsg,self.OnOperateItem,self)
    end
end

function SceneItem_RemoteCtrl:OnOperateItem()
    self:Dead()
end

function SceneItem_RemoteCtrl:Release()
    SceneItem_RemoteCtrl.super.Release(self)
    if self:GetTriggerConditions() then
        EventTrigger:RemoveEventListener(listenMsg,self.OnOperateItem,self)
    end
end

function SceneItem_RemoteCtrl:OnAction(target)
    self.target = target
end

function SceneItem_RemoteCtrl:IsUseSelectable()
    return self.dataModel:IsUseSelectable()
end

function SceneItem_RemoteCtrl:GetActType()
    return _G.Const.PlayerCmdType.OPERATEITEM
end

function SceneItem_RemoteCtrl:Dead()
    SceneItem_RemoteCtrl.super.Dead(self)
    if self.view then
        self.view:PlayDefaultAnim()
    end
end


return SceneItem_RemoteCtrl

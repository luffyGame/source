---
--- Description 传送带
--- Created by SunShubin.
--- DateTime: 2018/6/04 11:31 AM
---
local SceneItem_ConveyorBelt = class("SceneItem_ConveyorBelt",require("app.object.entity.SceneItem"))

extendMethod(SceneItem_ConveyorBelt,require("app.object.datamodel.extends.TriggerConditionExtend"))
local EventTrigger = _G.EventTrigger
local listenMsg
function SceneItem_ConveyorBelt:EnterStage()
    self:Load()
    if self:GetTriggerConditions() then
        local triggerID = self:GetTriggerConditions()[1].triggerParam
        listenMsg = EventTrigger.OperateItem..triggerID
        EventTrigger:AddEventListener(listenMsg,self.OnOperateItem,self)
    end
end

function SceneItem_ConveyorBelt:OnLoaded()
    SceneItem_ConveyorBelt.super.OnLoaded(self)
    local param = self.dataModel:GetParam()
    local sceneItem = _G.SceneItemManger:AddItemByTid(param[1])
    sceneItem.dataModel:SetParent(self.view:GetMount(0))
end

function SceneItem_ConveyorBelt:Release()
    SceneItem_ConveyorBelt.super.Release(self)
    if self:GetTriggerConditions() then
        EventTrigger:RemoveEventListener(listenMsg,self.OnOperateItem,self)
    end
end

function SceneItem_ConveyorBelt:IsUseSelectable()
    return false
end


function SceneItem_ConveyorBelt:OnAction(target)
    self:OnDeadBy(target)
    self:Dead()
end

function SceneItem_ConveyorBelt:OnOperateItem()
    self:Dead()
end

function SceneItem_ConveyorBelt:Dead()
    SceneItem_ConveyorBelt.super.Dead(self)
    if self.view then
        self.view:PlayDefaultAnim()
    end
    self:FireExecuteEvents()
end

return SceneItem_ConveyorBelt

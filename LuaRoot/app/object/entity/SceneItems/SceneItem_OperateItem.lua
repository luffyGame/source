---
--- Description 操作物体（比如电脑）
--- Created by SunShubin.
--- DateTime: 2018/5/31 10:16 AM
---

local SceneItem_OperateItem = class("SceneItem_Pipeline",require("app.object.entity.SceneItem"))
extendMethod(SceneItem_OperateItem,require("app.object.datamodel.extends.TriggerConditionExtend"))

function SceneItem_OperateItem:EnterStage()
    self:Load()
end

function SceneItem_OperateItem:Release()
    SceneItem_OperateItem.super.Release(self)
    self.target = nil
end

function SceneItem_OperateItem:OnAction(target)
    self.target = target
    self:OnDeadBy(target)
    self:Dead()
    self:FireExecuteEvents()
end

function SceneItem_OperateItem:IsUseSelectable()
    return self.dataModel:IsUseSelectable()
end

function SceneItem_OperateItem:GetActType()
    return _G.Const.PlayerCmdType.OPERATEITEM
end


function SceneItem_OperateItem:Dead()
    print("dead dead dead")
    SceneItem_OperateItem.super.Dead(self)
end


return SceneItem_OperateItem

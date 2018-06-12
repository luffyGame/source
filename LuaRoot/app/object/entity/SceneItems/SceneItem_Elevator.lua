---
--- Description 电梯
--- Created by SunShubin.
--- DateTime: 2018/5/31 10:16 AM
---
local SceneItem_Elevator = class("SceneItem_Elevator",require("app.object.entity.SceneItem"))
local GlobalMap = Global.GetGlobalMap
local ViewManager,ElevatorView = ViewManager,ElevatorView

function SceneItem_Elevator:EnterStage()
    self:Load()
end

function SceneItem_Elevator:Release()
    self.target = nil
end

function SceneItem_Elevator:OnAction(target)
    self.target = target
    local stageId = GlobalMap():GetLastStage()
    local param = self.dataModel:GetParam()
    ViewManager:OpenViewWith(ElevatorView,ElevatorView.Init,stageId,param)
end

function SceneItem_Elevator:GetActType()
    return _G.Const.PlayerCmdType.OPERATEITEM
end

function SceneItem_Elevator:IsUseSelectable()
    return self.dataModel:IsUseSelectable()
end

return SceneItem_Elevator

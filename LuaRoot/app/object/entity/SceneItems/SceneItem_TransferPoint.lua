---
--- Description 传送点
--- Created by SunShubin.
--- DateTime: 2018/5/31 10:16 AM
---
local SceneItem_TransferPoint = class("SceneItem_TransferPoint",require("app.object.entity.SceneItem"))

function SceneItem_TransferPoint:EnterStage()
    self:Load()
end

function SceneItem_TransferPoint:Release()
    self.target = nil
end

function SceneItem_TransferPoint:OnAction(target)
    self.target = target
    local param = self.dataModel:GetParam()
    Global.GetSceneLoader():LoadStageByStageId(param[1],true)
end

function SceneItem_TransferPoint:IsUseSelectable()
    return self.dataModel:IsUseSelectable()
end

function SceneItem_TransferPoint:GetActType()
    return _G.Const.PlayerCmdType.OPERATEITEM
end


return SceneItem_TransferPoint

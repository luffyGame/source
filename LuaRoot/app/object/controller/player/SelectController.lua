local SelectController = class("SelectController")

local PlayerSelector = require("app.object.controller.selector.PlayerSelector")
local UseSelector,AtkSelector = PlayerSelector.UseSelector,PlayerSelector.AtkSelector
local CfgData = CfgData

function SelectController:ctor(user)
    self.useSelector = UseSelector.new(user,CfgData:GetSystemParam().findItemDetection)
    self.atkSelector = AtkSelector.new(user, CfgData:GetSettingBattle().findEnemyDetection)
end

function SelectController:EnterScene()
    self.useSelector:EnterScene()
    self.atkSelector:EnterScene()
end

function SelectController:Release()
    self:EnableTrigger(false)
    self.useSelector:Release()
    self.atkSelector:Release()
end

function SelectController:EnableTrigger(enable)
    if self.useSelector then
        self.useSelector:EnableTrigger(enable)
    end
end

function SelectController:OnUpdate(deltaTime)
    --self.useSelector:SelectNearest()
    self.atkSelector:SelectNearest()
    self.atkSelector:UpdateMarkPos()
end

function SelectController:GetUseTarget()
    return self.useSelector.target
end

function SelectController:GetAtkTarget()
    return self.atkSelector.target
end

return SelectController
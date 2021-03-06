---
--- Generated by EmmyLua(https://github.com/EmmyLua)
--- Created by wangliang.
--- DateTime: 2018/3/22 下午3:04
---

local ComposedCmd = require("app.object.controller.player.PlayerComposeCmd")
local CmdUse,CmdAttack = ComposedCmd.CmdUse,ComposedCmd.CmdAttack

local PlayerActiveController = class("PlayerActiveController",Commander)

function PlayerActiveController:Use(target)
    if self:IsUse(target) then return end
    local can,skill = CmdUse.CanUse(self.owner,target)
    if can then
        self.owner:ViewWeapon(skill and skill.weapon)
        self:ExecCmd(CmdUse.new(target,skill))
    end
end

function PlayerActiveController:IsUse(target)
    return self.currentCmd and self.currentCmd.class == CmdUse and self.currentCmd.target == target
end

function PlayerActiveController:Attack(target)
    if self:IsAttack(target) then return end
    local can,skill = CmdAttack.CanAttack(self.owner)
    if can then
        self.owner:ResetViewWeapon()
        self:ExecCmd(CmdAttack.new(target,skill))
    end
end

function PlayerActiveController:IsAttack(target)
    return self.currentCmd and self.currentCmd.class == CmdAttack and self.currentCmd.target == target
end

function PlayerActiveController:OnFollowEnd()
    if self.currentCmd and self.currentCmd.useFollow then
        self.currentCmd:OnFollowEnd()
    end
end

function PlayerActiveController:Cancel()
    self.owner:ResetViewWeapon()
    self:Attacking(false)
    PlayerActiveController.super.Cancel(self)
end

function PlayerActiveController:OnUpdate(deltaTime)
    PlayerActiveController.super.OnUpdate(self,deltaTime)
    if self.isAttacking and not self.currentCmd then
        self.owner:Atk()
    end
end

function PlayerActiveController:Attacking(isAttacking)
    self.isAttacking = isAttacking
    if isAttacking then
        self.owner:Atk()
    end
end

return PlayerActiveController


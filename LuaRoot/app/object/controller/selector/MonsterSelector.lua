---
----- Description 怪物目标选择
----- Created by SunShubin.
----- DateTime: 2018/4/13 下午5:49
-----

local Selector = require("app.object.controller.selector.Selector")
local MonsterSelector = class("TargetSelector", Selector)
local Camp = _G.Const.Camp

local linkedAoi
function MonsterSelector:ctor(user)
    MonsterSelector.super.ctor(self, user, _G.Const.MonsterSearchRadius)
    if not linkedAoi then
        linkedAoi = LinkedAoi
    end
end

function MonsterSelector:SetCamp(camp)
    self.camp = camp
end

function MonsterSelector:IsSelectable(entity)
    if entity.isItem or entity:IsDead() or (entity.isMonster and not entity:IsAtkSelectable()) or (entity.IsCrawl and entity:IsCrawl()) then
        return false
    end

    local camp = entity:GetCamp()
    if not Camp:IsEnemy(self.camp, camp) then
        return false
    end

    if self.user:IsTargetInRange(entity) then
        return true
    end
    return false
end

function MonsterSelector:OnTargetSelect()
    self.user:OnTargetSelect(self.target)
end

return MonsterSelector
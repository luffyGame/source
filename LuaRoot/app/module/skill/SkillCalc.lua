---
--- Generated by EmmyLua(https://github.com/EmmyLua)
--- Created by wangliang.
--- DateTime: 2018/5/5 下午5:54
---
--[[
A对B造成伤害=Max【（a*A攻击力-b*B防御力），1】*（A攻击技能系数）*Max平【（1+A伤害增加系数-B伤害减免系数），0】+真实伤害
命中公式：
A对B命中判定=A命中率-B闪避率
暴击判定：
A对B暴击判定=A暴击率
偏斜判定：
A对B偏斜判定=B偏斜率

生产伤害：
树木被伐木斧砍，每次伤害=伐木斧生产伤害
伤害判定
按照技能类型
近战技能：
优先级：命中/闪避》暴击》伤害计算
远程技能：
优先级：偏斜》伤害计算

命中/闪避：miss=0伤害
暴击：固定3倍伤害
偏斜：仅对远程技能影响，偏斜后造成伤害降低40%

]]
--[[
--计算体calc包含的数据
    user,userData,
    originT:初始目标，有可能是entity，也有可能是位置
    pivot:范围锚点，根据定义和originT产生
    reached:是否到达预期位置
    pos:技能被阻挡后的伤害发生位置,弹道飞行的终点
    fromPos:弹道飞行的起始位置
    userAtk:攻击方攻击力
    targetCount:目标数量
]]
local max,floor = math.max,math.floor
local pairs = _G.pairs
local Chance10K = Math.Chance10K
local ViewObject = require("app.object.view.ViewObject")
local LuaUtility = CS.Game.LuaUtility
local CRIT_MUL,MISS_MUL,SNEAK_MUL = 3,0.4,3
local Buff = require("app.module.skill.Buff")
local SkillArea = Const.SkillArea
local AssetType = Const.AssetType
---=======================
local SkillCalc = class("SkillCalc")

function SkillCalc:ctor(skill,user,originTarget,isSneak)
    self.ref = 1
    self.skill = skill
    self.isNear = skill:IsNear()
    self.isProduce = skill.isProduce
    self.user = user
    self.userData = user.dataModel
    self.originT = originTarget
    self.targetCount = 0
    self.isSneak = isSneak
    self.isSkillPoint = skill:GetAreaType() == SkillArea.POINT
end

function SkillCalc:Ref()
    self.ref = self.ref + 1
    return self
end

function SkillCalc:Release()
    self.ref = self.ref -1
    if self.ref == 0 then
        self:ReleaseWarn()
        self.skill = nil
        self.user = nil
        self.userData = nil
        self.target = nil
        self.originT = nil
        self.pivot = nil
    end
end

---计算施法者的攻击值
function SkillCalc:CalcUserAtk()
    local weapon = self.skill.weapon
    local weaponDmg = weapon and weapon:GetDmg(self.isProduce) or 0
    if self.skill.isProduce then
        self.userAtk = weaponDmg*self.skill:GetParam()
    else
        self.userAtk = self.userData:GetAtk(weaponDmg)
    end
end

function SkillCalc:AddTarget(entity)
    if self.targetCount == 0 then
        self.target = entity
    elseif self.targetCount == 1 then
        local one = self.target
        if one == entity then return end
        self.target = {}
        self.target[one.objId] = one
        self.target[entity.objId] = entity
    else
        if self.target[entity.objId] then return end
        self.target[entity.objId] = entity
    end
    self.targetCount = self.targetCount + 1
end

---计算所有目标的闪避、暴击信息
function SkillCalc:CalcTargetAtked()
    if self.targetCount == 0 then return end
    if self.targetCount == 1 then
        self.atkedInfo = self:CalcOneTargetAtked(self.target)
    else
        self.atkedInfo = {}
        for oid,target in pairs(self.target) do
            self.atkedInfo[oid] = self:CalcOneTargetAtked(target)
        end
    end
end

function SkillCalc:CalcOneTargetAtked(target)
    local targetData = target.dataModel
    local atked = {}
    if self.isNear then
        atked.hitted = Chance10K(self.userData:GetAgl()-targetData:GetDex())
        if atked.hitted then atked.crit = Chance10K(self.userData:GetCrit()) end
    else
        atked.missed = Chance10K(targetData:GetMiss())
    end
    return atked
end

function SkillCalc:IsTargetInvalid(target)
    return not target or target.dataModel.dead
end

function SkillCalc:AtkOneTarget(target,atked)
    if not atked or self:IsTargetInvalid(target) then return end
    if self.isNear and not atked.hitted then return end
    local hitEffect = self.skill:GetHitEffect()
    if hitEffect then
        local epoint = self.skill:GetHitPart() + 1
        target:PlayEffect(hitEffect,epoint,false)
    end
    local dmg = self:GetAtkDmg(target.dataModel)
    if atked.crit and not self.isProduce then
        dmg = dmg * CRIT_MUL
    end
    if atked.missed then
        dmg = dmg * MISS_MUL
    end

    local mult = self:GetSneakMul(target,self.isSneak,self.skill)
    dmg = dmg * mult

    target:HurtBy(dmg,self,self.skill)
end

--破隐一击-伤害系数
function SkillCalc:GetSneakMul(target,isSneak,skill)
    if isSneak and target.HaveHate and not target:HaveHate(self.user) and skill:IsSneakCrit() then
        return SNEAK_MUL
    end
    return 1
end

function SkillCalc:DoAtk(isLastHit)
    if isLastHit then
        self:OnLastHit(self.targetCount ~= 0)
    end
    if self.targetCount == 0 then return end
    if self.targetCount == 1 then
        self:AtkOneTarget(self.target,self.atkedInfo)
    else
        for oid,target in pairs(self.target) do
            self:AtkOneTarget(target,self.atkedInfo[oid])
        end
    end
end

function SkillCalc:DoBuff()
    self:BuffTarget()
    self:BuffUser()
end

function SkillCalc:BuffTarget()
    if self.targetCount == 0 then return end
    local targetBuff = self.skill:GetTargetBuff()
    if not targetBuff then return end
    local theTarget = self.skill:BuffOnlyMain() and self.originT or nil
    if self.targetCount == 1 then
        self:BuffOneTarget(targetBuff,self.target,self.atkedInfo,theTarget)
    else
        for oid,target in pairs(self.target) do
            self:BuffOneTarget(targetBuff,target,self.atkedInfo[oid],theTarget)
        end
    end
end

function SkillCalc:BuffOneTarget(buffId,target,atked,mainTarget)
    if mainTarget and target ~= mainTarget then return end
    if not atked or self:IsTargetInvalid(target) then return end
    if self.isNear and not atked.hitted then return end
    local buff = Buff.new(buffId,self.user.objId)
    buff:TryAttach(target)
end

function SkillCalc:BuffUser()
    local selfBuff = self.skill:GetSelfBuff()
    if not selfBuff then return end
    local buff = Buff.new(self.buffId,self.user.objId)
    buff:TryAttach(self.user)
end

function SkillCalc:GetAtkDmg(targetData)
    if self.isProduce then
        return self.userAtk
    end
    local a,b = 1,1
    local targetDef = targetData:GetDef()
    local atk = max(a*self.userAtk-b*targetDef,1)*self.skill:GetParam()
    local dmgMod = max(1 + (self.userData:GetDmgInc()-targetData:GetDmgDec())/10000,0)
    local dmg = atk*dmgMod + self.userData:GetAtkTrue()
    return floor(dmg)
end

function SkillCalc:ShowWarn(res,scale,rot)
    self.warnMark = ViewObject.new()
    self.warnScale = scale
    self.warnMark:DoLoad(res,LuaUtility.LoadBasicModel,self.OnWarnMarkLoaded,self,AssetType.MODEL_DUMMY)
end

function SkillCalc:GetPivotPos()
    if not self.pivot then return end
    if not self.pivotPos then
        local pos = self.pivot
        if pos.isEntity then
            pos = pos:GetPos(true)
        else
            pos = pos:clone()
        end
        pos.y = pos.y + 0.1
        self.pivotPos = pos
    end
    return self.pivotPos
end

function SkillCalc:OnWarnMarkLoaded()
    if not self.pivot then return end
    local pos = self:GetPivotPos()
    self.warnMark:SetPos(pos)
    self.warnMark:SetDir(self.user:GetDir())
    self.warnMark:SetScale(self.warnScale)
end

function SkillCalc:ReleaseWarn()
    if self.warnMark then
        self.warnMark:Release()
        self.warnMark = nil
    end
end

function SkillCalc:GetHurtDir(target)
    local pos = target:GetPos()
    local hurtFrom
    if self.isSkillPoint then
        hurtFrom = self.user:GetPos()
    else
        hurtFrom = self:GetPivotPos()
    end
    local dir = pos - hurtFrom
    dir.y = 0
    return dir:setNormalize()
end
--最后一下
function SkillCalc:OnLastHit(hitTar)
    if self.user.isPlayer then
        if self.skill.weapon and hitTar then
            self.user:SetNoiseDis(self.skill.weapon:GetNoiseRange())
        end

        if self.isSneak then
            self.user:SetSneak(false)
        end
    end
end

return SkillCalc
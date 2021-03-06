---
--- Generated by EmmyLua(https://github.com/EmmyLua)
--- Created by wangliang.
--- DateTime: 2018/4/10 上午11:14
---

local _G = _G
local max,floor = math.max,math.floor
local EffectPoint = Const.EffectPoint

local SkillCalc = require("app.module.skill.SkillCalc")
local SkillTargetSelector = require("app.module.skill.SkillTargetSelector")

local SkillImpl = class("SkillImpl")

function SkillImpl:ctor(skill)
    self.skill = skill
    self.isProduce = skill.isProduce
    self.selector = SkillTargetSelector.new(skill)
end

function SkillImpl:Perform(user,target,isSneak)
    local action = self.skill:GetPerformAct()
    user:ActSkill(action)
    self:InitCalc(user,target,isSneak)
    self:Start()
end

function SkillImpl:PlayCastEffect()
    local effectRes = self.skill:GetPerformEffect()
    if not effectRes then return end
    local effectPoint = self.skill:GetPerformEffectPoint() or EffectPoint.POS
    local user = self.calc.user
    if self.skill.weapon then
        user:PlayWeaponEffect(effectRes,effectPoint,true)
    else
        user:PlayEffect(effectRes,effectPoint,true)
    end
end

function SkillImpl:Start()
    self.isPlaying = true
    self.isPerform = true
    self.isCanceled = false
    self.performTime = self.skill:GetUncontrolTime()
    self.castEffectTime = self.skill:GetPerformEffectTime()
    self.hitTimes = self.skill:GetHitTime()
    self.hitCount = self.hitTimes and #self.hitTimes or 0
    self.nextHitIndex = 1
    self.time = 0
    self:SetIsCd(true)
    self:OnStart()
end

function SkillImpl:OnStart()
    local calc = self.calc

    if calc.originT and calc.originT ~= calc.user then
        local pos = calc.originT
        if pos.isEntity then pos = pos:GetPos() end
        self.calc.user:LookAt(pos)
    end

    self.selector:OnStart(calc)
end

function SkillImpl:Update(deltaTime)
    if not self.isPlaying then return true end
    self.time = self.time + deltaTime
    if self.isCd and self.time > self.skill:GetCd() then
        self:SetIsCd(false)
        if not self.isPerform then self.isPlaying = false end
    end
    if self.castEffectTime and self.time>= self.castEffectTime then
        self.castEffectTime = nil
        self:PlayCastEffect()
    end
    if self.isPerform then
        self:DoPerform()
        if self.time >= self.performTime then
            self.isPerform = false
            if not self.isCd then self.isPlaying = false end
            self:OnPerformEnd()
        end
    end
end

function SkillImpl:DoPerform()
    if self.isCanceled then return end
    if self.nextHitIndex > self.hitCount then return end
    if self.time >= self.hitTimes[self.nextHitIndex] then
        if self.nextHitIndex == 1 then
            self:OnFirstHit()
        end
        self:CheckCost()
        self:PlayHit(self.nextHitIndex == self.hitCount)
        self.nextHitIndex = self.nextHitIndex + 1
    end
end

function SkillImpl:OnFirstHit()
    self.selector:OnFirstHit(self.calc)
end

function SkillImpl:PlayHit(isLastHit) end

function SkillImpl:CheckCost()
    if self.nextHitIndex == self.hitCount then
        return true
    end
end

function SkillImpl:InitCalc(user,target,isSneak)
    local calc = SkillCalc.new(self.skill,user,target,isSneak)
    calc:CalcUserAtk(self.skill)
    self.calc = calc
end

function SkillImpl:CalcTarget(calc)
    calc = calc or self.calc
    if not calc then return end
    self.selector:CalcTarget(calc)
    calc:CalcTargetAtked()
end

function SkillImpl:Do(isLastHit,calc)
    calc = calc or self.calc
    if calc then
        if not self.skill:OnlyBuff() then
            calc:DoAtk(isLastHit)
        end
        if isLastHit then
            calc:DoBuff()
        end
    end
end

function SkillImpl:OnPerformEnd()
    self.isCanceled = false
    self.calc:Release()
    self.calc = nil
end

function SkillImpl:SetIsCd(isCd)
    self.isCd = isCd
    local weapon = self.skill.weapon
    if weapon then
        weapon.isCd = isCd
    end
end

function SkillImpl:IsCd()
    return self.isCd
end

function SkillImpl:CanPlay()
    return not self.isPlaying
end

function SkillImpl:Cancel()
    self.isCanceled = true
end

local NearSkillImpl = class("NearSkillImpl",SkillImpl)
---在施法时刻计算目标
function NearSkillImpl:OnFirstHit()
    NearSkillImpl.super.OnFirstHit(self)

    self:CalcTarget()
end

function NearSkillImpl:PlayHit(isLastHit)
    self:Do(isLastHit)
end


local FarSkillImpl = class("FarSkillImpl",SkillImpl)
---1.指定目标或者非弹道技能在开始时刻计算
local EFlyType = {
    RAY = 1,
}

function FarSkillImpl:OnFirstHit()
    FarSkillImpl.super.OnFirstHit(self)
    if not self.skill:IsBallistic() then
        self:CalcTarget()
    end
    self:CalcFlyPos()
end

function FarSkillImpl:CalcFlyPos()
    local calc = self.calc
    if not calc.fromPos then
        calc.fromPos = self:GetFlyFrom()
        if not calc.originT then
            calc.pos = calc.user:GetSkillReachPos(self.skill:GetRange())
        end
    end
end

function FarSkillImpl:GetFlyFrom()
    if self.skill.weapon then
        return self.calc.user:GetWeaponFirePos()
    end
    return self.calc.user:GetPos()
end

function FarSkillImpl:PlayHit(isLastHit)
    local flyType = self.skill:GetFlyType()
    if not flyType then
        self:Do(isLastHit)
    elseif flyType == EFlyType.RAY then
        local ballistic = _G.BallisticManager:AddBallistic(self.skill)
        ballistic:Fly(isLastHit)
    end
end

return {NearSkillImpl,FarSkillImpl}
---
--- Description Monster战斗
--- Created by 孙树斌
--- DateTime: 2018/4/26 上午10:03
---
local MonsterFightExtend = class("MonsterFightExtend")

local ipairs = ipairs
local random = math.random

function MonsterFightExtend:MonsterFightInit()
    self.posIndex = 1
    self.hateList = {}
    self.hateTargets = {}
    self.lastActT = 0
    self.lastAtkT = 0
    self.skillCountdown = false
    self.skillArray = {}
    self:InitNormalAtk()
    self:PrepareSkills()
end

function MonsterFightExtend:MonsterFightUpdate(deltaTime)
    self.lastActT = self.lastActT + deltaTime
    if self.skillCountdown and self.preskill then
        for i, v in ipairs(self.preskill) do
            v.preCD = v.preCD - deltaTime
            if v.preCD <= 0 then
                self.skillArray[i] = v.skillID
                self.preskill[i] = nil
            end
        end
        if #self.preskill == 0 then
            self.preskill = nil
        end
    end
    if self.lastAtkT > 0 then
        self.lastAtkT = self.lastAtkT - deltaTime
    end
end

--初始化普通攻击
function MonsterFightExtend:InitNormalAtk()
    self.normalAtk = {}
    if self.cfg.normalAtk then
        for i, v in ipairs(self.cfg.normalAtk) do
            self.normalAtk[i] = v
        end
    end
end
--准备技能
local skillNames = {'skill1','skill2','skill3','skill4','skill5',}
function MonsterFightExtend:PrepareSkills()
    self.preskill = {}
    for i = 1, #skillNames do
        local tempSkillCfg = self.cfg[skillNames[i]]
        if tempSkillCfg then
            self.preskill[i] = {}
            self.preskill[i].skillID = tempSkillCfg[1]
            self.preskill[i].preCD = tempSkillCfg[2]
        else
            break
        end
    end
end

--得到技能，普通攻击也算技能
function MonsterFightExtend:GetCurSkill()
    local tempSkill = nil
    for i = 1, #self.skillArray do
        tempSkill = self:GetSkill(self.skillArray[i])
        if tempSkill:CanPerform() then
            break
        else
            tempSkill = nil
        end
    end

    if not tempSkill and #self.normalAtk > 0 then
        local random = random(1,#self.normalAtk)
        tempSkill = self:GetSkill(self.normalAtk[random])
    end
    return tempSkill
end

--0.永久存在，直至场景刷新 1.一段时间后清除 2.离开出生点一段距离后清除 3.见到目标就跑
function MonsterFightExtend:GetHateType()
    return self.cfg.hateType
end
--时间清除仇恨
function MonsterFightExtend:IsTimeClear()
    return self:GetHateType() == 1
end
--距离清除仇恨
function MonsterFightExtend:IsDisClear()
    return self:GetHateType() == 2
end
--见到目标就跑
function MonsterFightExtend:IsCoward()
    return self:GetHateType() == 3
end

--仇恨数值 当仇恨Type为1的时候，返回的是时间，Type为2的时候，返回的是距离
function MonsterFightExtend:GetHateNum()
    return self.cfg.hatNum
end
--得到下次攻击时间
function MonsterFightExtend:SetNextAtkTime()
    if self.cfg.atkSpeedDown == self.cfg.atkSpeedUp then
        self.lastAtkT = self.cfg.atkSpeedDown
    else
        self.lastAtkT = random(self.cfg.atkSpeedUp * 100,self.cfg.atkSpeedDown * 100) / 100
    end
end
--可以攻击
function MonsterFightExtend:CanAtk()
    if self.lastAtkT <= 0 then
        return true
    end
    return false
end

--被选择的优先级
function MonsterFightExtend:GetChoosePriorty()
    return self.cfg.aimChosePriority
end
--重置上次行动的时间
function MonsterFightExtend:ResetLastActTime()
    self.lastActT = 0
end

function MonsterFightExtend:StartCountDown()
    self.skillCountdown = true
end

function MonsterFightExtend:GetLastActTime()
    return self.lastActT
end

--添加仇恨成员
function MonsterFightExtend:AddHate(target)
    if not self.dataModel.hateList[target.objId] then
        self.dataModel.hateList[target.objId] = target
    end
    if target.isMonster then
        target.dataModel.hateTargets[self.objId] = self
    end
end

function MonsterFightExtend:RemoveHate(target)
    self.dataModel.hateList[target.objId] = nil
    self:GetHateTarget()
end

function MonsterFightExtend:HaveHate(target)
    if self.dataModel.hateList[target.objId] then
        return true
    end
    return false
end

function MonsterFightExtend:NotifyDead()
    for i, v in pairs(self.dataModel.hateTargets) do
        self.dataModel.hateTargets[i]:RemoveHate(self)
    end
end

--清空仇恨列表
function MonsterFightExtend:ClearHate()
    self.dataModel.hateList = {}
end


return MonsterFightExtend
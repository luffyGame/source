local Monster = class("Monster", require("app.object.entity.Object"))
extendMethod(Monster, require("app.object.entity.extends.MoverExtend"))
extendMethod(Monster, require("app.object.entity.extends.FighterExtend"))
extendMethod(Monster, require("app.object.datamodel.extends.MonsterFightExtend"))
extendMethod(Monster, require("app.object.datamodel.extends.TriggerConditionExtend"))

Monster.isMonster = true
Monster.sightAngle = 30

local MonsterDataModel = require("app.object.datamodel.actor.MonsterDataModel")

local ViewSprite = require("app.object.view.ViewSprite")

local MonsterActor = require("app.object.controller.monster.MonsterActor")
local MonsterSelector = require("app.object.controller.selector.MonsterSelector")

--region math
local sin = math.sin
local cos = math.cos
local rad = math.rad
local pairs = pairs
local random = math.random
local Timer = Timer
local Vector3 = Vector3
--endregion
local PopHpType = Const.PopHpType
local SawPart = Const.SawPart

local MonsterActiveController = require("app.object.controller.monster.MonsterActiveController")
local MonsterAI = require("app.object.controller.monster.MonsterAI")
local MonsterAIStates = require("app.object.controller.monster.MonsterAIStates")
local EventTrigger = _G.EventTrigger

local listenMsg

function Monster:Born(tid)
    self.dataModel = MonsterDataModel.new()
    self.dataModel:Init(tid)
end

function Monster:SetLocInfo(locIndex, locGroup)
    self.dataModel:SetLocInfo(locIndex, locGroup)
end

function Monster:GetLocInfo()
    return self.dataModel.locIndex, self.dataModel.locGroup
end

function Monster:CacheLoc(loc)
    self.dataModel.loc = loc
end

function Monster:Load()
    if self.view then
        return
    end
    self.view = ViewSprite.new(self)
    self.actor = MonsterActor.new(self)
    self.view:Load(self.dataModel:GetModel(), self.OnLoaded, self)
end

function Monster:Release()
    EventTrigger:RemoveEventListener(EventTrigger.Trigger, self.EventTrigger, self)

    if self:GetTriggerConditions() then
        EventTrigger:RemoveEventListener(listenMsg, self.OnAreaOpen, self)
    end

    self.AI = nil
    self:EnableUpdate(false)
    self:ReleaseMover()
    Monster.super.Release(self)
end

function Monster:OnLoaded()
    self.view:SetObjInfo(self:GetId())
    Monster.super.OnLoaded(self)
    if self.dataModel.dead then
        self:PlayDead()
    end

end

function Monster:Export(modified)
    if self.dataModel then
        return self.dataModel:Export(modified)
    end
end

function Monster:Import(data)
    self.dataModel = MonsterDataModel.new()
    self.dataModel:Import(data)
end

function Monster:EnterStage()
    local scale = self.dataModel:GetCfgScale()
    self:SetScale(Vector3.new(scale, scale, scale))
    if not self.dataModel.dead then
        --region AI
        self.selector = MonsterSelector.new(self)
        self.controller = MonsterActiveController.new(self)
        self.selector:SetCamp(self.dataModel:GetCamp())
        self.AI = MonsterAI.new(self)
        local r = random(-30, 30)
        self.randomAngle = rad(r)
        self.followPos = Vector3.zero()

        EventTrigger:AddMonster(self.dataModel:GetGroup())
        EventTrigger:AddEventListener(EventTrigger.Trigger, self.EventTrigger, self)

        if self:GetTriggerConditions() then
            self.awake = false
            local triggerID = self:GetTriggerConditions()[1].triggerParam
            listenMsg = EventTrigger.AreaOpen .. triggerID
            EventTrigger:AddEventListener(listenMsg, self.OnAreaOpen, self)
        else
            self.awake = true
        end
        --endregion
    end
    self:Load()
end

function Monster:OnAreaOpen()
    if self.AI then
        self.AI:OnAreaOpen()
    end
end

function Monster:EventTrigger(type, param)
    if self.AI then
        self.AI:EventTrigger(type, param)
    end

end

function Monster:Ready()
    self:EnableUpdate(true)
    if self.AI then
        self.AI:Start()
    end
end

function Monster:StartFollow()
    self:StartFollowPos(self:GetTargetPos())
end

function Monster:OnFollowCancel()

end

function Monster:OnFollowEnd()
    if self.AI then
        self.AI:MoveEnd()
    end
    if self.controller then
        self.controller:OnFollowEnd()
    end

end

function Monster:Idle(time, callback)
    self.controller:Idle(time, callback)
end

function Monster:MoveToPos(pos)
    self:StartFollowPos(pos)
end

function Monster:Move(pos, callback)
    self.controller:Move(pos, callback)
end

function Monster:Update()
    local deltaTime = Timer.deltaTime
    if self.actor then
        self.actor:OnUpdate(deltaTime)
    end
    self.dataModel:OnUpdate(deltaTime)
    if self.AI then
        self.AI:OnUpdate(deltaTime)
    end
    if self.controller then
        self.controller:OnUpdate(deltaTime)
    end
end

function Monster:IsAtkSelectable()
    if not self.awake then
        return false
    end
    return self.dataModel:IsAtkSelectable()
end

function Monster:IsUseSelectable()
    return self.dataModel:IsUseSelectable()
end

function Monster:GetUseIcon()
    return self.dataModel:GetUseIcon()
end

function Monster:GetRadarIcon()
    return self.dataModel:GetRadarIcon()
end

function Monster:GetBox()
    return self.dataModel.box
end

function Monster:OnBoxClose()
    self.dataModel:OnBoxClose(self.view:SetUsable(self.dataModel:IsUseSelectable()))
end

function Monster:PlayDmgPop(dmg)
    self:PopHpDmg(PopHpType.MONSTER_DMG, dmg)
end

function Monster:OnHurtBy(user)
    self.lastHurt = user
    self:AddHate(user)
    self.dataModel:ResetLastActTime()
    if self.AI then
        self.AI:HurtBy(user)
    end
end

function Monster:Atk()
    if self.dataModel:CanAtk() then
        self.dataModel:ResetLastActTime()
        self.curSkill = self.dataModel:GetCurSkill()
        if self.curSkill:IsAimSelf() then
            self.curSkill:Perform(self, self)
        elseif self.curSkill:IsAimEnemy() then
            self.curSkill:Perform(self, self.target)
        end
        self.dataModel:SetNextAtkTime()
    end
end

function Monster:SkillIsPerform()
    if self.curSkill then
        return self.curSkill:IsPerform()
    else
        return false
    end
end

function Monster:CancleAtk()
    if self.curSkill then
        self.curSkill:Cancel()
        self.curSkill = nil
    end
end

--用来播放动画
function Monster:ActSkill(act)
    self.actor:Skill(act)
end

function Monster:Hit()
    if self.actor then
    end
end

function Monster:Dead(causeSkill, skillCalc)
    EventTrigger:RemoveMonster(self.dataModel:GetGroup())
    if self.AI then
        self.AI:SetTransition(MonsterAIStates.Transition.Dead)
    end
    self:PlayDead(causeSkill, skillCalc)
end

function Monster:PlayDead(causeSkill, skillCalc)
    if self.view then
        self.view:Alive(false)
        if self:IsUseSelectable() then
            self.view:SetUsable(true)
        end
    end
    if causeSkill and causeSkill:GetSawPart() then
        if self.dataModel:CanDismember() then
            --肢解
            local dir = skillCalc and skillCalc:GetHurtDir(self) or Vector3.zero()
            self.view:Dismember(causeSkill:GetSawPart(), causeSkill:GetPower(), causeSkill:GetSawOtherPart(), dir)
            return
        end
    end
    if self.view:HasDieAni() then
        if self.actor then
            self.actor:Dead()
        end
    else
        --布娃娃受力
        local dir = skillCalc and skillCalc:GetHurtDir(self) or Vector3.zero()
        local sawPart = causeSkill and causeSkill:GetSawPart() or random(SawPart.MAX)
        local power = causeSkill and causeSkill:GetPower() or 0
        self.view:Ragdoll(sawPart, power, dir)
    end
end

function Monster:OnDeadBy(user)
    if user.isMonster then
        self:NotifyDead()
    end
end

function Monster:SetMonsterDir()
    if self.dataModel:IsTrap() then
        return
    end
    if self.target then
        local tarPos = self.target:GetPos()
        local selfPos = self:GetPos()
        local dir = Vector3.new(tarPos.x - selfPos.x, 0, tarPos.z - selfPos.z)
        dir = dir:setNormalize()
        self.view:SetDir(dir)
    end
end

--根据目标生成偏移
function Monster:GetTargetPos()
    if self:GetCanAtkDis() then
        self.atkRange = self:GetCanAtkDis()
    else
        self.atkRange = -1
    end

    local target = self:GetHateTarget()
    if target and self.randomAngle then
        local tarPos = target:GetPos()
        local tarPosCp = self:GetPos():clone()
        local desPos = tarPosCp:sub(tarPos)
        desPos:setNormalize()
        local x = (desPos.x * cos(self.randomAngle) - desPos.z * sin(self.randomAngle)) * self.atkRange
        local z = (desPos.x * sin(self.randomAngle) + desPos.z * cos(self.randomAngle)) * self.atkRange
        self.followPos = target:GetPos()
        --tarPos:clone():add(Vector3.new(x, 0, z))
    end
    return self.followPos
end

function Monster:UpdateSelector()
    if self.selector then
        self.selector:SelectNearest()
    end
end

function Monster:IsTargetInRange(target)
    local dis = Vector3.distNH(target:GetPos(), self:GetPos())
    local noise = 0
    if target.isPlayer then
        noise = target:GetNoiseDis()
    end
    local isInRange = false
    if not target:IsSneak() then
        if dis < self.dataModel:GetSight(true) + noise then
            isInRange = true
        end
    else
        if dis < self.dataModel:GetSight() then
            local tarDir = target:GetPos():clone():sub(self:GetPos())
            local angle = Vector3.Angle(self:GetDir(), tarDir)
            if angle < Monster.sightAngle then
                isInRange = true
            end
        end
    end
    if isInRange then
        local canReach, reachPos = self:CanSkillReachEntity(target, _G.Const.MonsterSearchRadius)
        isInRange = canReach
    end

    return isInRange
end

function Monster:GetAtkDis()
    if self.dataModel:GetCurSkill() then
        self.atkRange = self.dataModel:GetCurSkill():GetPerformMaxRange()
    else
        self.atkRange = -1
    end
    return self.atkRange
end

function Monster:GetCanAtkDis()
    local curSkill = self.dataModel:GetCurSkill()
    local canAtkDis = -1
    if curSkill then
        if curSkill:IsRushSkill() then
            canAtkDis = self.dataModel:GetCurSkill():GetPerformMaxRange()
        else
            canAtkDis = self.dataModel:GetCurSkill():GetPerformAiRange()
        end
    end
    if not canAtkDis then
        canAtkDis = 1
    end
    return canAtkDis
end

--选择目标
function Monster:OnTargetSelect(target)
    if target then
        self:AddHate(target)
        self:SetTarget(target)
        self.AI:SetTransition(MonsterAIStates.Transition.FindTarget)
    end
    self.lastHurt = target
end

--得到仇恨列表的目标
function Monster:GetHateTarget()
    local count = 0
    for i, v in pairs(self.dataModel.hateList) do
        count = count + 1
    end

    if count == 0 then
        self.target = nil
        self.AI:SetTransition(MonsterAIStates.Transition.NoTarget)
    end
    local newTarget = nil
    for i, v in pairs(self.dataModel.hateList) do
        if not v:IsDead() and (v.IsCrawl and not v:IsCrawl()) then
            if not newTarget then
                newTarget = self.dataModel.hateList[i]
            else
                local selfPos = self:GetPos()
                local dis1 = newTarget:GetPos():distNH(selfPos)
                local dis2 = v:GetPos():distNH(selfPos)
                print(dis1, dis2, newTarget:GetChoosePriorty(), v:GetChoosePriorty())
                if newTarget:GetChoosePriorty() >= v:GetChoosePriorty() and dis1 > dis2 then
                    newTarget = self.dataModel.hateList[i]
                end
            end
        else
            self:RemoveHate(v)
        end
    end

    self:SetTarget(newTarget)
    return self.target
end
--设置目标
function Monster:SetTarget(target)
    if not target then
        --print("no enemy")
        self.AI:SetTransition(MonsterAIStates.Transition.ClearHate)
    end
    self.target = target
end

--得到目标与自己的距离
function Monster:GetTarDis()
    if not self.target then
        self:GetHateTarget()
    end
    if self.target then
        return self:GetPos():distNH(self.target:GetPos())
    end
end

function Monster:Return()
    self:ClearHate()
    self.view:SetSpeed(self.dataModel:GetOutofBattleSpeed())
    self:MoveToPos(self.dataModel:GetBeginBattlePos())
end

return Monster
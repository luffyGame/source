---
--- Description 怪物的IDLE状态
--- Created by SunShubin.
--- DateTime: 2018/4/16 上午11:01
---

local MonsterAI_Idle = class("MonsterAI_Idle",require("app.object.controller.monster.states.MonsterAIState_Base"))
local random = math.random
local MonsterAIStates = require("app.object.controller.monster.MonsterAIStates")

function MonsterAI_Idle:ctor()
    self:AddTransition(MonsterAIStates.Transition.Dead,MonsterAIStates.MonsterAIState.MonsterAI_Dead)
end

function MonsterAI_Idle:OnEnter()
    --print("enter monster idle")
    if self.owner.dataModel:IsCoward() then
        self:AddTransition(MonsterAIStates.Transition.AtkedByTarget,MonsterAIStates.MonsterAIState.MonsterAI_Escape)
        self:AddTransition(MonsterAIStates.Transition.FindTarget,MonsterAIStates.MonsterAIState.MonsterAI_Escape)
    else
        self:AddTransition(MonsterAIStates.Transition.AtkedByTarget,MonsterAIStates.MonsterAIState.MonsterAI_Atked)
        self:AddTransition(MonsterAIStates.Transition.FindTarget,MonsterAIStates.MonsterAIState.MonsterAI_Follow)
    end


    self.patrolTime = random(10,30) / 10
    self.ownerData = self.owner.dataModel
    self.ownerAI = self.owner.AI
    self.patrolType = self.ownerData:GetPatrolType()
    self.owner.actor:Idle()

    if self.patrolType == 0 then

    elseif self.patrolType == 1 then
        self:AddTransition(MonsterAIStates.Transition.NoTarget,MonsterAIStates.MonsterAIState.MonsterAI_Patrol_Circle)
    else
        self:AddTransition(MonsterAIStates.Transition.NoTarget,MonsterAIStates.MonsterAIState.MonsterAI_Patrol_Point)
    end
end

function MonsterAI_Idle:OnUpdate(deltaTime)
    self.patrolTime = self.patrolTime - deltaTime
    if self.patrolTime <= 0 then
       self.ownerAI:SetTransition(MonsterAIStates.Transition.NoTarget)
    end

    self.owner:UpdateSelector()
end

function MonsterAI_Idle:OnExit()
    --print("leave Idle")
    self.ownerData:SetBeginBattlePos(self.owner:GetPos())
    self.ownerData:StartCountDown()
end

return MonsterAI_Idle
--endregion
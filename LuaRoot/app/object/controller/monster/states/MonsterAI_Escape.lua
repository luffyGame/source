---
--- Description 逃跑
--- Created by SunShubin
--- DateTime: 2018/5/18 11:35 AM
---

local MonsterAI_Escape = class("MonsterAI_Escape", require("app.object.controller.monster.states.MonsterAIState_Base"))
local MonsterAIStates = require("app.object.controller.monster.MonsterAIStates")
local random = math.random
local Vector3 = _G.Vector3
local SceneEnv = _G.SceneEnv

function MonsterAI_Escape:ctor()
    self:AddTransition(MonsterAIStates.Transition.Dead, MonsterAIStates.MonsterAIState.MonsterAI_Dead)
    self:AddTransition(MonsterAIStates.Transition.AtkedByTarget, MonsterAIStates.MonsterAIState.MonsterAI_Escape)
    self:AddTransition(MonsterAIStates.Transition.ClearHate, MonsterAIStates.MonsterAIState.MonsterAI_Patrol_Circle)
end

local dataModel = nil

function MonsterAI_Escape:OnEnter()
    --print("enter escape")
    self.owner:CancelFollow()
    self:MoveToRandomPos()
    dataModel = self.owner.dataModel
    self.owner.view:SetSpeed(self.owner.dataModel:GetFightSpeed())
    self.owner.actor:Run_Fight()

end

function MonsterAI_Escape:MoveEnd()
    if not self.owner.lastHurt then
        self.owner.AI:SetTransition(MonsterAIStates.Transition.ClearHate)
    else
        local lastHurtDM = self.owner.dataModel
        if self.owner.lastHurt:GetPos():distNH(self.owner:GetPos()) >= 10 or (lastHurtDM.IsDead and lastHurtDM:IsDead()) then
            self.owner.AI:SetTransition(MonsterAIStates.Transition.ClearHate)
        else
            self:MoveToRandomPos()
        end
    end
end

function MonsterAI_Escape:MoveToRandomPos()
    self.moveEnd = false
    self.moveEndTime = 0
    local ownerData = self.owner.dataModel
    local moveDis = ownerData:GetPatrolRange()
    if moveDis == 0 then
        moveDis = random(5, 10)
    end
    if self.owner.lastHurt then
        local selfPos = self.owner:GetPos():clone()
        selfPos:sub(self.owner.lastHurt:GetPos())
        selfPos:setNormalize()
        selfPos = self.owner:GetPos() + selfPos:mul(moveDis)

        if SceneEnv:IsPosOut(selfPos) then
            selfPos = self.owner:GetPos() + selfPos:setNormalize():mul(moveDis * -1)
        end

        self.owner:MoveToPos(selfPos)
    else
        local pos = Vector3.new(ownerData.initPos.x, 0, ownerData.initPos.z)
        local delta = Vector3.new(random(-1, 1), 0, random(-1, 1)):setNormalize()
        pos = pos + delta:mul(moveDis)

        if SceneEnv:IsPosOut(pos) then
            pos = self.owner:GetPos() + pos:setNormalize():mul(moveDis * -1)
        end

        self.owner:MoveToPos(pos)
    end
end

return MonsterAI_Escape
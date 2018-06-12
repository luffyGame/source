---
--- Description 怪物的受击状态
--- Created by SunShubin.
--- DateTime: 2018/4/16 上午11:01
---
local MonsterAI_Atked = class("MonsterAI_Atked",require("app.object.controller.monster.states.MonsterAIState_Base"))

local MonsterAIStates = require("app.object.controller.monster.MonsterAIStates")

function MonsterAI_Atked:ctor()
    self:AddTransition(MonsterAIStates.Transition.FindTarget,MonsterAIStates.MonsterAIState.MonsterAI_Follow)
    self:AddTransition(MonsterAIStates.Transition.Dead,MonsterAIStates.MonsterAIState.MonsterAI_Dead)
    self:AddTransition(MonsterAIStates.Transition.NearTarget,MonsterAIStates.MonsterAIState.MonsterAI_Atk)
end

function MonsterAI_Atked:OnEnter()
    local monsterData = self.owner.dataModel
    if monsterData:IsBoss() then
    elseif monsterData:IsPowerful() then
        if self.owner.actor:IsIdle() then
            self.owner.actor:Hit()
        end
    else
        --print("<color=red> i'm normal monster</color>")
        self.owner:CancleAtk()
        self.owner:CancelFollow()
        self.owner.actor:Hit()
    end
    --print("<color=red>enter atked state</color>")
end

function MonsterAI_Atked:OnUpdate(deltaTime)
    if not self.owner.actor:IsHit() then
        if self.owner:GetTarDis() < self.owner:GetAtkDis() then
            self.owner.AI:SetTransition(MonsterAIStates.Transition.NearTarget)
        else
            self.owner.AI:SetTransition(MonsterAIStates.Transition.FindTarget)
        end
    end
end

function MonsterAI_Atked:OnExit()

end
return MonsterAI_Atked
--endregion
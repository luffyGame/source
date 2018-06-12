---
--- Description 唤醒状态（预留怪物出场的状态）
--- Created by SunShubin.
--- DateTime: 2018/4/16
---

local MonsterAI_Awake = class("MonsterAI_Awake",require("app.object.controller.monster.states.MonsterAIState_Base"))
local MonsterAIStates = require("app.object.controller.monster.MonsterAIStates")

--_G.MonsterAI

function MonsterAI_Awake:ctor()

end

function MonsterAI_Awake:OnEnter()
    --print("enter monster awake",self.owner)
    if self.owner.dataModel:IsTrap() then
        self:AddTransition(MonsterAIStates.Transition.ReadyToIdle,MonsterAIStates.MonsterAIState.MonsterAI_FixedAtk)
    else
        self:AddTransition(MonsterAIStates.Transition.ReadyToIdle,MonsterAIStates.MonsterAIState.MonsterAI_Idle)
    end
    self.owner.actor:Idle()
end

function MonsterAI_Awake:OnUpdate(deltaTime)
    if self.owner.awake then
        self.owner.AI:SetTransition(MonsterAIStates.Transition.ReadyToIdle)
    end
end

function MonsterAI_Awake:OnAreaOpen()
    self.owner.AI:SetTransition(MonsterAIStates.Transition.ReadyToIdle)
    self.owner.awake = true
end

return MonsterAI_Awake
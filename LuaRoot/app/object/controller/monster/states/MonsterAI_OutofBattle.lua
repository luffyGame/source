---
--- Description 脱离战斗状态
--- Created by SunShubin.
--- DateTime: 2018/4/16 下午3:47
---

local MonsterAI_OutofBattle = class("MonsterAI_OutofBattle",require("app.object.controller.monster.states.MonsterAIState_Base"))
local MonsterAIStates = require("app.object.controller.monster.MonsterAIStates")

function MonsterAI_OutofBattle:ctor()
    self:AddTransition(MonsterAIStates.Transition.ReadyToIdle,MonsterAIStates.MonsterAIState.MonsterAI_Idle)
end

function MonsterAI_OutofBattle:OnEnter()
    print("enter monster outofbattle")
    self.owner:Return()
    self.owner.actor:Run_Fight()
end

function MonsterAI_OutofBattle:MoveEnd()
    self.owner.AI:SetTransition(MonsterAIStates.Transition.ReadyToIdle)
end

return MonsterAI_OutofBattle
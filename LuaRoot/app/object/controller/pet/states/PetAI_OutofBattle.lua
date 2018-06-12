---
--- Description 脱战状态
--- Created by SunShubin.
--- DateTime: 2018/6/9
---

local PetAI_OutofBattle = class("PetAI_OutofBattle",require("app.object.controller.base.AIState_Base"))
local PetAIStates = require("app.object.controller.pet.PetAIStates")

function PetAI_OutofBattle:ctor()
    self:AddTransition(PetAIStates.Transition.NearHost,PetAIStates.PetAIState.PetAI_Idle)
end

function PetAI_OutofBattle:OnEnter()
    print("pet enter outofbattle")

end

function PetAI_OutofBattle:OnUpdate(deltaTime)

end


return PetAI_OutofBattle
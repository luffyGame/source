---
--- Description 唤醒状态
--- Created by SunShubin.
--- DateTime: 2018/6/9
---

local PetAI_Awake = class("PetAI_Awake",require("app.object.controller.base.AIState_Base"))
local PetAIStates = require("app.object.controller.pet.PetAIStates")

function PetAI_Awake:ctor()
    self:AddTransition(PetAIStates.Transition.NearHost,PetAIStates.PetAIState.PetAI_Idle)
end

function PetAI_Awake:OnEnter()
    --print("pet enter awake",self.owner)
    self.owner.AI:SetTransition(PetAIStates.Transition.NearHost)
end

function PetAI_Awake:OnUpdate(deltaTime)

end


return PetAI_Awake
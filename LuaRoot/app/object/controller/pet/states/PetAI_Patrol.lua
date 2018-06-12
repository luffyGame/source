---
--- Description 巡逻状态
--- Created by SunShubin.
--- DateTime: 2018/6/9
---

local PetAI_Patrol = class("PetAI_Patrol",require("app.object.controller.base.AIState_Base"))
local PetAIStates = require("app.object.controller.pet.PetAIStates")

function PetAI_Patrol:ctor()

end

function PetAI_Patrol:OnEnter()
    --print("pet enter patrol")
    self.view:SetSpeed(self.dataModel:GetMoveSpeed())
end

function PetAI_Patrol:OnUpdate(deltaTime)

end


return PetAI_Patrol
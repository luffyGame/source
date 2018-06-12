---
--- Description 被击状态
--- Created by SunShubin.
--- DateTime: 2018/6/9
---

local PetAI_Atked = class("PetAI_Atked",require("app.object.controller.base.AIState_Base"))
local PetAIStates = require("app.object.controller.pet.PetAIStates")

function PetAI_Atked:ctor()

end

function PetAI_Atked:OnEnter()
    print("pet enter awake")
end

function PetAI_Atked:OnUpdate(deltaTime)

end


return PetAI_Atked
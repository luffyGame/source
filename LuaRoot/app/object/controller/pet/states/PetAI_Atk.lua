---
--- Description 攻击状态
--- Created by SunShubin.
--- DateTime: 2018/6/9
---

local PetAI_Atk = class("PetAI_Atk",require("app.object.controller.base.AIState_Base"))
local PetAIStates = require("app.object.controller.pet.PetAIStates")

function PetAI_Atk:ctor()

end

function PetAI_Atk:OnEnter()
    print("pet enter awake")
end

function PetAI_Atk:OnUpdate(deltaTime)

end


return PetAI_Atk
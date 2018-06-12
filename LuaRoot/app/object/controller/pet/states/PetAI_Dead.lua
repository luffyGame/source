---
--- Description 死亡状态
--- Created by SunShubin.
--- DateTime: 2018/6/9
---

local PetAI_Dead = class("PetAI_Dead",require("app.object.controller.base.AIState_Base"))
local PetAIStates = require("app.object.controller.pet.PetAIStates")

function PetAI_Dead:ctor()

end

function PetAI_Dead:OnEnter()
    print("pet enter dead")
end

function PetAI_Dead:OnUpdate(deltaTime)

end


return PetAI_Dead
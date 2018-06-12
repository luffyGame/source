---
--- Description 定时攻击状态
--- Created by SunShubin.
--- DateTime: 2018/5/21 2:46 PM
---

local MonsterAI_FixedAtk = class("MonsterAI_FixedAtk",require("app.object.controller.monster.states.MonsterAIState_Base"))

function MonsterAI_FixedAtk:OnEnter()
    --print("enter monster fixed atk")
end


function MonsterAI_FixedAtk:OnUpdate(deltaTime)
    self.owner:Atk()
end


return MonsterAI_FixedAtk
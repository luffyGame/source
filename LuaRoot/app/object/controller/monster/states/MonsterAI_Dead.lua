---
--- Description 死亡状态
--- Created by wade.
--- DateTime: 2018/4/16 下午2:53
---

local MonsterAI_Dead = class("MonsterAI_Dead",require("app.object.controller.monster.states.MonsterAIState_Base"))

function MonsterAI_Dead:OnEnter()
    --print("enter monster dead")
    self.owner:CancleAtk()
    self.owner:CancelFollow()
end
return MonsterAI_Dead
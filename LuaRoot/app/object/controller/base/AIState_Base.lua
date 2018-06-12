---
--- Description AI Base
--- Created by SunShubin.
--- DateTime: 2018/6/9
---

local FsmState = FsmState
local AIStage_Base = class("AIStage_Base",FsmState)

function AIStage_Base:GetStateId()
    return self.stateId
end

function AIStage_Base:ctor()
    self.map = {}
end

function AIStage_Base:AddTransition(transition,fsmID)
    if not self.map then
        self.map = {}
    end
    self.map[transition] = fsmID
end

function AIStage_Base:DelTransition(transition)
    self.map[transition] = nil
end

return AIStage_Base


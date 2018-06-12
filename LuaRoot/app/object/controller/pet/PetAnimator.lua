---
--- Description Pet动画
--- Created by SunShubin.
--- DateTime: 2018/3/26 下午3:58
---

local PetState = {
    IDLE = 1,
    RUN = 2,
    DEATH = 3,
    HIT = 4,
    SKILL = 5,
    RUN_FIGHT = 6,
}

--region IDLE
local IdleState = class("IdleState",FsmState)
function IdleState:GetStateID()
    return PetState.IDLE
end

function IdleState:OnEnter()
    self.animTime = self.owner:PlayAni("idle",0.5)
end

function IdleState:OnUpdate(deltaTime)
    self.animTime = self.animTime - deltaTime
    if self.animTime <= 0.5 then
        self.owner.animator:ChangeState(PetState.STAND,true)
    end
end

PetState[PetState.IDLE] = IdleState
--endregion

--region STAND
local StandState = class("StandState",FsmState)
function StandState:GetStateID()
    return PetState.STAND
end

function StandState:OnEnter()
    self.owner:PlayAni("idle",0.5)
end
PetState[PetState.STAND] = StandState
--endregion

--region RUN
local RunState = class("RunState",FsmState)
function RunState:GetStateID()
    return PetState.RUN
end

function RunState:OnEnter()
    self.owner:PlayAni("run",0.5)
end
PetState[PetState.RUN] = RunState
--endregion

local PetAnimator = class("PetAnimator",StateMachine)

function PetAnimator:CreateState(stateID)
    return PetState[stateID].new()
end

function PetAnimator:Start()
    self:ChangeState(PetState.IDLE)
end

function PetAnimator:Stand()
    self:ChangeState(PetState.STAND)
end

function PetAnimator:Run()
    self:ChangeState(PetState.RUN)
end

function PetAnimator:Idle()
    self:ChangeState(PetState.IDLE)
end
return PetAnimator






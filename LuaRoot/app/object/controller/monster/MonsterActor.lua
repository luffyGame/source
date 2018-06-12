---
--- Description monster 的动画
--- Created by wangliang.
--- DateTime: 2018/3/27 下午3:45
---
local random = math.random
local ContAction,OnceAction,StateMachine = ContAction,OnceAction,StateMachine
local registerActorState = registerActorState

local MonsterState = {
    IDLE = 1,
    RUN = 2,
    DEATH = 3,
    HIT = 4,
    SKILL = 5,
    RUN_FIGHT = 6,
}

registerActorState(MonsterState,MonsterState.IDLE,class("idle",ContAction),0.5)

registerActorState(MonsterState,MonsterState.RUN,class("run",ContAction),0.2)

registerActorState(MonsterState,MonsterState.RUN_FIGHT,class("run_fight",ContAction),0.2)

registerActorState(MonsterState,MonsterState.DEATH,class("death",OnceAction),0.2)

local ActionState = class("ActionState",OnceAction)
function ActionState:OnActionOver()
    self.owner.actor:ChangeState(MonsterState.IDLE)
end

registerActorState(MonsterState,MonsterState.HIT,class("hit",ActionState),0.1)

local SkillState = class("SkillState",ActionState)
function SkillState:Init(act)
    self.act = act
end
function SkillState:IsSame(params)
    return self.act == params
end
function SkillState:GetAction()
    return self.act
end
registerActorState(MonsterState,MonsterState.SKILL,SkillState,0.3,0.3)


local MonsterActor = class("MonsterActor",StateMachine)

function MonsterActor:Start()
    self:ChangeState(MonsterState.IDLE)
end

function MonsterActor:CreateState(stateId)
    return MonsterState[stateId].new()
end

function MonsterActor:Idle()
    self:ChangeState(MonsterState.IDLE)
end

function MonsterActor:Run()
    self:ChangeState(MonsterState.RUN)
end

function MonsterActor:Run_Fight()
    self:ChangeState(MonsterState.RUN_FIGHT)
end

function MonsterActor:Dead()
   self:ChangeState(MonsterState.DEATH)
end

function MonsterActor:Hit()
    self:ChangeState(MonsterState.HIT)
end

function MonsterActor:Skill(act)
    if not act then
        act = "animation is null!"
    end
    self:ChangeState(MonsterState.SKILL,false,act)
end

function MonsterActor:IsHit()
    local curStateId = self.currentState and self.currentState:GetStateId()
    if curStateId == MonsterState.HIT then
        return true
    else
        return false
    end
end

function MonsterActor:IsIdle()
    local curStateId = self.currentState and self.currentState:GetStateId()
    if curStateId == MonsterState.IDLE then
        return true
    else
        return false
    end
end


MonsterActor.StateId = MonsterState

return MonsterActor
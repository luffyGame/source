--相当于Animator
local ContAction,OnceAction,StateMachine = ContAction,OnceAction,StateMachine
local registerActorState = registerActorState

local PlayerState = {
    IDLE = 1,
    RUN = 2,
    PICK = 3,
    OPEN = 4,
    DEATH = 5,
    SKILL = 6,
    ALERT = 7,
    SNEAK = 8,--潜行
    CRAWL = 9,--爬行
}
--==========================
local IdleState = class("IdleState",ContAction)
IdleState.isActionAdpated = true --动作自适应
local idleacts,idle_default = {},"idle"
function IdleState:GetAction()
    local actPost = self.owner:GetActionPost()
    actPost = self.owner:GetSneakAnim(actPost)
    local act
    if actPost then
        act = idleacts[actPost]
        if not act then
            act = "idle_" .. actPost
            idleacts[actPost] = act
        end
    end
    return act or idle_default
end
function IdleState:OnWeaponEquipped()
    self:OnEnter()
end
registerActorState(PlayerState,PlayerState.IDLE,IdleState)

local RunState = class("RunState",ContAction)
local runacts,run_default = {},"run"
function RunState:GetAction()
    local actPost = self.owner:GetActionPost()
    actPost = self.owner:GetSneakAnim(actPost)
    local act
    if actPost then
        act = runacts[actPost]
        if not act then
            act = "run_" .. actPost
            runacts[actPost] = act
        end
    end
    return act or run_default
end
function RunState:OnWeaponEquipped()
    self:OnEnter()
end
registerActorState(PlayerState,PlayerState.RUN,RunState)

local UseActionState = class("UseActionState",OnceAction)
function UseActionState:OnActionOver()
    self.owner.actor:ChangeState(PlayerState.IDLE)
end

registerActorState(PlayerState,PlayerState.SNEAK,class("sneak",ContAction))

registerActorState(PlayerState,PlayerState.CRAWL,class("crawl",ContAction))

registerActorState(PlayerState,PlayerState.PICK,class("pick",UseActionState))

registerActorState(PlayerState,PlayerState.OPEN,class("open",UseActionState))

registerActorState(PlayerState,PlayerState.DEATH,class("death",OnceAction))

local SkillState = class("SkillState",OnceAction)
function SkillState:Init(act)
    self.act = act
end
function SkillState:IsSame(params)
    return self.act == params
end
function SkillState:GetAction()
    return self.act
end
function SkillState:OnActionOver()
    self.owner.actor:ChangeState(PlayerState.ALERT)
end
registerActorState(PlayerState,PlayerState.SKILL,SkillState)

local AlertState = class("AlertState",ContAction)
local alertacts,alert_default = {},"idle_fight"
function AlertState:GetAction()
    local actPost = self.owner:GetActionPost()
    actPost = self.owner:GetSneakAnim(actPost)
    local act
    if actPost then
        act = alertacts[actPost]
        if not act then
            act = "idle_fight_" .. actPost
            alertacts[actPost] = act
        end
    end
    return act or alert_default
end
function AlertState:OnWeaponEquipped()
    self:OnEnter()
end
registerActorState(PlayerState,PlayerState.ALERT,AlertState)

local PlayerActor = class("PlayerActor",StateMachine)

function PlayerActor:Start()
    self:ChangeState(PlayerState.IDLE)
end

function PlayerActor:CreateState(stateId)
    return PlayerState[stateId].new()
end

function PlayerActor:Idle()
    self:ChangeState(PlayerState.IDLE)
end

function PlayerActor:Run()
    self:ChangeState(PlayerState.RUN)
end

function PlayerActor:Sneak()
    if self.currentState then
        self:ChangeState(self.currentState:GetStateId(),true)
    end
end

function PlayerActor:Crawl()
    self:ChangeState(PlayerState.CRAWL)
end

function PlayerActor:Pick()
    self:ChangeState(PlayerState.PICK)
end

function PlayerActor:Open()
    self:ChangeState(PlayerState.OPEN)
end

function PlayerActor:Dead()
    self:ChangeState(PlayerState.DEATH)
end

function PlayerActor:Skill(act)
    self:ChangeState(PlayerState.SKILL,false,act)
end

function PlayerActor:CanDoCmd()
    local curStateId = self.currentState and self.currentState:GetStateId()
    return curStateId == PlayerState.IDLE or curStateId == PlayerState.RUN or curStateId == PlayerState.ALERT
end

function PlayerActor:CanRun()
    local curStateId = self.currentState and self.currentState:GetStateId()
    return curStateId == PlayerState.IDLE or curStateId == PlayerState.RUN or curStateId == PlayerState.ALERT
            or curStateId == PlayerState.SKILL
end

function PlayerActor:IsRun()
    return self.currentState and self.currentState:GetStateId() ==PlayerState.RUN
end

function PlayerActor:CanAtk()
    return self:CanDoCmd()
end

function PlayerActor:CanUse()
    return self:CanDoCmd()
end

function PlayerActor:OnWeaponEquipped()
    if self.currentState and self.currentState.OnWeaponEquipped then
        self.currentState:OnWeaponEquipped()
    end
end

PlayerActor.StateId = PlayerState

return PlayerActor

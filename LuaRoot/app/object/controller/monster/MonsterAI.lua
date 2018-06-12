---
--- Description Monster AI
--- Created by SunShubin.
--- DateTime: 2018/3/29 上午10:01
---
local MonsterAIStates = require("app.object.controller.monster.MonsterAIStates")

--region 状态
local MonsterAI_Awake = require("app.object.controller.monster.states.MonsterAI_Awake")
local MonsterAI_Idle = require("app.object.controller.monster.states.MonsterAI_Idle")
local MonsterAI_Patrol_Point = require("app.object.controller.monster.states.MonsterAI_Patrol_Point")
local MonsterAI_Patrol_Circle = require("app.object.controller.monster.states.MonsterAI_Patrol_Circle")
local MonsterAI_Follow = require("app.object.controller.monster.states.MonsterAI_Follow")
local MonsterAI_Atk = require("app.object.controller.monster.states.MonsterAI_Atk")
local MonsterAI_Atked = require("app.object.controller.monster.states.MonsterAI_Atked")
local MonsterAI_Dead = require("app.object.controller.monster.states.MonsterAI_Dead")
local MonsterAI_OutofBattle = require("app.object.controller.monster.states.MonsterAI_OutofBattle")
local MonsterAI_Escape = require("app.object.controller.monster.states.MonsterAI_Escape")
local MonsterAI_FixedAtk = require("app.object.controller.monster.states.MonsterAI_FixedAtk")
--endregion

local StateMachine = StateMachine
local MonsterAI = class("MonsterAI",StateMachine)

local function registerState(states,id,cls)
    cls.stateId = id
    states[id] = cls
end

registerState(MonsterAIStates.MonsterAIState,MonsterAIStates.MonsterAIState.MonsterAI_Awake,MonsterAI_Awake)
registerState(MonsterAIStates.MonsterAIState,MonsterAIStates.MonsterAIState.MonsterAI_Idle,MonsterAI_Idle)
registerState(MonsterAIStates.MonsterAIState,MonsterAIStates.MonsterAIState.MonsterAI_Patrol_Point,MonsterAI_Patrol_Point)
registerState(MonsterAIStates.MonsterAIState,MonsterAIStates.MonsterAIState.MonsterAI_Patrol_Circle,MonsterAI_Patrol_Circle)
registerState(MonsterAIStates.MonsterAIState,MonsterAIStates.MonsterAIState.MonsterAI_Follow,MonsterAI_Follow)
registerState(MonsterAIStates.MonsterAIState,MonsterAIStates.MonsterAIState.MonsterAI_Atk,MonsterAI_Atk)
registerState(MonsterAIStates.MonsterAIState,MonsterAIStates.MonsterAIState.MonsterAI_Atked,MonsterAI_Atked)
registerState(MonsterAIStates.MonsterAIState,MonsterAIStates.MonsterAIState.MonsterAI_Dead,MonsterAI_Dead)
registerState(MonsterAIStates.MonsterAIState,MonsterAIStates.MonsterAIState.MonsterAI_OutofBattle,MonsterAI_OutofBattle)
registerState(MonsterAIStates.MonsterAIState,MonsterAIStates.MonsterAIState.MonsterAI_Escape,MonsterAI_Escape)
registerState(MonsterAIStates.MonsterAIState,MonsterAIStates.MonsterAIState.MonsterAI_FixedAtk,MonsterAI_FixedAtk)

function MonsterAI:CreateState(stateID)
    return MonsterAIStates.MonsterAIState[stateID].new()
end

function MonsterAI:OnUpdate(deltaTime)
    if self.currentState then
        self.currentState:OnUpdate(deltaTime)
    end
end

function MonsterAI:SetTransition(transition)
    if self.currentState.map[transition] then
        self:ChangeState(self.currentState.map[transition],true)
    end
end

function MonsterAI:Start()
    self:ChangeState(MonsterAIStates.MonsterAIState.MonsterAI_Awake)
end

function MonsterAI:MoveEnd()
    if self.currentState and self.currentState.MoveEnd then
        self.currentState:MoveEnd()
    end
end

function MonsterAI:EventTrigger(type,param)
    if self.currentState and self.currentState.EventTrigger then
        self.currentState:EventTrigger(type,param)
    end
end


function MonsterAI:OnAreaOpen()
    if self.currentState and self.currentState.OnAreaOpen then
        self.currentState:OnAreaOpen()
    end
end

function MonsterAI:HurtBy(user)
    if self.currentState  then
        if self.currentState.HurtBy then
            self.currentState:HurtBy(user)
        end
        self:SetTransition(MonsterAIStates.Transition.AtkedByTarget)
    end
end

return MonsterAI



---
--- Description Pet AI
--- Created by SunShubin.
--- DateTime: 2018/3/26 下午3:58
---

local PetAIStates = require("app.object.controller.pet.PetAIStates")

--region 状态
local PetAI_Awake = require("app.object.controller.pet.states.PetAI_Awake")
local PetAI_Idle = require("app.object.controller.pet.states.PetAI_Idle")
local PetAI_Patrol = require("app.object.controller.pet.states.PetAI_Patrol")
local PetAI_Follow = require("app.object.controller.pet.states.PetAI_Follow")
local PetAI_Atk = require("app.object.controller.pet.states.PetAI_Atk")
local PetAI_Atked = require("app.object.controller.pet.states.PetAI_Atked")
local PetAI_Dead = require("app.object.controller.pet.states.PetAI_Dead")
local PetAI_OutofBattle = require("app.object.controller.pet.states.PetAI_OutofBattle")
--endregion

local StateMachine = StateMachine
local PetAI = class("PetAI", StateMachine)

local function registerState(states,id,cls)
    cls.stateId = id
    states[id] = cls
end

registerState(PetAIStates.PetAIState,PetAIStates.PetAIState.PetAI_Awake,PetAI_Awake)
registerState(PetAIStates.PetAIState,PetAIStates.PetAIState.PetAI_Idle,PetAI_Idle)
registerState(PetAIStates.PetAIState,PetAIStates.PetAIState.PetAI_Patrol,PetAI_Patrol)
registerState(PetAIStates.PetAIState,PetAIStates.PetAIState.PetAI_Follow,PetAI_Follow)
registerState(PetAIStates.PetAIState,PetAIStates.PetAIState.PetAI_Atk,PetAI_Atk)
registerState(PetAIStates.PetAIState,PetAIStates.PetAIState.PetAI_Atked,PetAI_Atked)
registerState(PetAIStates.PetAIState,PetAIStates.PetAIState.PetAI_Dead,PetAI_Dead)
registerState(PetAIStates.PetAIState,PetAIStates.PetAIState.PetAI_OutofBattle,PetAI_OutofBattle)


function PetAI:CreateState(stateID)
    return PetAIStates.PetAIState[stateID].new()
end

function PetAI:Start()
    self:ChangeState(PetAIStates.PetAIState.PetAI_Awake)
end

function PetAI:OnUpdate(deltaTime)
    if self.currentState then
        self.currentState:OnUpdate(deltaTime)
    end
end

function PetAI:SetTransition(transition)
    if self.currentState.map[transition] then
        self:ChangeState(self.currentState.map[transition],true)
    end
end

function PetAI:MoveEnd()
    if self.currentState and self.currentState.MoveEnd then
        self.currentState:MoveEnd()
    end
end

function PetAI:HurtBy(user)
    if self.currentState and self.currentState.HurtBy then
        self.currentState:HurtBy(user)
    end
end

return PetAI


--local FsmState = FsmState
--local StateMachine = StateMachine
--local math = math
--local random = math.random
--local Vector3 = Vector3
--
--local PetAIState = {
--    IDLE = 1,
--    FOLLOW = 2,
--}
--
----region Base
--local BaseState_AI = class("BaseState_AI", FsmState)
--
----endregion
--
----region IDLE
--local IdleState_AI = class("IdleState_AI", BaseState_AI)
--function IdleState_AI:GetStateID()
--    return PetAIState.IDLE
--end
--
--function IdleState_AI:OnEnter()
--    --print("enter idle state")
--    self.super.owner = self.owner
--    self.owner.owner:RegisterDataNotify("pos", self.OnPosChange, self)
--    if random(5) < 3 then
--        self.owner.animator:Idle()
--    else
--        self.owner.animator:Stand()
--    end
--    self.hostPos = self.owner.host:GetPos()
--end
--
--function IdleState_AI:OnExit()
--    self.owner.owner:UnregisterDataNotify("pos", self.OnPosChange, self)
--end
--
--function IdleState_AI:OnPosChange(pos)
--    if self.hostPos then
--        if self.hostPos:dist(pos) > self.owner.dataModel.followrange then
--            self.owner.AI:ChangeState(PetAIState.FOLLOW, true)
--        end
--    end
--end
--
--PetAIState[PetAIState.IDLE] = IdleState_AI
----endregion
--
--
----region FOLLOW
--local FollowState_AI = class("FollowState_AI", BaseState_AI)
--function FollowState_AI:GetStateID()
--    return PetAIState.FOLLOW
--end
--local followTime = 0.3
--function FollowState_AI:OnEnter()
--    --print("enter follow state")
--    self.super.owner = self.owner
--    self.super:OnEnter()
--    self.time = followTime
--    self.owner.animator:Run()
--
--    if self.desPos == nil then
--        self.desPos = Vector3.new(0, 0, 0)
--    end
--end
--
--function FollowState_AI:OnUpdate(deltaTime)
--    self.time = self.time + deltaTime
--    if self.time > followTime then
--        local hostPos = self.owner.owner:GetPos()
--        if self.lastPos then
--            if self.lastPos:distNH(hostPos) < 1 then
--                return
--            end
--        end
--        self.lastPos = hostPos
--
--        local dir = self.owner.owner:GetDir()
--        if dir then
--            dir = dir:setNormalize()
--            self.desPos:set(hostPos.x + dir.z, hostPos.y, hostPos.z - dir.x)
--            self.owner:StartNav(self.desPos, 0)
--        else
--            self.desPos:set(hostPos.x, hostPos.y, hostPos.z)
--            self.owner:StartNav(hostPos, 2)
--        end
--        self.time = 0
--    end
--end
--
--PetAIState[PetAIState.FOLLOW] = FollowState_AI
----endregion
--







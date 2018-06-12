---
--- Description 跟随状态
--- Created by SunShubin.
--- DateTime: 2018/6/9
---

local PetAI_Follow = class("PetAI_Follow",require("app.object.controller.base.AIState_Base"))
local PetAIStates = require("app.object.controller.pet.PetAIStates")
local PetArea = _G.Const.PetArea

local Transition = PetAIStates.Transition
local PetAIState = PetAIStates.PetAIState

function PetAI_Follow:ctor()
    self:AddTransition(Transition.NearHost,PetAIState.PetAI_Idle)
    self:AddTransition(Transition.LostHost,PetAIState.PetAI_OutofBattle)
end

function PetAI_Follow:OnEnter()
    --print("pet enter follow")
    local owner = self.owner
    owner.view:SetSpeed(owner.dataModel:GetFightSpeed())
    owner.actor:Run_Fight()
    self.owner:StartFollow()
end

function PetAI_Follow:OnUpdate(deltaTime)
    local curArea = self.owner:GetArea()
    if curArea == PetArea.AreaA then
        self.owner.AI:SetTransition(Transition.NearHost)
    elseif curArea == PetArea.AreaB then
        self.owner:StartFollow()
    else
        self.owner.AI:SetTransition(Transition.PetAI_OutofBattle)
    end
end


return PetAI_Follow
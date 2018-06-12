---
--- Description Idle状态
--- Created by SunShubin.
--- DateTime: 2018/6/9
---

local PetAI_Idle = class("PetAI_Idle",require("app.object.controller.base.AIState_Base"))
local PetAIStates = require("app.object.controller.pet.PetAIStates")
local Transition = PetAIStates.Transition
local PetAIState = PetAIStates.PetAIState
local PetArea = _G.Const.PetArea

function PetAI_Idle:ctor()
    self:AddTransition(Transition.FarawayHost,PetAIState.PetAI_Follow)
    self:AddTransition(Transition.LostHost,PetAIState.PetAI_OutofBattle)
end

function PetAI_Idle:OnEnter()
    --print("pet enter idle",self.owner)
    self.owner.actor:Idle()
end

function PetAI_Idle:OnUpdate(deltaTime)
    local curArea = self.owner:GetArea()
    if curArea == PetArea.AreaA then
    elseif curArea == PetArea.AreaB then
        self.owner.AI:SetTransition(Transition.FarawayHost)
    else
        self.owner.AI:SetTransition(Transition.PetAI_OutofBattle)
    end
end


return PetAI_Idle
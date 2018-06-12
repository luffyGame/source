---
--- Description 宠物AI状态和切换
--- Created by SunShubin
--- DateTime: 2018/6/9
---
local PetAIStates = class("PetAIStates")
PetAIStates.Transition = {
    NearHost = 1,
    FarawayHost = 2,
    LostHost = 3,
    FindTarget = 4,
    Dead = 5,
    ClearHate = 6,
}

PetAIStates.PetAIState = {
    PetAI_Awake = 1,
    PetAI_Idle = 2,
    PetAI_Patrol = 3,
    PetAI_Follow = 4,
    PetAI_Atk = 5,
    PetAI_Atked = 6,
    PetAI_Dead = 7,
    PetAI_OutofBattle = 8,
}
return PetAIStates
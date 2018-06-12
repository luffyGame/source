---
--- Description 怪物AI状态和切换
--- Created by SunShubin
--- DateTime: 2018/4/19 上午11:44
---
local MonsterAIStates = class("MonsterAIStates")
MonsterAIStates.Transition = {
    FindTarget = 1,
    NearTarget = 2,
    FarawayTarget = 3,
    AtkedByTarget = 4,
    ReadyToIdle = 5,
    NoTarget = 6,
    Dead = 7,
    ClearHate = 8,
}

MonsterAIStates.MonsterAIState = {
    MonsterAI_Awake = 1,
    MonsterAI_Idle = 2,
    MonsterAI_Patrol_Point = 3,
    MonsterAI_Patrol_Circle = 4,
    MonsterAI_Follow = 5,
    MonsterAI_Atk = 6,
    MonsterAI_Atked = 7,
    MonsterAI_Dead = 8,
    MonsterAI_OutofBattle = 9,
    MonsterAI_Escape = 10,
    MonsterAI_FixedAtk = 11,
}
return MonsterAIStates
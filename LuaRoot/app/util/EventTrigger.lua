---
--- Description 事件触发器
--- Created by SunShubin.
--- DateTime: 2018/4/24 下午6:07
---
local SceneEnv = SceneEnv
local LuaUtility = CS.Game.LuaUtility

local EventTrigger = {
    hostPlayer = nil,
    stage = nil,
    playerPosEvent = {},
    monsterGroup = {},
    monsterDeadTrigger = {},
    monsterTrigger = {},
}

EventTrigger.Trigger = "EventTrigger.Trigger"

EventTrigger.TriggerType = {
    None = 0,
    MonsterDead = 1,
    PlayerEnterTrigger = 2,
    MonsterTrigger = 3,
    AreaOpen = 4,
}

EventTrigger.EventType = {
    None = 0,
    Idle = 1,
    Scene = 2,
    Show = 3,
}
EventTrigger.ConditionMsg = "EventTrigger.ConditionMsg"
EventTrigger.Condition = {
    PlayerEnter = 1,
    AreaOpen = 2,
}
EventTrigger.Event = {
    [1] = 2, --触发区域
}

EventTrigger.PlayerEnter = "EventTrigger.Enter"
EventTrigger.PlayerExit = "EventTrigger.Exit"


EventTrigger.AreaOpen = "EventTrigger.AreaOpen"
EventTrigger.OperateItem = "EventTrigger.OperateItem"

EventTrigger.ExecuteMsg = {
    [1] = EventTrigger.AreaOpen,
    [2] = EventTrigger.OperateItem,
}

function EventTrigger:EnterStage(stage)
    local location = stage:GetLocation()
    self:EnterScene(location)
end

function EventTrigger:EnterScene(location)
    self.location = location
    if self.location and self.location.monsterTrigger then
        for i, v in ipairs(self.location.monsterTrigger) do
            self.monsterTrigger[i] = {}
            self.monsterTrigger[i].triggerList = {}
            if v.triggerList then
                for _, value in ipairs(v.triggerList) do
                    self.monsterTrigger[i].triggerList[value] = true
                end
            end
        end
    end

    local onPlayerEnter = function(ID, objId)
        self:OnPlayerTriggerEnter(ID, objId)
    end

    local onPlayerExit = function(ID, objId)
        self:OnPlayerTriggerExit(ID, objId)
    end
    self.triggers = SceneEnv:GetTriggerGroup()

    if SceneEnv:GetTriggerGroup() then
        LuaUtility.RegisterPlayerTriggers(self.triggers, onPlayerEnter, onPlayerExit)
    end

    local onMonsterEnter = function(ID, objId)
        self:OnMonsterTriggerEnter(ID, objId)
    end

    local onMonsterExit = function(ID, objId)
        self:OnMonsterTriggerExit(ID, objId)
    end
    if SceneEnv:GetTriggerGroup() then
        LuaUtility.RegisterMonsterTriggers(self.triggers, onMonsterEnter, onMonsterExit)

    end
end


--region 某个区域怪物死亡事件
function EventTrigger:AddMonster(groupID)
    if groupID ~= 0 and groupID then
        if self.monsterGroup[groupID] then
            self.monsterGroup[groupID] = self.monsterGroup[groupID] + 1
        else
            self.monsterGroup[groupID] = 1
        end
    end
end

function EventTrigger:RemoveMonster(groupID)
    if groupID ~= 0 and groupID then
        if self.monsterGroup[groupID] then
            self.monsterGroup[groupID] = self.monsterGroup[groupID] - 1
            if self.monsterGroup[groupID] == 0 then
                print("<color=red>monster group:" .. groupID .. " all dead!!!</color>")
                self.monsterDeadTrigger[groupID] = true
            end
        end
    end
end

--得到怪物死亡触发状态
function EventTrigger:GetMonsterTrigger(groupID)
    return self.monsterDeadTrigger[groupID]
end

--endregion


function EventTrigger:RemoveEvent(triggerType, param)
    if triggerType == EventTrigger.TriggerType.MonsterDead then
        self.monsterGroup[param] = nil
    elseif triggerType == EventTrigger.TriggerType.PlayerEnterTrigger then
        self.playerPosEvent[param] = nil
    end
end

function EventTrigger:Release()
    self.playerPosEvent = {}
    self.monsterGroup = {}
    self.monsterDeadTrigger = {}
    if self.triggers then
        LuaUtility.RegisterPlayerTriggers(self.triggers)
        LuaUtility.RegisterMonsterTriggers(self.triggers)
    end
end

function EventTrigger:Ready()

end

function EventTrigger:OnPlayerTriggerEnter(ID, objId)
    print("player enter:", ID, objId)
    self:FireEvent(EventTrigger.Trigger, EventTrigger.TriggerType.PlayerEnterTrigger, ID, true)
    self:FireEvent(EventTrigger.ConditionMsg, EventTrigger.Condition.PlayerEnter, ID, true)

    self:FireEvent(EventTrigger.PlayerEnter..ID)
end

function EventTrigger:OnPlayerTriggerExit(ID, objId)
    print("player exit:", ID, objId)
    self:FireEvent(EventTrigger.Trigger, EventTrigger.TriggerType.PlayerEnterTrigger, ID, false)
    self:FireEvent(EventTrigger.ConditionMsg, EventTrigger.Condition.PlayerEnter, ID, false)

    self:FireEvent(EventTrigger.PlayerExit..ID)
end

function EventTrigger:OnMonsterTriggerEnter(ID, objId)
    print("monster enter:", ID, objId)
    local tempMonster = _G.MonsterManger:GetMonster(objId)
    if tempMonster and tempMonster.uniqueID and self.monsterTrigger[ID] then
        if self.monsterTrigger[ID].triggerList[tempMonster.uniqueID] then
            self:FireEvent(EventTrigger.Trigger, EventTrigger.TriggerType.MonsterTrigger, ID)
        end
    end
end

function EventTrigger:OnMonsterTriggerExit(ID, objId)
    print("monster exit:", ID, objId)
    local tempMonster = _G.MonsterManger:GetMonster(objId)
    if tempMonster.uniqueID and self.monsterTrigger[ID] then
        print(self.monsterTrigger[ID].triggerList[objId])
        if self.monsterTrigger[ID] and self.monsterTrigger[ID].triggerList[objId] then
            self:FireEvent(EventTrigger.Trigger, EventTrigger.TriggerType.MonsterTrigger, ID)
        end
    end
end

function EventTrigger:ExecuteEvent(eventType, eventParam)
    print("executeEvent", eventType, eventParam)
    self:FireEvent(EventTrigger.ConditionMsg, EventTrigger.Event[eventType], eventParam)
end

function EventTrigger:FireExecuteEvent(eventType,eventParam)
    local eventMsg = EventTrigger.ExecuteMsg[eventType]
    if eventMsg then
        self:FireEvent(eventMsg..eventParam)
        print("fireEvent:",eventMsg..eventParam)
    end
end

require("framework.EventDispatcher").Extend(EventTrigger)
_G.EventTrigger = EventTrigger
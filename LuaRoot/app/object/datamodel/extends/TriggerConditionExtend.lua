---
--- Description 触发条件和执行事件的拓展
--- Created by SunShubin.
--- DateTime: 2018/5/17 4:20 PM
---

local TriggerConditionExtend = class("TriggerConditionExtend")
local EventTrigger = _G.EventTrigger

function TriggerConditionExtend:GetTriggerConditions()
    return self.dataModel.loc.triggerConditions
end

function TriggerConditionExtend:GetExecuteEvents()
    return self.dataModel.loc.executeEvents
end

function TriggerConditionExtend:FireExecuteEvents()
    local executeEvents = self:GetExecuteEvents()
    if executeEvents then
        for i, v in ipairs(executeEvents) do
            EventTrigger:FireExecuteEvent(v.executeType, v.executeParam)
        end
    end
end

function TriggerConditionExtend:SetTriggerCondition(type, param, success)

    if not self.reachCount then
        self.reachCount = 0
    end
    print("set trigger condition:", type, param, success)
    if self:GetTriggerConditions() then
        for i, v in pairs(self:GetTriggerConditions()) do
            print(v.triggerType, v.triggerParam, v)
            if v.triggerType == type and v.triggerParam == param then
                if success ~= nil then
                    if success then
                        self.reachCount = self.reachCount + 1
                    else
                        self.reachCount = self.reachCount - 1
                    end
                else
                    self.reachCount = self.reachCount + 1
                end
            end
        end
    end
    --print(self.reachCount)
end

function TriggerConditionExtend:ReachTheCondition()
    if not self.dataModel.loc.triggerConditions then
        return true
    end
    if self.reachCount == #self.dataModel.loc.triggerConditions then
        return true
    end
    return false
end

return TriggerConditionExtend
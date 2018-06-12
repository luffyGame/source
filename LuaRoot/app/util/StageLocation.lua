---
--- Description 关卡配置选择
--- Created by SunShubin.
--- DateTime: 2018/5/9 3:18 PM
---
local StageLocation = class("StageLocation")
local CfgData = CfgData
local HostPlayer = HostPlayer
local random = math.random

local function GetLocationIndex(arr_Location,arr_LocationWeight)
    for i, _ in ipairs(arr_Location) do
        print("<color=blue>",arr_Location[i],arr_LocationWeight[i],"</color>")
    end
    if #arr_Location == 1 then
        return arr_Location[1]
    end

    local weight = {}
    local tempVal = 0
    for i, v in ipairs(arr_LocationWeight) do
        tempVal = tempVal + v
        weight[i] = tempVal
    end
    local randomVal = random(0,weight[#weight])
    for i, v in ipairs(weight) do
        if randomVal < v then
            return arr_Location[i]
        end
    end
end

function StageLocation:GetLocation(stageId)
    local location = CfgData:GetLocation(stageId)
    local stageCfg = CfgData:GetStage(stageId)
    local locationIndex = 1
    ---todo:改为从Stage存储数据中拿
    locationIndex = GetLocationIndex(stageCfg.setRandomId,stageCfg.setRandomRate)
    self.location = location[locationIndex]
    print("stageID:",stageId,"Index:", locationIndex)
    return location[locationIndex],locationIndex
end

function StageLocation:GetLocationIndex(stageId)
    local stageCfg = CfgData:GetStage(stageId)
    local locationIndex = GetLocationIndex(stageCfg.setRandomId,stageCfg.setRandomRate)
    return locationIndex
end

function StageLocation:GetLocationByIndex(stageId,index)
    local location = CfgData:GetLocation(stageId)
    local locationIndex = index
    self.location = location[locationIndex]
    return location[locationIndex]
end

_G.StageLocation = StageLocation.new()
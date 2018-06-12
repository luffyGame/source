---
--- Description 场景物体FACTORY
--- Created by SunShubin.
--- DateTime:2018/5/31 9:49 AM
---
local commonItem = require("app.object.entity.SceneItems.SceneItem_NormalItem")
local CfgData = _G.CfgData
local ItemType = {
    DOOR = 109,--门
    CEILING = 110,--天花板
    OPERATEITEM = 112,--操作物体
    PIPELINE = 113, --管道
    ELEVATOR = 114, --电梯
    TRANSFERPOINT = 115,--传送点
    REMOTEITEM = 116,--遥控物体
    CONVEYORBELT = 117,--传送带
    AIRWALL = 118,--空气墙
}

local SceneItemFactory = {
    [ItemType.DOOR] = require("app.object.entity.SceneItems.SceneItem_Door"),
    [ItemType.CEILING] = require("app.object.entity.SceneItems.SceneItem_Ceiling"),
    [ItemType.OPERATEITEM] = require("app.object.entity.SceneItems.SceneItem_OperateItem"),
    [ItemType.PIPELINE] = require("app.object.entity.SceneItems.SceneItem_Pipeline"),
    [ItemType.ELEVATOR] = require("app.object.entity.SceneItems.SceneItem_Elevator"),
    [ItemType.TRANSFERPOINT] = require("app.object.entity.SceneItems.SceneItem_TransferPoint"),
    [ItemType.REMOTEITEM] = require("app.object.entity.SceneItems.SceneItem_RemoteCtrl"),
    [ItemType.CONVEYORBELT] = require("app.object.entity.SceneItems.SceneItem_ConveyorBelt"),
    [ItemType.AIRWALL] = require("app.object.entity.SceneItems.SceneItem_AirWall"),
}

function SceneItemFactory:CreateById(tid)
    local sceneItemCfg = CfgData:GetSceneItem(tid)
    if not sceneItemCfg then
        print("<color=red><error>SceneItem tid:",tid,"not exist!!!</color>")
        return
    end
    local item = self[sceneItemCfg.monsterType]
    if not item then
        item = commonItem
    end
    return item.new()
end

_G.SceneItemFactory = SceneItemFactory
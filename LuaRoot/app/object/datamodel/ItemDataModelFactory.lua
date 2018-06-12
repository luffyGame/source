local ItemType = Const.ItemType

local ItemDataModelFactory = {
    [ItemType.FOOD] = require("app.object.datamodel.item.ItemDataModel"),
    [ItemType.TREATMENT] = require("app.object.datamodel.item.ItemDataModel"),
    [ItemType.WEAPON] = require("app.object.datamodel.item.WeaponDataModel"),
    [ItemType.ARMOR] = require("app.object.datamodel.item.ArmorDataModel"),
    [ItemType.BUILD] = require("app.object.datamodel.item.BuildDataModel"),
    [ItemType.MATERIAL] = require("app.object.datamodel.item.ItemDataModel"),
}
local CfgData = CfgData

--根据模板id新建数据模型
function ItemDataModelFactory:CreateByTid(tid, new, cfg)
    cfg = cfg or CfgData:GetItem(tid)
    local cls = self[cfg.itemType]
    if not cls then
        return
    end
    local dataModel = cls.new()
    dataModel:SetCfg(cfg)
    if new then
        dataModel:Init(tid)
    end
    return dataModel
end

_G.ItemDataModelFactory = ItemDataModelFactory
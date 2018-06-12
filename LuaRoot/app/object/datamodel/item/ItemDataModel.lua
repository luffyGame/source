local ItemDataModel = class("ItemDataModel",require("app.base.DataModel"))
local _G = _G
local CfgData = _G.CfgData
local ItemType = Const.ItemType

local saveFields = {"tid","count"}
function ItemDataModel:MarkSave()
    self:MarkFieldSave(saveFields)
end

function ItemDataModel:SetCfg(cfg)
    self.cfg = cfg
end

function ItemDataModel:GetItemType()
    return self.cfg.itemType
end

function ItemDataModel:IsBuild()
    return self:GetItemType() == ItemType.BUILD
end

--function ItemDataModel:CanUse()
--    return self:GetItemType() == ItemType.FOOD
--end

function ItemDataModel:Init(tid)
    self:InitId()
    self:SetValue("tid",tid,true)
    self:PostImport()
end

function ItemDataModel:GetIcon()
    return self.cfg.icon
end

function ItemDataModel:GetName()
    return CfgData:GetText(self.cfg.name)
end

function ItemDataModel:OnInit() end

function ItemDataModel:PostImport()
    self:OnInit()
end

function ItemDataModel:GetPileMax()
    return self.cfg.pileUpValue or 1
end

function ItemDataModel:CanPile()
    return self.cfg.pileUpValue and self.cfg.pileUpValue>1
end

function ItemDataModel:SetCount(count)
    self:SetValue("count",count,true)
end

function ItemDataModel:GetCount()
    return self.count
end

return ItemDataModel
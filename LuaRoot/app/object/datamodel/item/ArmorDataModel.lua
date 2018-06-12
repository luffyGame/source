local ArmorDataModel = class("ArmorDataModel",require("app.object.datamodel.item.ItemDataModel"))
local CfgData = CfgData

ArmorDataModel.isArmor = true

local EquipPart = Const.EquipPart

local ArmorEquipDef = {
    HAT = 1,--:帽子
    CLOTH = 2,--:衣服
    PANTS = 3,--:裤子
    SHOES = 4,--:鞋子
    BAG = 5,--:背包
    HEAD = 6,--.头
}
local EquipPartDef = {
    [ArmorEquipDef.HAT] = EquipPart.HAIR,
    [ArmorEquipDef.CLOTH] = EquipPart.CLOTH,
    [ArmorEquipDef.PANTS] = EquipPart.PANTS,
    [ArmorEquipDef.SHOES] = EquipPart.SHOES,
    [ArmorEquipDef.BAG] = EquipPart.BAG,
    [ArmorEquipDef.HEAD] = EquipPart.HEAD,
}
local EquipPos = Const.EquipPos

local saveFields = {"durability"}

function ArmorDataModel:MarkSave()
    ArmorDataModel.super.MarkSave(self)
    self:MarkFieldSave(saveFields)
end

function ArmorDataModel:SetCfg(cfg)
    ArmorDataModel.super.SetCfg(self,cfg)

    self.armorCfg = CfgData:GetArmor(cfg.id)
    if not  self.armorCfg then
        print("<color=red>there is no armor:",cfg.id,"</color>")
        return
    end
    self.modelCfg = CfgData:GetModel(self.armorCfg.resId)
end

function ArmorDataModel:GetEquipPos()
    local armorPat = self.armorCfg.armorPat
    if armorPat == ArmorEquipDef.HEAD then
        return EquipPos.HAT
    end
    return armorPat
end

function ArmorDataModel:GetEquipPart()
    local armorPat = self.armorCfg.armorPat
    return EquipPartDef[armorPat]
end

function ArmorDataModel:GetModel()
    return self.modelCfg.prefab
end

--得到背包的格子数 by SunShubin
function ArmorDataModel:GetPackageValue()
    return self.armorCfg.maxParam
end
function ArmorDataModel:OnInit()
    if self.armorCfg.maxDurability then
        self:SetValue("maxDurability",self.armorCfg.maxDurability,true)
        if not self.durability then
            self:SetValue("durability",self.armorCfg.maxDurability,true)
        end
    end
end

--耐久度
function ArmorDataModel:GetDuration()
    if self.durability then
        return self.durability
    elseif self.maxDurability then
        return self.maxDurability
    else
        return nil
    end
end

function ArmorDataModel:GetMaxDurability()
    return self.maxDurability
end

return ArmorDataModel
local PlayerSlot = class("PlayerSlot", require("app.ui.backpack.SlotBase"))
local LuaUtility = CS.Game.LuaUtility
local Const = Const

PlayerSlot.IsOnBody = true

function PlayerSlot:RegisterEvents()
    self:RegisterDragEvents(self.event, self.OnClick, self.OnBeginDrag, self.OnDrag, self.OnEndDrag, self.OnDrop)
end

function PlayerSlot:Init()
    self:SetChooseState(false)
    LuaUtility.SetComponentEnabled(self.img_puttip,false)
end

function PlayerSlot:SetData(data)
    self.data = data
    LuaUtility.SetComponentEnabled(self.img_icon,data ~= nil)
    LuaUtility.SetComponentEnabled(self.img_default,data == nil)

    if data ~= nil then
        LuaUtility.ImgSetSprite(self.img_icon,data.cfg.icon,true)
    end

end

function PlayerSlot:SetChooseState(state)
    if state then
        LuaUtility.ImgSetColor(self.img_icon,Const.Color.GREEN)
    else
        LuaUtility.ImgSetColor(self.img_icon,Const.Color.WHITE)
    end

end

function PlayerSlot:IsPocket()
    return self.index == Const.EquipPos.POCKET
end

function PlayerSlot:IsWeapon()
    return self.index == Const.EquipPos.WEAPON
end

return PlayerSlot

local PocketSlot = class("PocketSlot", require("app.ui.backpack.SlotBase"))
local LuaUtility = CS.Game.LuaUtility
local Const = Const

function PocketSlot:Init()
    self:SetChooseState(false)
end

function PocketSlot:SetData(data,isClose)
    self.data = data
    self.isClose = isClose
    LuaUtility.SetComponentEnabled(self.txt_counts, data ~= nil)
    LuaUtility.SetComponentEnabled(self.img_icon, data ~= nil)
    if(data ~= nil) then
        LuaUtility.TextSetTxt( self.txt_counts,data.count)
        LuaUtility.ImgSetSprite(self.img_icon,  data.cfg.icon, true)
    else
        self:SetChooseState(false)
    end
        LuaUtility.ImgSetAlpha(self.img_slotBg, 0)
    --if not isClose then
    --    LuaUtility.ImgSetAlpha(self.img_slotBg, 1)
    --
    --else
    --    LuaUtility.ImgSetAlpha(self.img_slotBg, 0.5)
    --end
    LuaUtility.SetComponentEnabled(self.img_closeIcon, isClose)
end

function PocketSlot:SetChooseState(state)
    if state then
        LuaUtility.ImgSetColor(self.img_icon,Const.Color.GREEN)
    else
        LuaUtility.ImgSetColor(self.img_icon,Const.Color.WHITE)
    end
end

function PocketSlot:OnDrop()
    -- print("base drop")
    if self.isClose then
        return
    end
    self.reciever:OnDrop(self)
end


function PocketSlot:SetVisible(visible)
    LuaUtility.SetComponentEnabled(self.txt_counts, visible)
    LuaUtility.SetComponentEnabled(self.img_icon, visible)
end

return PocketSlot

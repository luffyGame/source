local PetCollectSlot = class("PetCollectSlot", require("app.ui.zoon.PetSlotBase"))
local LuaUtility = CS.Game.LuaUtility
local Const = Const

PetCollectSlot.IsOnBody = true

function PetCollectSlot:Init()
    self:SetChooseState(false)
    self:RegisterDragEvent()
end

function PetCollectSlot:SetData(data)
    self.data = data
    LuaUtility.SetComponentEnabled(self.txt_name,data ~= nil)
    LuaUtility.SetComponentEnabled(self.txt_time,data ~= nil)

    if data ~= nil then
        LuaUtility.ImgSetSprite(self.img_icon,"icon_170",true)
		LuaUtility.TextSetTxt(self.txt_name,"erha")
		LuaUtility.TextSetTxt(self.txt_time,"11h11")
	else
		LuaUtility.ImgSetSprite(self.img_icon,"lock",true)
    end

end

function PetCollectSlot:SetChooseState(state)
    if state then
        LuaUtility.ImgSetColor(self.img_icon,Const.Color.GREEN)
    else
        LuaUtility.ImgSetColor(self.img_icon,Const.Color.WHITE)
    end

end

return PetCollectSlot

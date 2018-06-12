local PetCareSlot = class("PetCareSlot", require("app.ui.zoon.PetSlotBase"))
local LuaUtility = CS.Game.LuaUtility
local Const = Const



function PetCareSlot:Init()
    self:SetChooseState(false)
    self:RegisterDragEvent()
end

function PetCareSlot:SetData(data)
    self.data = data
    LuaUtility.SetComponentEnabled(self.txt_name,data ~= nil)

    if data ~= nil then
        LuaUtility.ImgSetSprite(self.img_pet,"icon_170",true)
		LuaUtility.TextSetTxt(self.txt_name,"erha")
	else
		LuaUtility.ImgSetSprite(self.img_pet,"lock",true)
    end

end

function PetCareSlot:SetChooseState(state)
    if state then
        LuaUtility.ImgSetColor(self.img_pet,Const.Color.GREEN)
    else
        LuaUtility.ImgSetColor(self.img_pet,Const.Color.WHITE)
    end

end

function PetCareSlot:IsCarePet()
	if self.index == 1 or self.index == 2 or self.index == 3 then
		return true
	end
end

function PetCareSlot:IsFollowPet()
	if  self.index == 4 then 
		return true
	end
end

return PetCareSlot

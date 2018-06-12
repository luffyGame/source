local PetSkillSlot = class("PetSkillSlot", require("app.ui.zoon.PetSlotBase"))
local LuaUtility = CS.Game.LuaUtility
local Const = Const

function PetSkillSlot:Init()
    self:SetChooseState(false)
	self:RegisterClickEvent()
end

function PetSkillSlot:SetData(data)
    self.data = data
    if data ~= nil then
        LuaUtility.ImgSetSprite(self.img_icon,data.cfg.icon,true)
	else
		LuaUtility.ImgSetSprite(self.img_icon,"lock",true)
    end

end

function PetSkillSlot:SetChooseState(state)
    if state then
        LuaUtility.ImgSetColor(self.img_icon,Const.Color.GREEN)
    else
        LuaUtility.ImgSetColor(self.img_icon,Const.Color.WHITE)
    end

end

function PetSkillSlot:Release()
    self:UnRegisterClickEvent()
end

return PetSkillSlot

local BenchSlot = class("BenchSlot",require("app.ui.workbench.BenchSlotBase"))
local luaUtility = CS.Game.LuaUtility
local Const = _G.Const

local BenchType = {
	STEW = 1002,
	CLOTH = 1012,
	FIREARMS = 1013,
	FERMENT = 1015,
}

function BenchSlot:Init()
	self.IsMaterial = true
	if self.reciever.tid == BenchType.STEW then
		if self.index == 1 then
			self:SetMaterialIcon("icon_water")
		elseif self.index == 2 or self.index == 3 then
			self:SetMaterialIcon("icon_173")
		end
	elseif self.reciever.tid == BenchType.CLOTH then
		self:SetMaterialIcon("icon_water")
	elseif self.reciever.tid == BenchType.FERMENT then
		if self.index == 1 then
			self:SetMaterialIcon("icon_water")
		elseif self.index == 2 then
			self:SetMaterialIcon("icon_170")
		end
	elseif self.reciever.tid == BenchType.FIREARMS then
		self:SetMaterialIcon("icon_water")
	else
		if index % 2 == 0 then
			self:SetMaterialIcon("icon_173")
		else
			self:SetMaterialIcon("icon_water")
		end
	end
	
	
	luaUtility.SetComponentEnabled(self.img_icon, self.data ~= nil)
	
	luaUtility.SetComponentEnabled(self.txt_count,self.data ~= nil)
	
	self:RegisterDrag()
end

function BenchSlot:SetMaterialIcon(icon)   
	luaUtility.ImgSetSprite(self.img_fire,icon, true)
end

function BenchSlot:SetData(data)
	self.data = data

    luaUtility.SetComponentEnabled(self.txt_count, data ~= nil)
    luaUtility.SetComponentEnabled(self.img_icon, data ~= nil)
    if(data ~= nil) then
        luaUtility.TextSetTxt( self.txt_count,data.count)
        luaUtility.ImgSetSprite(self.img_icon,data.cfg.icon, true)
    end
end

function BenchSlot:SetVisible(visible)
    luaUtility.SetComponentEnabled(self.txt_count, visible)
    luaUtility.SetComponentEnabled(self.img_icon, visible)
end

function BenchSlot:SetChooseState(state)
    if state then
        luaUtility.ImgSetColor(self.img_icon,Const.Color.GREEN)
    else
        luaUtility.ImgSetColor(self.img_icon,Const.Color.WHITE)
    end

end

return BenchSlot


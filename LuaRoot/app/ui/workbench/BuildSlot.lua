local BuildSlot = class("BuildSlot",require("app.ui.workbench.BenchSlotBase"))
local luaUtility = CS.Game.LuaUtility
local Const = _G.Const
local isMaterial = false


function BuildSlot:SetData(data)
	self.data = data
	luaUtility.ComponentGameObjVisible(self.com_timer,data ~= nil)
	luaUtility.SetComponentEnabled(self.img_mask,data ~= nil)
	luaUtility.SetComponentEnabled(self.img_build,data ~= nil) 
	luaUtility.SetComponentEnabled(self.txt_count,data ~= nil) 
	--luaUtility.SetComponentEnabled(self.event_slotTemplate,false)
	if data ~= nil then	
		luaUtility.ImgSetSprite(self.img_build,data.cfg.icon,true)
		luaUtility.TextSetTxt(self.txt_count,data.count)
	end
end

function BuildSlot:FinishProduct()
	print("FinishProduct--")
	self:CloseTimer()
	self:RegisterDrag()
end

function BuildSlot:SetTime(loop,cd)
	luaUtility.ImgSetAmount(self.img_mask,loop/cd)
	luaUtility.ImgSetAmount(self.img_timemask,loop/cd)
	local hour = math.floor(loop / 60)
	local minute = math.floor(loop % 60)
	if hour > 0 then
		luaUtility.TextSetTxt(self.txt_time,tostring(hour).."h"..tostring(minute).."m")
	else
		luaUtility.TextSetTxt(self.txt_time,tostring(minute).."m")
	end
end

function BuildSlot:CloseTimer()
	luaUtility.ComponentGameObjVisible(self.com_timer,false)
	luaUtility.SetComponentEnabled(self.img_mask,false)
end

function BuildSlot:SetVisible(visible)
	luaUtility.SetComponentEnabled(self.img_build,visible) 
	luaUtility.SetComponentEnabled(self.txt_count,visible) 
end

function BuildSlot:SetChooseState(state)
    if state then
        luaUtility.ImgSetColor(self.img_build,Const.Color.GREEN)
    else
        luaUtility.ImgSetColor(self.img_build,Const.Color.WHITE)
    end

end

function BuildSlot:SetFeedIcon(icon)
	print("set feed icon:",icon)
	luaUtility.ImgSetSprite(self.img_fire,icon,true)
end

return BuildSlot
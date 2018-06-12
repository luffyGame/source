local FormulaElementSlot = class("FormulaElementSlot")
local luaUtility = CS.Game.LuaUtility
local Const = _G.Const

function FormulaElementSlot:ctor(index,luaItem,receiver)
	luaItem.Data:Inject(self)
	self.index = index
	self.receiver = receiver
end

function FormulaElementSlot:SetData(icon,count)
	luaUtility.SetComponentEnabled(self.img_slot, icon ~= nil)
	luaUtility.SetComponentEnabled(self.txt_count, count ~= nil and count > 1)
	luaUtility.TextSetTxt(self.txt_count,count)
	luaUtility.ImgSetSprite(self.img_slot,icon, true)

end


return FormulaElementSlot
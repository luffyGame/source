local HintSlot = class("HintSlot")
local Manager = WorkBenchManager
local luaUtility = CS.Game.LuaUtility

function HintSlot:ctor(index,luaItem,reciever)
	luaItem.Data:Inject(self)
    self.reciever = reciever
    self.index = index
end

function HintSlot:SetData(icon)
	luaUtility.ImgSetSprite(self.img_element,icon,true)
end

return HintSlot
local PetSlotBase = class("PetSlotBase", require("app.ui.UiHandler"))

function PetSlotBase:Create(index,luaItem, data, reciever)
    self = PetSlotBase.new()

    return self
end

function PetSlotBase:ctor(index,luaItem,storeItemData,reciever)
    luaItem.Data:Inject(self)
	if storeItemData ~= nil then
		self.data = storeItemData.items[index]
	end
	self.owner = storeItemData
    self.reciever = reciever
    self.index = index
end

function PetSlotBase:RegisterDragEvent()
    self:RegisterDragEvents(self.event_slot, self.OnClick, self.OnBeginDrag, self.OnDrag, self.OnEndDrag, self.OnDrop)
end

function PetSlotBase:RegisterClickEvent()
	self:RegisterButtonClick(self.event_slot,self.OnClick)
end

function PetSlotBase:UnRegisterClickEvent()
	self:UnregisterButtonClick(self.event_slot)
end

function PetSlotBase:Init()
	
end

function PetSlotBase:SetData(data)
    self.data = data
end

function PetSlotBase:SetVisible(visible)

end

function PetSlotBase:OnClick()
     --print("base click")
    self.reciever:OnChoosed(self)
end

function PetSlotBase:OnBeginDrag()
    self.reciever:OnBeginDrag(self)
end

function PetSlotBase:OnDrag(x, y)
    -- print("base drag", x, y)
end

function PetSlotBase:OnEndDrag()
    -- print("base enddrag")
    self.reciever:OnEndDrag(self)
end

function PetSlotBase:OnDrop()
    -- print("base drop")
    self.reciever:OnDrop(self)
end

function PetSlotBase:OnLongPress()
	self.reciever:LongPress(self.index,self.event_longPress)
end

function PetSlotBase:OnLongPressUp()
	self.reciever:LongPressUp()
end

function PetSlotBase:Release()

end

function PetSlotBase:SetSign(sign)
	self.sign = sign
end

return PetSlotBase
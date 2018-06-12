local SlotBase = class("SlotBase", require("app.ui.UiHandler"))

function SlotBase:Create(index,luaItem, data, reciever)
    self = SlotBase.new()

    return self
end

function SlotBase:ctor(index,luaItem,storeItemData,reciever)
    luaItem.Data:Inject(self)
    self.data = storeItemData.items[index]
	self.owner = storeItemData
    self.reciever = reciever
    self.index = index
    -- eventHandler对应c#的EventHandler
    self:RegisterEvents()
end

function SlotBase:RegisterEvents()
    self:RegisterDragEvents(self.event_slotTemplate, self.OnClick, self.OnBeginDrag, self.OnDrag, self.OnEndDrag, self.OnDrop)
	if (self.data == nil) then return end
	self:RegisterLongPress(self.event_longPress,self.OnLongPress)
	self:RegisterLongPressCancel(self.event_longPress,self.OnLongPressUp)
end

function SlotBase:UnRegisterEvents()
	print("TestImageShow.....")
end

function SlotBase:Init()
	
end

function SlotBase:SetData(data)
    self.data = data
end

function SlotBase:SetVisible(visible)

end

function SlotBase:OnClick()
     --print("base click")
    self.reciever:OnChoosed(self)
end

function SlotBase:OnBeginDrag()
    self.reciever:OnBeginDrag(self)
end

function SlotBase:OnDrag(x, y)
    -- print("base drag", x, y)
end

function SlotBase:OnEndDrag()
    -- print("base enddrag")
    self.reciever:OnEndDrag(self)
end

function SlotBase:OnDrop()
    -- print("base drop")
    self.reciever:OnDrop(self)
end

function SlotBase:OnLongPress()
	self.reciever:LongPress(self,self.event_longPress)
end

function SlotBase:OnLongPressUp()
	self.reciever:LongPressUp()
end

function SlotBase:IsBenchMaterial()
	return false
end

function SlotBase:Release()
	if (self.data == nil) then return end
	if self.event_longPress then
		self:UnRegisterLongPress(self.event_longPress)
		self:UnRegisterLongPressCancel(self.event_longPress)
	end
	
end

return SlotBase
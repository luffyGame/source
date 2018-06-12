local BenchSlotBase = class("BenchSlotBase", require("app.ui.UiHandler"))

function BenchSlotBase:Create(index,luaItem, data, reciever)
    self = BenchSlotBase.new()

    return self
end

function BenchSlotBase:ctor(index,luaItem,storeItemData,reciever)
    luaItem.Data:Inject(self)
	if index  then
		self.data = storeItemData.items[index]
	end

	self.owner = storeItemData
    self.reciever = reciever
    self.index = index
end

function BenchSlotBase:RegisterDrag()
    self:RegisterDragEvents(self.event_slot, self.OnClick, self.OnBeginDrag, self.OnDrag, self.OnEndDrag, self.OnDrop)
end

function BenchSlotBase:Init()
	
end

function BenchSlotBase:SetVisible(visible)

end

function BenchSlotBase:OnClick()
     --print("base click")
end

function BenchSlotBase:OnBeginDrag()
    self.reciever:OnBeginDrag(self)
end

function BenchSlotBase:OnDrag(x, y)
    -- print("base drag", x, y)
end

function BenchSlotBase:OnEndDrag()
    -- print("base enddrag")
    self.reciever:OnEndDrag(self)
end

function BenchSlotBase:OnDrop()
    -- print("base drop")
    self.reciever:OnDrop(self)
end

function BenchSlotBase:IsBenchMaterial()
	return true
end

function BenchSlotBase:Release()
	print("====")
end

return BenchSlotBase
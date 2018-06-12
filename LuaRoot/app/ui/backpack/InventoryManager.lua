local M = class("InventoryManager")
local PocketSlot = require("app.ui.backpack.PocketSlot")
local PlayerSlot = require("app.ui.backpack.PlayerSlot")
local Const = Const
local ViewManager = ViewManager
local math = math
local pairs = pairs
local Util = _G.Util

M.Events = {
    OnChoosed = "OnChoosed",
    OnBeginDrag = "OnBeginDrag",
    OnDrop = "OnDrop",
}

function M:ctor()
    self.lastItem = nil
end

-- params 1--slot 唯一标识 2--Slot 3--LuaItem 4--StoreItemData
function M:CreatSlot(index, slot, luaItem, storeItemData, isClose)
    local item = slot.new(index, luaItem, storeItemData, self)
    item:SetData(item.data, isClose)
    item:Init()
    return item
end

function M:CreatePocketSlots(index, item)
    local isClose = false
    if index > self.bag.cap then
        isClose = true
    end
    return InventoryManager:CreatSlot(index, PocketSlot, item, self.bag, isClose)
end

--region Box
function M:CreateBoxSlots(index, item)
    local isClose = false
    if index > self.box.cap then
        isClose = true
    end
    return InventoryManager:CreatSlot(index, PocketSlot, item, self.box, isClose)
end

--玩家
function M:CreatePlayerSlots(index, item)
    local tempSlot = InventoryManager:CreatSlot(index, PlayerSlot, item, self.equips)
    if not self.equipSlots then
        self.equipSlots = {}
    end
    self.equipSlots[index] = tempSlot
    return tempSlot
end

function M:InitBoxData(box)
    if box then
        self.box = box
    end

end

function M:ReleaseBoxData()
    self.box = nil
end
--endregion


--region 操作相关事件
function M:OnChoosed(slot)
    if self.lastItem then
        self.lastItem:SetChooseState(false)
    end

    if slot.data then
        slot:SetChooseState(true)
    end

    self:FireEvent("OnChoosed", slot)
    self.lastItem = slot
end

function M:OnBeginDrag(slot)
    local data = slot.data
    if data ~= nil then
        self.dragView:SetSprite(data.cfg.icon)
        slot:SetVisible(false)
    end
    self.dragSlot = slot
end

function M:OnDrop(slot)
	if self.dragSlot:IsBenchMaterial() then
		print("---1",self.dragSlot.owner.count,self.bag.count)		
		self.bag:AddItem(self.dragSlot.data.tid,self.dragSlot.data.count)
		self.dragSlot.owner:Remove(self.dragSlot.data)
		print("---2",self.dragSlot.owner.count,self.bag.count)
    else
		self:CanSwap(self.dragSlot, slot)
        self:Swap(self.dragSlot, slot)
        self:OnChoosed(slot)
    end
end

function M:OnEndDrag(slot)
    if self.dragView then
        self.dragView:SetVisible(false)
    end

    if slot.data then
        slot:SetVisible(true)
    end
end
--endregion

--region 长按 弹起
function M:LongPress(slot, fromTrans)
    if slot.data then
        self.hintView:SetContain(slot.data.cfg.name, fromTrans)
    end
end

function M:LongPressUp(isUp)
    self.hintView:SetVisible(isUp)
end

function M:HideHint()
end
--endregion

--region 工作台
function M:DropOff(drop,fun)
	if drop then 
		if self.dragSlot then
			fun(self.dragSlot)
		end
	end
end

function M:SetDragItem(drag)
	if drag:IsBenchMaterial() then
		self.dragSlot = drag
	end
end
--endregion
function M:Remove()
    self.lastItem.owner:Remove(self.lastItem.data)
    self:OnChoosed(self.lastItem)
end

function M:CanSwap(dragSlot, dropSlot)
    if not dragSlot or not dragSlot.data then
        return false
    end
    if self:_CanSwapWeapon(dragSlot, dropSlot) then
        return true
    end

    if self:_CanSwapArmor(dragSlot, dropSlot) then
        return true
    end

    if self:_CanSwapBag(dragSlot, dropSlot) then
        return true
    end

    if not dragSlot.IsOnBody and not dropSlot.IsOnBody then
        return true
    end

    return false
end

function M:_CanSwapWeapon(dragSlot, dropSlot)
    local dragItem = dragSlot.data
    if not dragItem.isWeapon then
        return false
    end
    local dropItem = dropSlot.data
    if dropItem then
        if dropItem.isWeapon then
            return true
        end
    else
        if (dropSlot.IsPocket and dropSlot:IsPocket()) or (dropSlot.IsWeapon and dropSlot:IsWeapon()) or not dropSlot.IsOnBody then
            return true
        end
    end
    return false
end

function M:_CanSwapArmor(dragSlot, dropSlot)
    local dragItem = dragSlot.data
    if not dragItem.isArmor or dragItem:GetEquipPos() == Const.EquipPos.BAG then
        return false
    end

    local dropItem = dropSlot.data
    if dropItem then
        if dropItem.isArmor and dropItem:GetEquipPos() == dragItem:GetEquipPos() then
            return true
        end
    else
        if not dropSlot.IsOnBody or dropSlot.index == dragItem:GetEquipPos() then
            return true
        end
    end
    return false
end

function M:_CanSwapBag(dragSlot, dropSlot)
    local dragItem = dragSlot.data
    if not dragItem.isArmor or dragItem:GetEquipPos() ~= Const.EquipPos.BAG then
        return false
    end

    local dropItem = dropSlot.data
    print(dropItem)
    if dropItem then
        print(dropItem.isArmor)
        if dropItem.isArmor and dropItem:GetEquipPos() == dragItem:GetEquipPos() then
            if dragSlot.IsOnBody then
                if dropSlot.index < self:GetPocketCap() and self:IsBagEmpty() then
                    return true
                else
                    if self:CanSwapTwoBag(dropItem, dragItem) then
                        return true
                    end
                end
            else
                if self:IsBagEmpty() and dragSlot.index < self:GetPocketCap() then
                    return true
                else
                    if self:CanSwapTwoBag(dragItem, dropItem) then
                        return true
                    end
                end
            end
        end
    else
        if not dropSlot.IsOnBody and dropSlot.index < self:GetPocketCap() and self:IsBagEmpty() or dropSlot.index == dragItem:GetEquipPos() then
            return true
        end
    end
    return false
end

--index1 ---起始index index2--drop时的index
function M:Swap(bgSlot, afSlot)
    local beginData = bgSlot.data
    local afterData = afSlot.data

    if afterData ~= nil and beginData.tid == afterData.tid and
            afterData.count < afterData:GetPileMax() and beginData.count < beginData:GetPileMax() then
        local total = beginData.count + afterData.count
        if total > afterData:GetPileMax() then
            afSlot.data:SetCount(afterData:GetPileMax())
            bgSlot.data:SetCount(total - afterData:GetPileMax())
        else
            afSlot.data:SetCount(total)
            bgSlot.owner:Remove(bgSlot.data)
        end
    else
        if bgSlot.owner == afSlot.owner then
            bgSlot.owner:SetItem(bgSlot.index, afterData)
            afSlot.owner:SetItem(afSlot.index, beginData)
        else
            self:SwapItem(bgSlot.owner, afSlot.owner, bgSlot.index, afSlot.index)
        end
    end
end

function M:SwapItem(pack1, pack2, index1, index2)
    local temp = pack1.items[index1]
    pack1:Remove(temp)
    pack1:MoveItem(index1, pack2.items[index2])
    if pack2.items[index2] then
        pack2:Remove(pack2.items[index2])
    end
    pack2:MoveItem(index2, temp)
end

--region 装备
function M:Equip()
    if not self.lastItem then
        return
    end
    local tempData = self.lastItem.data
    if tempData.isWeapon == nil and tempData:GetEquipPos() == nil then
        return
    end

    if tempData.isWeapon then
        self:Swap(self.lastItem, self.equipSlots[Const.EquipPos.WEAPON])
    elseif tempData.isArmor then
        local equipPos = tempData:GetEquipPos()
        if self:CanSwap(self.lastItem, self.equipSlots[equipPos]) then
            self:Swap(self.lastItem, self.equipSlots[equipPos])
        end
    end
    self:OnChoosed(self.lastItem)
end
--endregion

--region 分开
function M:Split()
    local tempData = self.lastItem.data
    local count = tempData.count
    tempData:SetCount(math.ceil(count / 2))
    self.lastItem.owner:AddItem(tempData.cfg.id, math.floor(count / 2))
    self:OnChoosed(self.lastItem)
end
--endregion

function M:CanSplit()
    if self.lastItem then
        if self.lastItem.IsOnBody then
            return false
        end
        if self.lastItem.owner == self.box then
            return self.box:IsFull() == false
        else
            return self.bag:IsFull() == false
        end
    end
    return false
end

--得到口袋格子数量
function M:GetPocketCap()
    return _G.CfgData:GetSystemParam().initBoxSize
end
--背包格子数量
function M:GetBagCap()
    return self.bag.cap - self:GetPocketCap()
end

--判断有没有背包
function M:HaveBag()
    if self.equips.items[Const.EquipPos.BAG] then
        return true
    end
    return false
end

function M:IsBagEmpty()
    for i, v in pairs(self.bag.idIndexs) do
        if v > 10 then
            return false
        end
    end
    return true
end

function M:CanSwapTwoBag(pack1, pack2)
    if pack1:GetPackageValue() >= pack2:GetPackageValue() then
        return true
    else
        local isEmpty = true
        for i = pack1:GetPackageValue() + 1 + self:GetPocketCap(), pack2:GetPackageValue() + self:GetPocketCap() do
            if self.bag.items[i] then
                isEmpty = false
                break
            end
        end
        return isEmpty
    end
end

--region 穿戴背包
function M:EquipBag()
    print("Equip bag")
    local bagItem = self.equips.items[Const.EquipPos.BAG]
    local newBagCap = self:GetPocketCap()
    local isClose = true
    local capCount = self.bag.cap
    if bagItem then
        newBagCap = newBagCap + bagItem:GetPackageValue()
        isClose = false
        capCount = newBagCap
    end

    for i = self:GetPocketCap() + 1, capCount do
        self.items[i]:SetData(self.bag.items[i], isClose)
    end
    self.bag:SetCap(newBagCap)
end
--endregion

function M:TakeAll(box)
    local left = 0
    for i, v in pairs(box.items) do
        left = self.bag:Merge(v.tid, v.count)
        if left <= 0 then
            box:Remove(v)
        end
    end
end

--region Init
function M:Init()
    local host = self:GetHost()
    self.bag = self:GetHost().dataModel.bag

    self.bag = host.dataModel.bag
    self.equips = host.dataModel.equips

    local bag = self.equips.items[Const.EquipPos.BAG]
    if bag then
        self.bag:SetCap(self:GetPocketCap() + bag:GetPackageValue())
    end

    ViewManager:OpenView(ViewSlotHint)
    ViewManager:OpenView(DragItemView)
    self.dragView = ViewManager:GetView(DragItemView)
    self.hintView = ViewManager:GetView(ViewSlotHint)
end

function M:InitEquip()
    self.equips = self:GetHost().dataModel.equips
end

function M:GetHost()
    if not self.host then
        self.host = _G.HostPlayer
    end
    return self.host
end


--endregion

--region Release
function M:Release()
    ViewManager:CloseView(DragItemView)
    ViewManager:CloseView(ViewSlotHint)

    self.equipSlots = nil
    self.lastItem = nil
    self.dragView = nil
    self.host = nil
    self.bag = nil
    self.equips = nil
end
--endregion

require("framework.EventDispatcher").Extend(M)

InventoryManager = M.new()
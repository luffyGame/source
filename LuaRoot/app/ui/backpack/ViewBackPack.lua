local view = class("ViewBackPack", require("app.ui.UiView"))
local CfgData = CfgData
local LuaUtility = CS.Game.LuaUtility
view.res = "backpack"

view.pocketCount = 10
view.backpackCount = 20

local bag

function view:OnOpen()
    bag = _G.HostPlayer.dataModel.bag
    bag:RegisterIndexNotify(self.ListenBagDataChanged,self)
    self.items = {}
    InventoryManager:Init()
    self:InitItems()
    self:SetText()
    self:OnChoosed(nil)

    self:RegisterButtonClick(self.btn_use, function ()
        InventoryManager:Equip()
    end)

    self:RegisterButtonClick(self.btn_split, function ()
        InventoryManager:Split()
    end)

    self:RegisterButtonClick(self.btn_discard, function ()
        InventoryManager:Remove()
    end)
    InventoryManager:AddEventListener(InventoryManager.Events.OnChoosed,self.OnChoosed,self)

end

function view:OnClose()
    InventoryManager:IsBagEmpty()

    ViewBackPack.super.OnClose(self)
    InventoryManager:RemoveEventListener(InventoryManager.Events.OnChoosed,self.OnChoosed,self)
    self:UnregisterButtonClick(self.btn_use)
    self:UnregisterButtonClick(self.btn_split)
    self:UnregisterButtonClick(self.btn_discard)
    LuaUtility.UnBindOnItemAdd(self.lbp_pocket)
    LuaUtility.UnBindOnItemAdd(self.lbp_backpack)

    for _, v in pairs(self.items) do
        v:Release()
    end

    LuaUtility.SetLuaSimpleListCount(self.lbp_pocket,0)
    LuaUtility.LuaSimpleListInit(self.lbp_pocket)
    LuaUtility.SetLuaSimpleListCount(self.lbp_backpack,0)
    LuaUtility.LuaSimpleListInit(self.lbp_backpack)
    bag:UnregisterIndexNotify(self.ListenBagDataChanged,self)
    InventoryManager:Release()
end

function view:ListenBagDataChanged(index)
    if self.items[index] then
        self.items[index]:SetData(bag.items[index])
    end
end

function view:OnChoosed(slot)
    if not slot then
        self.currentData = nil
    else
        self.currentData = slot.data
    end

    local splitUsable = (self.currentData ~= nil and self.currentData.count > 1) and (not(slot and slot.IsOnBody))
    and InventoryManager:CanSplit()
    local discardUsable = (self.currentData ~= nil) and (not(not slot or slot.IsOnBody))
    local useUsable = self:_GetBtnEnabled(self.currentData) and (not(not slot or slot.IsOnBody))

    LuaUtility.SetBtnExtendInteractable(self.btn_split,(self.currentData ~= nil and self.currentData.count > 1))
    LuaUtility.SetBtnExtendInteractable(self.btn_discard,(self.currentData ~= nil))

    if self.currentData ~= nil  then
        LuaUtility.TextSetTxt(self.txt_use,self:_GetBtnText(self.currentData))
    end

    LuaUtility.SetBtnExtendInteractable(self.btn_split,splitUsable)
    LuaUtility.SetBtnExtendInteractable(self.btn_discard,discardUsable)
    LuaUtility.SetBtnExtendInteractable(self.btn_use,useUsable)

end

function view:InitItems()
    LuaUtility.SetLuaSimpleListCount(self.lbp_pocket,self.pocketCount)
    local itemIndex = 1
    LuaUtility.BindOnItemAdd(self.lbp_pocket, function(item)
        self.items[itemIndex] = InventoryManager:CreatePocketSlots(itemIndex,item)
        itemIndex = itemIndex + 1
    end)
    LuaUtility.LuaSimpleListInit(self.lbp_pocket);
    LuaUtility.SetLuaSimpleListCount(self.lbp_backpack,self.backpackCount)
    LuaUtility.BindOnItemAdd(self.lbp_backpack, function(item)
        self.items[itemIndex] = InventoryManager:CreatePocketSlots(itemIndex,item)
        itemIndex = itemIndex + 1
    end)
    LuaUtility.LuaSimpleListInit(self.lbp_backpack);
end

function view:SetText()
    LuaUtility.TextSetTxt(self.txt_pocket, CfgData:GetText("UIDesc_1105_desc"))
    LuaUtility.TextSetTxt(self.txt_backpack, CfgData:GetText("UIDesc_1106_desc"))
    LuaUtility.TextSetTxt(self.txt_use, CfgData:GetText("UIDesc_1110_desc"))
    LuaUtility.TextSetTxt(self.txt_split, CfgData:GetText("UIDesc_1111_desc"))
    LuaUtility.TextSetTxt(self.txt_discard,CfgData:GetText("UIDesc_1112_desc"))
end

local _BtnTextTable = {
    [3] = CfgData:GetText("UIDesc_1103_desc"),
    [4] = CfgData:GetText("UIDesc_1103_desc"),
    [5] = CfgData:GetText("UIDesc_1110_desc"),
}
function view:_GetBtnText(data)
    if _BtnTextTable[data.cfg.itemType] then
        return _BtnTextTable[data.cfg.itemType]
    end
    return CfgData:GetText("UIDesc_1110_desc")
end

function view:_GetBtnEnabled(data)
    if data == nil then
        return false
    end
    if data.cfg.itemType == 6 or data.cfg.itemType == 7 then
        return false
    else
        return true
    end
end

ViewBackPack = view

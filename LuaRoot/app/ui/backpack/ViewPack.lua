local view = class("ViewPack",require("app.ui.UiView"))
local LuaUtility = CS.Game.LuaUtility
view.res = "viewpack"

function view:OnOpen()
    self.items = {}
    InventoryManager:InitBoxData(self.pack:GetBox())
    self.box = self.pack:GetBox()
    self.count = 30
    LuaUtility.SetLuaSimpleListCount(self.lbp_pack,self.count)
    local itemIndex =  1
    LuaUtility.BindOnItemAdd(self.lbp_pack, function(item)
        self.items[itemIndex] = InventoryManager:CreateBoxSlots(itemIndex,item)
        itemIndex = itemIndex + 1
    end)
    LuaUtility.LuaSimpleListInit(self.lbp_pack);
    LuaUtility.TextSetTxt(self.txt_takeall, CfgData:GetText("UIDesc_1109_desc"))
    LuaUtility.TextSetTxt(self.txt_arrange, CfgData:GetText("UIDesc_1108_desc"))
    self:RegisterButtonClick(self.btn_takeall, self.TakeAll)

    self:RegisterButtonClick(self.btn_arrange, self.Arrange)

    self.box:RegisterIndexNotify(self.ListenBoxDataChanged,self)
end

function view:Init(pack)
    self.pack = pack
end

function view:OnClose()
    ViewMenu.super.OnClose(self)
    self:UnregisterButtonClick(self.btn_takeall)
    self:UnregisterButtonClick(self.btn_arrange)
    for _, v in pairs(self.items) do
        v:Release()
    end
    self.box:UnregisterIndexNotify(self.ListenBoxDataChanged,self)
    LuaUtility.UnBindOnItemAdd(self.lbp_pack)
    LuaUtility.SetLuaSimpleListCount(self.lbp_pack,0)
    LuaUtility.LuaSimpleListInit(self.lbp_pack)
    InventoryManager:ReleaseBoxData()
end

function view:ListenBoxDataChanged(index)
    self.items[index]:SetData(self.box.items[index])
end

function view:TakeAll()
    InventoryManager:TakeAll(self.pack:GetBox())
end

function view:Arrange()
    self.pack:GetBox():Arrange()
end

ViewPack = view
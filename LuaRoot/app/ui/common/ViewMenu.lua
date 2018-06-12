local view = class("ViewMenu",require("app.ui.UiView"))
local LuaUtility = CS.Game.LuaUtility
local CfgData = CfgData
view.res = "viewmenu"

local ViewType = {
    BOX = 1,
    BACKPACK = 2,
    CRAFT = 3,
}

function view:OnOpen()
    LuaUtility.TextSetTxt(self.txt_close, CfgData:GetText("UIDesc_1107_desc"))
    LuaUtility.TextSetTxt(self.txt_bag,CfgData:GetText("UIDesc_1106_desc"))
    LuaUtility.TextSetTxt(self.txt_blueprint,CfgData:GetText("UIDesc_1204_desc"))
    LuaUtility.SetComponentEnabled(self.img_current,self.index == ViewType.BOX)
    self:SetButtonsState(self.index)
    self:RegisterEvents()
end

function view:OnClose()
    self:UnregisterEvents()
    self:CloseOtherViews()
end

function view:RegisterEvents()
    local btns = {self.btn_current,self.btn_bag,self.btn_blueprint}

    for i=1, #btns do
        self:RegisterButtonClick(btns[i],function ()
            print("open",i)
            self:SetButtonsState(i)
            self:OpenOtherViewBy(i)
        end)
    end

    self:RegisterButtonClick(self.btn_close, self.Close)
end

function view:UnregisterEvents()
    self:UnregisterButtonClick(self.btn_bag)
    self:UnregisterButtonClick(self.btn_blueprint)
    self:UnregisterButtonClick(self.btn_current)
    self:UnregisterButtonClick(self.btn_close)
end

function view:SetButtonsState(index)
    LuaUtility.SetBtnExtendInteractable(self.btn_current, index ~= ViewType.BOX)
    LuaUtility.SetBtnExtendInteractable(self.btn_bag, index ~= ViewType.BACKPACK)
    LuaUtility.SetBtnExtendInteractable(self.btn_blueprint, index ~= ViewType.CRAFT)
end

function view:OpenOtherViewBy(index,...)
    self.index = index
    if index == ViewType.BOX then
        self:_OpenBoxView(...)
    elseif index == ViewType.BACKPACK then
        self:_OpenBackPackView()
    elseif index == ViewType.CRAFT then
        self:_OpenBlueprintsView()
    else
        print("nothing")
    end
end

--=======================打开各相关界面

function view:_OpenBackPackView()
    self.uiMnger:CloseView(ViewPack)
    self.uiMnger:CloseView(ViewBluePrints)

    self.uiMnger:OpenView(ViewBackground)
    self.uiMnger:OpenView(ViewBackPack)
    self.uiMnger:OpenView(ViewPlayerShow)
end

function view:_OpenBlueprintsView()
    self.uiMnger:CloseView(ViewPack)
    self.uiMnger:CloseView(ViewBackPack)
    self.uiMnger:CloseView(ViewPlayerShow)

    self.uiMnger:OpenView(ViewBackground)
    self.uiMnger:OpenView(ViewBluePrints)
end

function view:_OpenBoxView(pack)
    self.uiMnger:CloseView(ViewPlayerShow)
    self.uiMnger:CloseView(ViewBluePrints)

    self.uiMnger:OpenView(ViewBackground)
    self.uiMnger:OpenView(ViewBackPack)
    if pack then
        self.pack = pack
    end

    if self.pack then
        self.uiMnger:OpenViewWith(ViewPack,ViewPack.Init,self.pack)
    end
end

function view.OpenBag()
    view.OpenView(ViewType.BACKPACK)
end

function view.OpenCraft()
    view.OpenView(ViewType.CRAFT)
end

function view.OpenBox(pack)
    view.OpenView(ViewType.BOX,pack)
end

function view.OpenView(type,...)
    local view = ViewManager:GetView(view)
    view:OpenOtherViewBy(type,...)
    view:Open()
end

function view:CloseOtherViews()
    if self.pack then
        self.pack:OnBoxClose()
        self.pack = nil
    end

    self.uiMnger:CloseView(ViewBackground)
    self.uiMnger:CloseView(ViewBackPack)
    self.uiMnger:CloseView(ViewPack)
    self.uiMnger:CloseView(ViewPlayerShow)
    self.uiMnger:CloseView(ViewBluePrints)
    self.uiMnger:CloseView(ViewPlayerShow)
	WorkBenchManager:CloseViewWith()
end

--=======================

ViewMenu =  view
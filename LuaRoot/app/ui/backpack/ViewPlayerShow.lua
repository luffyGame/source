local view = class("ViewPlayerShow", require("app.ui.UiView"))
local LuaUtility = CS.Game.LuaUtility
local ViewSprite = require("app.object.view.ViewSprite")
local Const = Const
view.res = "viewplayer"
--view.layer = view.LayerEnum.LAYER_MID
local host, equips

function view:OnOpen()
    InventoryManager:InitEquip()
    host = HostPlayer
    equips = host.dataModel.equips
    local invManger = InventoryManager
    self.items = {}

    self.items[1] = invManger:CreatePlayerSlots(Const.EquipPos.HAT, self.luaitem_head)
    self.items[2] = invManger:CreatePlayerSlots(Const.EquipPos.CLOTH, self.luaitem_coat)
    self.items[3] = invManger:CreatePlayerSlots(Const.EquipPos.PANTS, self.luaitem_trousers)
    self.items[4] = invManger:CreatePlayerSlots(Const.EquipPos.SHOES, self.luaitem_shoes)
    self.items[5] = invManger:CreatePlayerSlots(Const.EquipPos.BAG, self.luaitem_bag)
    self.items[6] = invManger:CreatePlayerSlots(Const.EquipPos.POCKET, self.luaitem_secweapon)
    self.items[7] = invManger:CreatePlayerSlots(Const.EquipPos.WEAPON, self.luaitem_weapon)
    self.items[8] = invManger:CreatePlayerSlots(Const.EquipPos.FOOD,self.luaitem_pocket)

    LuaUtility.BindDragEvents(self.event_dragHandler, nil, function(...)
        self:OnDragBegin(...)
    end, function(...)
        self:OnDrag(...)
    end, function(...)
        self:OnDragEnd(...)
    end, nil)

    self:SetSlotActive(true)
    self:LoadViewPlayer()

    self:Init()
    self:RegisterData()
end

function view:OnDragBegin()

end

function view:OnDrag(x, y)
    LuaUtility.Rotate(self.tran_playerpos, -x)
end

function view:OnDragEnd()

end

function view:OnClose()
    LuaUtility.SetRendererLayer(self.tran_playerpos,Const.Default)
    self:ReleaseViewPlayer()
    self:UnRegisterData()
    for _, v in pairs(self.items) do
        v:Release()
    end
    host = nil
    equips = nil
    self:SetSlotActive(false)
    LuaUtility.BindDragEvents(self.event_dragHandler)
    ViewMenu.super.OnClose(self)
end

function view:Init()
    LuaUtility.TextSetTxt(self.txt_hungry, host.dataModel.hunger)
    LuaUtility.TextSetTxt(self.txt_thirsty, host.dataModel.thirst)
    LuaUtility.TextSetTxt(self.txt_lv, host.dataModel.level)
    LuaUtility.TextSetTxt(self.txt_health, host.dataModel.hp)
    LuaUtility.ImgSetAmount(self.img_lvprocess, host.dataModel:GetExpRate())
end

function view:RegisterData()
    host:RegisterDataNotify("hunger", self.ListenHunger, self)
    host:RegisterDataNotify("thirst", self.ListenThirst, self)
    host:RegisterDataNotify("level", self.ListenLevel, self)
    host:RegisterDataNotify("exp", self.ListenExp, self)
    host:RegisterDataNotify("hp", self.ListenHp, self)

    equips:RegisterIndexNotify(self.ListenBodyDataChanged, self)
end
function view:UnRegisterData()
    host:UnregisterDataNotify("hunger", self.ListenHunger, self)
    host:UnregisterDataNotify("thirst", self.ListenThirst, self)
    host:UnregisterDataNotify("level", self.ListenLevel, self)
    host:UnregisterDataNotify("exp", self.ListenExp, self)
    host:UnregisterDataNotify("hp", self.ListenHp, self)

    equips:UnregisterIndexNotify(self.ListenBodyDataChanged, self)
end

function view:SetSlotActive(active)
    LuaUtility.ComponentGameObjVisible(self.luaitem_head, active);
    LuaUtility.ComponentGameObjVisible(self.luaitem_coat, active);
    LuaUtility.ComponentGameObjVisible(self.luaitem_trousers, active);
    LuaUtility.ComponentGameObjVisible(self.luaitem_shoes, active);
    LuaUtility.ComponentGameObjVisible(self.luaitem_bag, active);
    LuaUtility.ComponentGameObjVisible(self.luaitem_pocket, active);
    LuaUtility.ComponentGameObjVisible(self.luaitem_weapon, active);
    LuaUtility.ComponentGameObjVisible(self.event_dragHandler, active)
end

function view:LoadViewPlayer()
    self.viewPlayer = ViewSprite.new()
    self.viewPlayer:Init(true)
    self.viewPlayer:Load(host.dataModel:GetModel(), self.OnViewPlayerLoaded, self)
end

function view:OnViewPlayerLoaded()
    self.viewPlayer:SetParent(self.tran_playerpos)
    self.viewPlayer:PlayAni("idle")
    self.viewPlayer:ShowEquips(equips.items,function (objBase)
        objBase:SetRendererLayer(Const.UILayer)
    end)
    equips:RegisterIndexNotify(self.OnEquipChanged, self)
    LuaUtility.SetRendererLayer(self.tran_playerpos,Const.UILayer)
end

function view:OnEquipChanged(index)
    local equipData = equips:GetItemByIndex(index)
    self.viewPlayer:Equip(index, equipData,function (objBase)
        objBase:SetRendererLayer(Const.UILayer)
    end)
end

function view:ReleaseViewPlayer()
    if self.viewPlayer then
        equips:UnregisterIndexNotify(self.OnEquipChanged, self)
        self.viewPlayer:Release()
        self.viewPlayer = nil
    end
end

function view:ListenLevel(level)
    LuaUtility.TextSetTxt(self.txt_lv, level)
end

local function RetunTextAndColor(value)
    if value <= 20 then
        return "<color=red>" .. value .. "</color>", Const.Color.RED
    else
        return value, Const.Color.WHITE
    end
end
local function SetValue(value, txt, img)
    local text, color = RetunTextAndColor(value)
    LuaUtility.TextSetTxt(txt, text)
    LuaUtility.ImgSetColor(img, color)
end

function view:ListenHunger(hunger)
    SetValue(hunger, self.txt_hungry, self.img_icon_hunger)
end

function view:ListenThirst(thirst)
    SetValue(thirst, self.txt_thirsty, self.img_icon_thirsty)
end

function view:ListenExp(exp)
    LuaUtility.ImgSetAmount(self.img_lvprocess, host.dataModel:GetExpRate())
end

function view:ListenHp(hp)
    LuaUtility.TextSetTxt(self.txt_health, hp)
    LuaUtility.DoAnimation(self.doanim_icon_life)
    LuaUtility.DoAnimation(self.doanim_health)
end

function view:ListenBodyDataChanged(index)
    print("print index:", index)
    self.items[index]:SetData(equips.items[index])
end

ViewPlayerShow = view
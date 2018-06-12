local ViewMain = class("ViewMain", require("app.ui.UiView"))
ViewMain.res = "dlgmain"
local LuaUtility = CS.Game.LuaUtility
local tostring, format = tostring, string.format
local HostEvent = HostEvent
local host, hostData
local EquipPos = Const.EquipPos
local bind = require("xlua.util").bind
local monsterManger
local getGlobalMap = Global.GetGlobalMap
local Const = _G.Const
local pairs = _G.pairs

--===========================
---region EquipDuration
local DurationPos = {
    HAT = 1,
    CLOTH = 2,
    PANTS = 3,
    SHOES = 4,
    --POCKET = 6,--辅助位
    WEAPON = 7,--主武器位
}

local DurationThreshold = 0.5

local function CanShowDuration(index)
    local equip = hostData:GetEquip(index)
    if equip then
        local equipMaxDuration = equip:GetMaxDurability()
        if equipMaxDuration then
            local duration = equip:GetDuration()
            if duration and duration/equipMaxDuration < DurationThreshold then
                return true
            end
        end
    end
    return false
end
---endregion

local radarShow = {}

function radarShow:Init(radarManager)
    self.radarManager = radarManager
    local x, z = SceneEnv:GetBoderXZ()
    local offsetX, offsetZ = SceneEnv:GetPlayerOffsetXZ(HostPlayer:GetPos())
    LuaUtility.RadarManagerInitBorderRot(self.radarManager, x, z, offsetX, offsetZ)
    host.radar:RegisterCallback(bind(self.RadarIn, self), bind(self.RadarMod, self), bind(self.RadarOut, self), bind(self.RadarOutBorder, self))
end

function radarShow:Release()
    LuaUtility.RadarManagerDestroyRadarItems(self.radarManager)
    self.radarManager = nil
    host.radar:RegisterCallback()
end

function radarShow:RadarOut(oid)
    LuaUtility.RadarManagerRemoveRadar(self.radarManager, oid)
end

function radarShow:RadarOutBorder(distX, distZ)
    LuaUtility.RadarManagerOutBorder(self.radarManager, distX, distZ)
end

function radarShow:RadarIn(oid, info)
    local entity = info.entity
    if entity.isMonster then
        local delta = info.normalDelta
        LuaUtility.RadarManagerAddRadar(self.radarManager, oid, delta.x, delta.z, entity:GetRadarIcon())
    end
end

function radarShow:RadarMod(oid, info, distMod, dirMod, deadMod)
    local entity = info.entity
    if entity.isPlayer then
        if dirMod then
            local dir = info.dir
            LuaUtility.RadarManagerRotatePlayerIcon(self.radarManager, dir.x, dir.z)
        end
    elseif entity.isMonster then
        if distMod or dirMod then
            local delta = info.normalDelta
            local dir = info.dir
            if dir then
                LuaUtility.RadarManagerMoveRadarPos(self.radarManager, oid, delta.x, delta.z, dir.x, dir.z)
            else
                LuaUtility.RadarManagerMoveRadarPos(self.radarManager, oid, delta.x, delta.z, 0, -1)
            end
        end
        if deadMod then
            LuaUtility.RadarManagerChangeIcon(self.radarManager, oid, entity:GetRadarIcon())
        end
    end
end

--===========================

function ViewMain:OnOpen()
    host = HostPlayer
    hostData = host.dataModel
    monsterManger = MonsterManger

    self:SetHostInfo()
    self:OnUseSelect()
    self:ShowEquip()
    self:ShowFood()
    self:ShowSecond()
    self:BindPointerDown(self.atkBut, self.OnAtkPressDown)
    self:BindPointerUp(self.atkBut, self.OnAtkPressUp)
    self:RegisterButtonClick(self.useBut, self.OnUseClick)
    self:RegisterButtonClick(self.assitBtn,self.OnAssitClick)
    self:RegisterButtonClick(self.foodBtn,self.OnFoodClick)
    self:RegisterButtonClick(self.bagBut, self.OnBagBtn)
    self:RegisterButtonClick(self.craftBut, self.OnCraftBtn)
    self:RegisterButtonClick(self.buildBut, self.OnBuildBtn)
    self:RegisterButtonClick(self.sneakBut, self.OnSneakBtn)
    --测试Duration
    self:RegisterButtonClick(self.setBtn, self.OnSetBtn)
    self:ShowDurationEquip()
    self:SetWeaponDuration()
    ---todo,临时写法
    LuaUtility.ComponentGameObjVisible(self.buildBut, getGlobalMap():GetHostEnterScene() == 1)
    host:AddEventListener(HostEvent.USE_SELECT, self.OnUseSelect, self)
    host:AddEventListener(HostEvent.ATK_SELECT, self.OnAtkSelect, self)
    host:RegisterEquipNotify(self.OnEquipChange, self)
    host:RegisterDataNotify("nowHp", self.OnHostHpChange, self)
    host:RegisterSneakNotify(self.SetSneakBtn, self)
    host:RegisterCrawlNotify(self.SetCrawlState,self)
    radarShow:Init(self.radarManager)
end

function ViewMain:SetHostInfo()
    self:OnHostHpChange()
    LuaUtility.TextSetTxt(self.hostLev, format("lv %d", hostData.level))
    self:OnHostExpChange()
    LuaUtility.SingleToggleSetValue(self.autoToggle, hostData.auto)
end

function ViewMain:OnClose()
    radarShow:Release()
    host:RemoveEventListener(HostEvent.USE_SELECT, self.OnUseSelect, self)
    host:RemoveEventListener(HostEvent.ATK_SELECT, self.OnAtkSelect, self)
    host:UnregisterEquipNotify(self.OnEquipChange, self)
    host:UnregisterDataNotify("nowHp", self.OnHostHpChange, self)
    self:UnrigisterTargetHpChanged()
    self:UnbindPointerDown(self.atkBut)
    self:UnbindPointerUp(self.atkBut)
    --weapon
    self:UnregisterButtonClick(self.setBtn)
    self:UnregisterButtonClick(self.useBut)
    self:UnregisterButtonClick(self.assitBtn)
    self:UnregisterButtonClick(self.foodBtn)
    self:UnregisterButtonClick(self.bagBut)
    self:UnregisterButtonClick(self.craftBut)
    self:UnregisterButtonClick(self.sneakBut)
    self:UnregisterButtonClick(self.buildBut)
    host:UnregisterSneaklNotify(self.SetSneakBtn, self)
    host:UnregisterCrawlNotify(self.SetCrawlState,self)
    monsterManger = nil
    host = nil
    hostData = nil
    radarShow.radarManager = nil
end

function ViewMain:OnHostHpChange()
    LuaUtility.TextSetTxt(self.hostHpVal, tostring(hostData:GetNowHp()))
    LuaUtility.SliderSetValue(self.hostHp, hostData:GetHpRate())
end

function ViewMain:OnTargetHpChanged()
    LuaUtility.TextSetTxt(self.targetHpVal, tostring(self.target.dataModel:GetNowHp()))
    LuaUtility.SliderSetValue(self.targetHp, self.target.dataModel:GetHpRate())
end

function ViewMain:RegisterTargetHpChanged()
    self.target:RegisterDataNotify("nowHp", self.OnTargetHpChanged, self)
end

function ViewMain:UnrigisterTargetHpChanged()
    if self.target then
        self.target:UnregisterDataNotify("nowHp", self.OnTargetHpChanged, self)
        self.target = nil
    end
end

function ViewMain:OnHostExpChange()
    LuaUtility.SliderSetValue(self.hostExp, hostData:GetExpRate())
end

function ViewMain:OnAtkPressDown()
    host:Attacking(true)
end

function ViewMain:OnAtkPressUp()
    host:Attacking(false)
end

function ViewMain:OnUseClick()
    host:Use()
end

function ViewMain:OnBagBtn()
    ViewMenu.OpenBag()
end

function ViewMain:OnCraftBtn()
    ViewMenu.OpenCraft()
end

function ViewMain:OnBuildBtn()
    _G.ViewManager:OpenView(ViewBuilding)
end

function ViewMain:OnUseSelect()
    local target = host:GetUseTarget()
    self:SetUseTarget(target)
end

function ViewMain:OnAtkSelect()
    local target = host:GetAtkTarget()
    self:SetAtkTarget(target)
end

function ViewMain:OnAssitClick()
    print("click assit")
    local assitItem = hostData:GetEquip(EquipPos.POCKET)
    if assitItem then
        local itemType = assitItem:GetItemType()
        if itemType == Const.ItemType.WEAPON then
            print("weapon")
            local equips = hostData.equips
            local weapon = hostData:GetEquipedWeapon()
            equips:SetItem(EquipPos.WEAPON,assitItem)
            equips:SetItem(EquipPos.POCKET,weapon)
        elseif itemType == Const.ItemType.FOOD then
            print("food")
        end
    end
end

function ViewMain:OnFoodClick()
    print("click food")
    local foodItem = hostData:GetEquip(EquipPos.FOOD)
    if foodItem then

    end
end

function ViewMain:SetUseTarget(target)
    if target then
        LuaUtility.SetButtonInteractable(self.useBut, true)
        LuaUtility.ImgColorGray(self.useIcon, false)
        LuaUtility.ImgSetSprite(self.useIcon, target:GetUseIcon())
        self.useHint:SetActive(true)
    else
        LuaUtility.SetButtonInteractable(self.useBut, false)
        LuaUtility.ImgColorGray(self.useIcon, true)
        LuaUtility.ImgSetSprite(self.useIcon, "grab")
        self.useHint:SetActive(false)
    end
end

function ViewMain:SetAtkTarget(target)
    self.atkHint:SetActive(target ~= nil)
    self.targetGroup:SetActive(target ~= nil)

    self:UnrigisterTargetHpChanged()
    if target ~= nil then
        self.target = target
        LuaUtility.TextSetTxt(self.targetHpVal, tostring(target.dataModel:GetNowHp()))
        LuaUtility.SliderSetValue(self.targetHp, target.dataModel:GetHpRate())
        self:RegisterTargetHpChanged()
    end
end

--主武器监听Duration变化，其余判断是否显示红色
function ViewMain:OnEquipChange(index)
    if index == EquipPos.WEAPON then
        self:ShowEquip()
        self:SetWeaponDuration()
    elseif index == EquipPos.POCKET then
        self:ShowSecond()
    elseif index == EquipPos.FOOD then
        self:ShowFood()
    end
    self:ShowDurationEquip()
end

function ViewMain:ShowEquip()
    local weapon = hostData:GetEquip(EquipPos.WEAPON)
    LuaUtility.SetComponentEnabled(self.atkDur,weapon ~= nil)
    if weapon then
        LuaUtility.ImgSetSprite(self.atkIcon, weapon:GetIcon())
    else
        LuaUtility.ImgSetSprite(self.atkIcon, "main_attack")
    end
end

function ViewMain:ShowFood()
    local food = hostData:GetEquip(EquipPos.FOOD)
    LuaUtility.SetComponentEnabled(self.foodIcon,food ~= nil)
    LuaUtility.SetComponentEnabled(self.foodNum,food ~= nil)
    if food then
        LuaUtility.ImgSetSprite(self.foodIcon, self:GetIcon())
    end
end

function ViewMain:ShowSecond()
    local second = hostData:GetEquip(EquipPos.POCKET)
    LuaUtility.SetComponentEnabled(self.secondDur,second ~= nil)
    LuaUtility.SetComponentEnabled(self.assitIcon,second ~= nil)
    LuaUtility.SetComponentEnabled(self.assitNum,second ~= nil)
    if second then
        LuaUtility.ImgSetSprite(self.assitIcon, second:GetIcon())
    end
end

function ViewMain:OnSneakBtn()
    local isSneak = host:IsSneak()
    if not isSneak then
        isSneak = true
    else
        isSneak = false
    end
    host:SetSneak(isSneak)
end

function ViewMain:SetSneakBtn(value)
    LuaUtility.SingleToggleSetValue(self.sneakToggle, value)
end

function ViewMain:SetCrawlState(isCrawl)
    print("爬行状态:", isCrawl)
    LuaUtility.SetButtonInteractable(self.useBut, isCrawl ~= true)
    LuaUtility.SetButtonInteractable(self.sneakBut, isCrawl ~= true)
    LuaUtility.SetButtonInteractable(self.autoBut, isCrawl ~= true)
    LuaUtility.SetComponentEnabled(self.atkBut,isCrawl ~= true)
end

---region耐久度
function ViewMain:ShowDurationEquip()
    local showParts
    for _,posIndex in pairs(DurationPos) do
        if CanShowDuration(posIndex) then
            if showParts == nil then showParts = {} end
            showParts[posIndex] = true
        end
    end

    --show/hide DurationImg
    if showParts then
        LuaUtility.ComponentGameObjVisible(self.equipDuration,true)
        for _,posIndex in pairs(DurationPos) do
            LuaUtility.ShowEquipDuration(self.equipDuration,posIndex,showParts[posIndex] and true or false)
        end
    else
        LuaUtility.ComponentGameObjVisible(self.equipDuration,false)
    end
end

--设置武器耐久
function ViewMain:SetWeaponDuration()
    local equip = hostData:GetEquip(EquipPos.WEAPON)
    if equip then
        local equipMaxDuration = equip:GetMaxDurability()
        if equipMaxDuration then
            LuaUtility.ImgSetAmount(self.duration,equip:GetDuration()/equipMaxDuration)
        else
            LuaUtility.ImgSetAmount(self.duration,1)
        end
    end 
end

function ViewMain:OnSetBtn()
    local weapon = hostData:GetEquipedWeapon()
    if weapon then
        weapon:CostOnAtk(1)
    end
end

---region

_G.ViewMain = ViewMain
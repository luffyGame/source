local ViewMapUI = class("ViewMapUI",require("app.ui.UiView"))
ViewMapUI.res = "dlgmapui"
local LuaUtility = CS.Game.LuaUtility
local date = os.date
local Timer = Timer
local floor = math.floor
local getGlobalMap = Global.GetGlobalMap

--===========================
local host = nil
local hostSceneId

function ViewMapUI:OnOpen()
    host = HostPlayer
    hostSceneId = getGlobalMap():GetHostEnterScene()
    self:RegisterButtonClick(self.bagBtn,self.OnBagBtn)
    self:RegisterButtonClick(self.craftBtn,self.OnCraftBtn)
    self:RegisterButtonClick(self.moneyAdd,self.OnAddMoney)
    self:RegisterButtonClick(self.setting, self.ShowSetting)
    self:RegisterButtonClick(self.currentposBtn, self.FocusCurrentPos)
    self:Init()
    self:RegisterData()
end

function ViewMapUI:OnClose()
    self:UnregisterButtonClick(self.bagBtn)
    self:UnregisterButtonClick(self.craftBtn)
    self:UnregisterButtonClick(self.moneyAdd)
    self:UnregisterButtonClick(self.setting)
    self:UnregisterButtonClick(self.currentposBtn)
    self:UnRegisterData()
end

function ViewMapUI:OnBagBtn()
    ViewMenu.OpenBag()
end

function ViewMapUI:OnCraftBtn()
    ViewMenu.OpenCraft()
end

function ViewMapUI:OnAddMoney()
    host.dataModel:AddEnergy(10)
end

function ViewMapUI:ShowSetting()
    _G.ViewManager:OpenView(TestImageShow)
end

local function GetEnergyRate(energy)
    return energy / host.dataModel.maxEnergy
end

function ViewMapUI:FocusCurrentPos()
    local sceneInfo = _G.CfgData:GetScene()
    local marker = _G.SceneEnv.marker
    local focusPos = nil
    if self.isMoving then
        focusPos = marker[self.moveTargetPos]
    else
        focusPos = marker[sceneInfo[hostSceneId].scencePicture]
    end
    _G.SceneEnv:CameraFocus(focusPos)
end

function ViewMapUI:Init()
    LuaUtility.TextSetTxt(self.energyValue, host.dataModel.energy .. "/" .. host.dataModel.maxEnergy)
    LuaUtility.SliderSetValue(self.energySlider,GetEnergyRate(host.dataModel.energy))
    LuaUtility.SetComponentEnabled(self.energyTime, host.dataModel.energy ~= host.dataModel.maxEnergy)
    LuaUtility.SetComponentEnabled(self.movetime, self.isMoving)
    local sceneInfo = _G.CfgData:GetScene()
    LuaUtility.SetComponentEnabled(self.currentposLabel, true)
    LuaUtility.TextSetTxt(self.currentposLabel, "CURRENT LOCATION:")
    LuaUtility.TextSetTxt(self.targetpos, sceneInfo[hostSceneId].scencePicture)
end

function ViewMapUI:RegisterData()
    getGlobalMap():AddMoveTimeListener(self.ListenHostMoveDelta,self)
    getGlobalMap():AddFinishMoveListener(self.FinishHostMove,self)
end

function ViewMapUI:UnRegisterData()
    getGlobalMap():RemoveMoveTimeListener()
    getGlobalMap():RemoveFinishMoveListener()
    getGlobalMap():RemoveHostMoveTime()
end

function ViewMapUI:ListenHostMoveDelta(currentTime)
    LuaUtility.TextSetTxt(self.currentposLabel, "DESTINATION LOCATION:")
    if self.targetPosStr == nil then
        local sceneInfo = _G.CfgData:GetScene()
        local scenePicture = sceneInfo[getGlobalMap():GetTargetScene()].scencePicture
        self.moveTargetPos = scenePicture
        self.isMoving = true
        LuaUtility.TextSetTxt(self.targetpos,self.moveTargetPos)
        LuaUtility.SetComponentEnabled(self.movetime, true)
        self.targetPosStr = scenePicture
    end
    LuaUtility.TextSetTxt(self.movetime, date("%Mm:%Ss", floor(currentTime)))
end

function ViewMapUI:FinishHostMove()
    LuaUtility.TextSetTxt(self.currentposLabel, "CURRENT LOCATION:")
    local sceneInfo = _G.CfgData:GetScene()
    local targetScene = getGlobalMap():GetTargetScene()
    if targetScene then
        local scenePicture = sceneInfo[targetScene].scencePicture
        LuaUtility.TextSetTxt(self.targetpos, scenePicture)
    end
    LuaUtility.SetComponentEnabled(self.movetime, false)
    self.targetPosStr = nil
    self.isMoving = false
    self.moveTargetPos = nil
end

_G.ViewMapUI = ViewMapUI

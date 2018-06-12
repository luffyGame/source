local ViewLoading = class("ViewLoading",require("app.ui.common.ViewLoadingBasic"))
ViewLoading.res = "dlgloading"
local LuaUtility = CS.Game.LuaUtility
local host = nil
function ViewLoading:OnOpen()
    host = HostPlayer
    ViewLoading.super.OnOpen(self)
    LuaUtility.TextSetTxt(self.healthText, host.dataModel.hp)
    LuaUtility.TextSetTxt(self.energyText, host.dataModel.energy)
    LuaUtility.TextSetTxt(self.hungryText, host.dataModel.hunger)
    LuaUtility.TextSetTxt(self.thirstyText, host.dataModel.thirst)
    self:StartLoading(1)
end


_G.ViewLoading = ViewLoading
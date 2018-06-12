local ViewLogin = class("ViewLogin",require("app.ui.common.ViewLoadingBasic"))

ViewLogin.res = "dlglogin"

local Global = Global
local getStorage= Global.GetStorage
local LuaUtility = CS.Game.LuaUtility

function ViewLogin:OnOpen()
	ViewLogin.super.OnOpen(self)
	self:OnGameStart()
end

function ViewLogin:OnClose()
	ViewLogin.super.OnClose(self)
	LuaUtility.DestorySplash()
end

function ViewLogin:OnGameStart()
	self.progressGo:SetActive(true)

	self:StartLoading(2)
end

function ViewLogin:DoLoadStorage(progress)
	local storage = getStorage()
	self.curProgress = progress or 1
	self.curRunning = function() return storage.isLoading end
	storage:Load()
end

function ViewLogin:DoLoad(index)
	if index == 1 then
		self:DoLoadStorage(0.5)
	end
end

function ViewLogin:DoLoadScene(progress)
	--有目标进入大地图
	if Global.GetGlobalMap():GetInMap() then
		Global.GetSceneLoader():LoadMap(false)
	else
		Global.GetSceneLoader():LoadStageScene(Global.GetGlobalMap():GetHostEnterScene(),false)
	end
	ViewLogin.super.DoLoadScene(self,progress)
	end
_G.ViewLogin = ViewLogin
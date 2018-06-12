--场景加载的进度显示的基类，最后一步加载场景

local ViewLoadingBasic = class("ViewLoadingBasic",require("app.ui.UiView"))
ViewLoadingBasic.top = true

local UpdateBeat = UpdateBeat
local LuaUtility = CS.Game.LuaUtility
local insert = table.insert
local getSceneLoader = Global.GetSceneLoader
local bind = require("xlua.util").bind

function ViewLoadingBasic:OnOpen()
	self.value = 0
	LuaUtility.SliderSetValue(self.progress,self.value)
	self.step = 0.01
end

function ViewLoadingBasic:OnClose()
	self.curRunning = nil
	if self.isLoading then
		UpdateBeat:RemoveListener(self.updateHandle)
		self.updateHandle = nil
		getSceneLoader():Finish()
		self.isLoading = false
	end
end

function ViewLoadingBasic:StartLoading(loadCount)
	self.isLoading = true
	self.index = 0
	self.loadCount = loadCount
	self.updateHandle = UpdateBeat:RegisterListener(self.Update,self)
	self:DoNextLoad()
end

function ViewLoadingBasic:DoNextLoad()
	self.index = self.index + 1
	if self.index > self.loadCount then
		self:Close()
	elseif self.index == self.loadCount then
		self:DoLoadScene()
	else
		self:DoLoad(self.index)
	end
end

function ViewLoadingBasic:DoLoad(index) end

function ViewLoadingBasic:DoLoadScene(progress)
	local sceneLoader = getSceneLoader()
	self.curProgress = progress or 1
	self.curRunning = function() return sceneLoader.isLoading end
	sceneLoader:Load()
end

function ViewLoadingBasic:Update(deltaTime)
	self.value = self.value + self.step
	if self.curRunning and self.curRunning() then
		if self.value > self.curProgress-0.1 then
			self.step = 0
		end
	else
		self.step = 0.01
	end
	LuaUtility.SliderSetValue(self.progress,self.value)
	if self.value >= self.curProgress then
		self:DoNextLoad()
	end
end

return ViewLoadingBasic
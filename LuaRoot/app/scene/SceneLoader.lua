local SceneLoader  = {}

local LuaUtility = CS.Game.LuaUtility

local SceneLogin = require("app.scene.entity.SceneLogin")
local SceneStage = require("app.scene.entity.SceneStage")
local SceneMap = require("app.scene.entity.SceneMap")

local _G = _G
_G.SceneLoader = SceneLoader
local getSystem = Global.GetSystem

function SceneLoader:Start(nextScene)
	if self.isLoading or self.nextScene then return end
	self.loadingCount = 1 -- load scene
	self.isLoading = true
	self.nextScene = nextScene
	LuaUtility.MaskInput(true)
	if self.showLoading then
		_G.ViewManager:OpenView(ViewLoading)
	end
end

function SceneLoader:Load()
	_G.SceneManager:SwitchScene(self.nextScene)
end

function SceneLoader:Finish()
	if self.nextScene then
		self.nextScene:Ready()
		self.nextScene = nil
	end
	LuaUtility.MaskInput(false)
	getSystem():EnableTimeTick(true)
end

function SceneLoader:AddOne(res)
	--print("add one",res)
	if not self.isLoading then return end
	self.loadingCount = self.loadingCount + 1
end

function SceneLoader:FinishOne(res)
	--print("finish one",res)
	if not self.isLoading then return end
	self.loadingCount = self.loadingCount - 1
	if self.loadingCount<=0 then
		self.isLoading = false
	end
end

function SceneLoader:LoadLoginScene()
	getSystem():EnableTimeTick(false)
	_G.SceneManager:EnterScene(SceneLogin.new())
end

function SceneLoader:LoadStageScene(sceneId,showLoading)
	getSystem():EnableTimeTick(false)
	self.showLoading = showLoading or (showLoading == nil)
	local scene = SceneStage.new(sceneId)
	self:Start(scene)
end

function SceneLoader:LoadStageByStageId(stageId,showLoading)
	getSystem():EnableTimeTick(false)
	self.showLoading = showLoading or (showLoading == nil)
	local scene = SceneStage.new()
	scene:GenFromStage(stageId)
	self:Start(scene)
end

function SceneLoader:LoadMap(showLoading)
	getSystem():EnableTimeTick(false)
	self.showLoading = showLoading or (showLoading == nil)
	self:Start(SceneMap.new())
end
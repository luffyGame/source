local len,format = string.len,string.format
local _G = _G
local CfgData = _G.CfgData
local StageLocation = StageLocation
local getStageAll = Global.GetStageAll

local SceneStage = class("SceneStage",require("app.scene.entity.Scene"))

function SceneStage:ctor(sceneId)
	if not sceneId then
		return
	end
	self.sceneId = sceneId
	self.sceneData = CfgData:GetScene(sceneId)
	self.stageData = CfgData:GetStage(self.sceneData.stageId)
end

function SceneStage:GenFromStage(stageId)
	self.stageData = CfgData:GetStage(stageId)
end

function SceneStage:GetLevelName()
	return self.stageData and self.stageData.resId
end

function SceneStage:Start()
	SceneStage.super.Start(self)
	getStageAll():EnterStage(self.stageData.id)
	_G.ViewManager:OpenView(ViewPop)
	_G.ViewManager:OpenView(ViewJoyStick)
	_G.ViewManager:OpenView(ViewMain)
end

function SceneStage:Leave()
	_G.ViewManager:CloseView(ViewJoyStick)
	_G.ViewManager:CloseView(ViewMain)
	_G.ViewManager:CloseView(ViewMenu)
	_G.ViewManager:CloseView(ViewPop)
	getStageAll():LeaveStage(self.stageData.id)
end

function SceneStage:Ready()
	SceneStage.super.Ready(self)
	_G.HostPlayer:Ready()
	_G.MonsterManger:Ready()
	_G.EventTrigger:Ready()
end

return SceneStage
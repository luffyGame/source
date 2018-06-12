local SceneManager = {}
local LuaUtility = CS.Game.LuaUtility

function SceneManager:EnterScene(scene)
	self.curScene = scene
	self.curScene:Start()
end

function SceneManager:SwitchScene(scene)
	self:LeaveScene()
	local levelName = scene:GetLevelName()
	local isInBuild = self:IsSceneInBuild(levelName)
	if not isInBuild then
		print(levelName .. " is not in build")
	end
	LuaUtility.LoadScene(scene:GetLevelName(),
		function (progress,done)
			if done then
				print("EnterScene " .. scene:GetLevelName())
				self:EnterScene(scene)
				SceneLoader:FinishOne()
			end	
		end,isInBuild)
end

function SceneManager:LeaveScene()
	if self.curScene then
		self.curScene:Leave()
		self.curScene = nil
	end
end

function SceneManager:GetCurrentScene()
	return self.curScene
end

local buildingScenes
function SceneManager:IsSceneInBuild(levelName)
	if not buildingScenes then
		buildingScenes = {}
		LuaUtility.GetAllBuildScene(buildingScenes)
	end
	return buildingScenes[levelName]
end

_G.SceneManager = SceneManager
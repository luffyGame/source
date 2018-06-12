local Scene = class("Scene")

function Scene:ctor()
	self.isAdded = true
end

function Scene:Start()
	self.isReady = false
end

function Scene:Leave()
end

function Scene:Ready()
	self.isReady = true
end

function Scene:GetLevelName()
end

return Scene
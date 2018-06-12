local SceneLogin = class("SceneLogin",require("app.scene.entity.Scene"))

--[[function SceneLogin:GetLevelName()
	return "Login"
end
]]

function SceneLogin:Start()
	SceneLogin.super.Start(self)
	ViewManager:OpenView(ViewLogin)
	ViewManager:OpenView(ViewConsole)
end

return SceneLogin
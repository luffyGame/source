local LuaUtility = CS.Game.LuaUtility
local bind = require("xlua.util").bind

local v = class("ViewJoyStick",require("app.ui.UiView"))

v.res = "joystick"

local host

function v:OnOpen()
	self.joyStick:Init()
	host = HostPlayer
	LuaUtility.JoystickBind(self.joyStick,bind(self.JoyStickMove,self),bind(self.JoyStickBegin),bind(self.JoyStickEnd))
end

function v:OnClose()
	host = nil
	LuaUtility.JoystickBind(self.joyStick,nil,nil,nil)
end

function v:JoyStickMove(x,z,deltaTime)
	host:Move(x,z,deltaTime)
end

function v:JoyStickBegin()
	-- body
end

function v:JoyStickEnd()
	host:StopMove()
end

ViewJoyStick = v
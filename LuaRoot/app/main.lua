--debug
local breakSocketHandle,debugXpCall = require("LuaDebug")("localhost",7003)
local LuaUtility = CS.Game.LuaUtility
LuaUtility.DebuggerLog("enter main.lua")

--[[local profiler = require("perf.profiler")
profiler.start()
local sum = 0
for i=1,10 do
	sum = sum +i;
end
print(profiler.report())
profiler.stop()]]

require("app.init")

--[[local rapidjson = require('rapidjson')
local t = {}
t.a = 456
t["16"] = 35
local s = rapidjson.encode(t)
print('json:' .. s)
local m = {}
print(tostring(m))]]

function Start()
	Global.GetStorage():Init()
	LuaUtility.CommonResCache(function ()
		SceneLoader:LoadLoginScene()
	end,"all","item")
end

local Timer = Timer
local UpdateBeat = UpdateBeat
function Update(deltaTime,unscaledDeltaTime)
	Timer.SetDeltaTime(deltaTime,unscaledDeltaTime)
	UpdateBeat()
end

function Exit()
	ViewManager:CloseAll()
	SceneManager:LeaveScene()
	Global.GetStorage():Release()
end
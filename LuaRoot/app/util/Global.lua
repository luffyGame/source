---
--- Generated by EmmyLua(https://github.com/EmmyLua)
--- Created by wangliang.
--- DateTime: 2018/5/9 下午12:03
---
local _G = _G
local Global = {}

local _linkedAoi
function Global.GetLinkedAoi()
    if not _linkedAoi then
        _linkedAoi = _G.LinkedAoi
    end
    return _linkedAoi
end

local _blockAoi
function Global.GetBlockAoi()
    if not _blockAoi then
        _blockAoi = _G.BlockAoi
    end
    return _blockAoi
end

local _host
function Global.GetHost()
    if not _host then
        _host = _G.HostPlayer
    end
    return _host
end

local _monsterMnger
function Global.GetMonsterMnger()
    if not _monsterMnger then
        _monsterMnger = _G.MonsterManger
    end
    return _monsterMnger
end

local _sceneItemMnger
function Global.GetSceneItemMnger()
    if not _sceneItemMnger then
        _sceneItemMnger = _G.SceneItemManger
    end
    return _sceneItemMnger
end

local _petManger
function Global.GetPetManager()
    if not _petManger then
        _petManger = _G.PetManager
    end
    return _petManger
end

local _triggerEventTrigger
function Global.GetEventTrigger()
    if not _triggerEventTrigger then
        _triggerEventTrigger = _G.EventTrigger
    end
    return _triggerEventTrigger
end

local _furnitureMgr
function Global.GetFurnitureMnger()
    if not _furnitureMgr then
        _furnitureMgr = _G.FurnitureManager
    end
    return _furnitureMgr
end

local _system
function Global.GetSystem()
    if not _system then
        _system = _G.System
    end
    return _system
end

function Global.GenId()
    return Global.GetSystem():GenId()
end

local _stageAll
function Global.GetStageAll()
    if not _stageAll then
        _stageAll = _G.StageAll
    end
    return _stageAll
end

local _sceneEnv
function Global.GetSceneEnv()
    if not _sceneEnv then
        _sceneEnv = _G.SceneEnv
    end
    return _sceneEnv
end

local _storage
function Global.GetStorage()
    if not _storage then
        _storage = _G.Storage
    end
    return _storage
end

local _sceneLoader
function Global.GetSceneLoader()
    if not _sceneLoader then
        _sceneLoader = _G.SceneLoader
    end
    return _sceneLoader
end

local _globalMap
function Global.GetGlobalMap()
    if not _globalMap then
        _globalMap = _G.GlobalMap
    end
    return _globalMap
end

local _time
function Global.GetTime()
    if not _time then
        _time = Global.GetSystem().time
    end
    return _time
end

local _globalPet
function Global.GetGlobalPet()
	if not _globalPet then
		_globalPet = _G.GlobalPet
	end
	return _globalPet
end

require("framework.EventDispatcher").Extend(Global)

_G.Global = Global
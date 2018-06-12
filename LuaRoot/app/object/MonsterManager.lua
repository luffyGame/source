local Monster = require("app.object.entity.Monster")
local _G = _G
local CfgData =_G.CfgData
local random = math.random
local ipairs,pairs = ipairs,pairs
local tostring = tostring
local Vector3 = _G.Vector3
local BirthType = _G.Const.LocBirthType
local SceneEnv = SceneEnv

local MonsterManger = 
{
    monsters = {}, --id->monster
    monsterTags = {},--id->changed
}
--- ===============local function
local function GetMonsterID(arr_MonsterID,arr_MonsterWeight)
    if #arr_MonsterID == 1 then
        return arr_MonsterID[1]
    end

    local weight = {}
    local tempVal = 0
    for i, v in ipairs(arr_MonsterWeight) do
        tempVal = tempVal + v
        weight[i] = tempVal
    end
    local randomVal = random(0,weight[#weight])
    for i, v in ipairs(weight) do
        if randomVal < v then
            return arr_MonsterID[i]
        end
    end
end

local function getLocGeo(loc)
    local x = loc.pos[1]
    local z = loc.pos[3]
    local pos = Vector3.new(x,0,z)
    if loc.birth == BirthType.CIRCLE then
        local delta = Vector3.new(random(-1,1),0,random(-1,1)):setNormalize()
        pos = pos + delta:mul(loc.radius)
    end
    pos.y = SceneEnv:GetTerrainHeight(pos.x,pos.z)
    local dir = loc.dir
    return pos,Vector3.new(dir[1],dir[2],dir[3])
end

local function getLoc(location,locIndex,locGroup)
    if locGroup then
        return location.monsterGroup[locIndex]
    else
        return location.monsters[locIndex]
    end
end

local function createMonster(tid,locIndex,locGroup)
    local monster = Monster.new()
    monster:Born(tid)
    monster:SetLocInfo(locIndex,locGroup)
    return monster
end

---============================

function MonsterManger:EnterStage(stage)
    self.stage = stage
    local location = stage:GetLocation()
    if stage.monsters then
        self:GenByImport(location,stage.monsters)
    else
        self:GenByLocation(location)
    end
end

function MonsterManger:GenByLocation(location)
    local locs = location.monsters
    if locs then
        for i,loc in ipairs(locs) do
            local monster = createMonster(loc.id,i)
            self:Add(monster,loc,true)
        end
    end
    local mLocs = location.monsterGroup
    if mLocs then
        for i, loc in ipairs(mLocs) do
            local monsterGroupCfg = CfgData:GetMonsterGroup(v.id)
            local tid = GetMonsterID(monsterGroupCfg.monsterId,monsterGroupCfg.monsterWeight)
            local monster = createMonster(tid,i,true)
            self:Add(monster,loc,true)
        end
    end
end

function MonsterManger:GenByImport(location,data)
    if not data then return end
    for _,monsterData in pairs(data) do
        local monster = Monster.new()
        monster:Import(monsterData)
        local locIndex,locGroup = monster:GetLocInfo()
        local loc = getLoc(location,locIndex,locGroup)
        self:Add(monster,loc)
    end
end

function MonsterManger:Release()
    for _,monster in pairs(self.monsters) do
        monster.dataModel:UnregisterBoxChanged(self.OnSceneItemsChanged,self)
        monster:Release()
    end
    self.monsters = {}
    self.monsterTags = {}
    self.stage = nil
end

function MonsterManger:Add(monster,loc,created)
    monster:CacheLoc(loc)
    self.monsters[monster:GetId()] = monster
    local pos,dir = getLocGeo(loc)
    monster:SetPos(pos)
    monster:SetDir(dir)
    monster:EnterStage()
    monster:BindChangeNotify(self.OnMonsterChanged,self)
    monster.dataModel:RegisterBoxChanged(self.OnMonsterChanged,self)
    if created then
        self:OnMonsterChanged(monster.dataModel)
    end
end

function MonsterManger:Remove(monster)
    monster.dataModel:UnregisterBoxChanged(self.OnMonsterChanged,self)
    self.monsters[monster:GetId()] = nil
    monster:Release()
end

function MonsterManger:Ready()
    for _,monster in pairs(self.monsters) do
        monster:Ready()
    end
end

function MonsterManger:GetMonster(id)
    return self.monsters[id]
end

--如果需要输出差异，且有差异值，则返回差异
function MonsterManger:Export(modified)
    local monsterDatas = self.stage.monsters
    if not monsterDatas then
        monsterDatas = {}
    end
    local modDatas
    for id,changed in pairs(self.monsterTags) do
        if changed then
            self.monsterTags[id] = false
            local monster = self.monsters[id]
            local data,mod = monster:Export(modified)
            local idStr = tostring(id)
            monsterDatas[idStr] = data
            if mod then
                if not modDatas then modDatas = {} end
                modDatas[idStr] = mod
            end
        end
    end
    return monsterDatas,modDatas
end

function MonsterManger:OnMonsterChanged(monsterData)
    local id = monsterData.id
    if id then
        self.stage:MarkMonsterDirty()
        self.monsterTags[id] = true
    end
end

_G.MonsterManger = MonsterManger
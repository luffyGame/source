local Util = {}

local _G = _G
local CfgData = _G.CfgData
local ipairs, insert = _G.ipairs, table.insert
local Check10K, random = _G.Math.Chance10K, math.random
local Vector3 = _G.Vector3
local LuaUtility = CS.Game.LuaUtility
local Const = _G.Const

function Util.GenItemList(genlist)
    local ret = {}
    for _, gen in ipairs(genlist) do
        local count = random(gen[2], gen[3])
        for i = 1, count do
            if Check10K(gen[4]) then
                Util.GenItem(gen[1], ret)
            end
        end
    end
    return ret
end

function Util.GenItem(itemGenId, ret)
    local itemGen = CfgData:GetItemGen(itemGenId)
    local rand = random(0, 10000)
    local count = #itemGen
    if count > 0 then
        local min = 0
        for i = 1, count do
            if Util.GenOneItem(itemGen[i], ret, rand, min) then
                break
            end
            min = min + itemGen[i].weight
        end
    else
        Util.GenOneItem(itemGen, ret, rand, 0)
    end
end

function Util.GenOneItem(gen, ret, rand, min)
    if rand < gen.weight + min then
        local count = random(gen.minCount, gen.maxCount)
        if count > 0 then
            insert(ret, { tid = gen.itemId, count = count, gen = gen })
        end
        return true
    end
end

function Util.GetTransformPos(trans)
    if not trans then
        return
    end
    local x, y, z = LuaUtility.TransformGetPos(trans)
    return Vector3.new(x, y, z)
end

function Util.GetTransformDir(trans)
    if not trans then return end
    local x,y,z = LuaUtility.TransformGetDir(trans)
    return Vector3.new(x,y,z)
end

function Util.GetNavMeshPos(pos)
    if not pos then
        return
    end
    local x, y, z = LuaUtility.GetNavMeshPos(pos.x, pos.y, pos.z)
    return Vector3.new(x, y, z)
end

local EquipTable = {
    [Const.EquipPos.HAT] = Const.ItemType.ARMOR,
    [Const.EquipPos.CLOTH] = Const.ItemType.ARMOR,
    [Const.EquipPos.PANTS] = Const.ItemType.ARMOR,
    [Const.EquipPos.SHOES]= Const.ItemType.ARMOR,
    [Const.EquipPos.BAG] = Const.ItemType.ARMOR,
    [Const.EquipPos.WEAPON]= Const.ItemType.WEAPON,
    [Const.EquipPos.POCKET] = CfgData:GetSecondlyTypes(),
    [Const.EquipPos.FOOD] = CfgData:GetSecondlyTypes(),
}
function Util.CanEquip(pos, type, part)
    local equipPos = EquipTable[pos]
    if equipPos then
        if equipPos == type then
            if pos == part then
                return true
            else
                return false
            end
        else
            if equipPos[type] then
                return true
            end
        end
    end
    return false
end

_G.Util = Util
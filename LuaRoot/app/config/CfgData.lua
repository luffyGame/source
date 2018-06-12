local CfgData = require("cfg")
local require, tostring = require, tostring
local insert = table.insert

function CfgData:Init()
    self.location = {}
end

function CfgData:GetMoveRate(rateId)
    if not rateId then
        return self.moveRate
    else
        return self.moveRate[rateId]
    end
end

function CfgData:GetScene(sceneId)
    if not sceneId then
        return self.scence
    else
        return self.scence[sceneId]
    end
end

function CfgData:GetStage(stageId)
    return self.stage[stageId]
end

function CfgData:GetAllStage()
    return self.stage
end

function CfgData:GetText(txt)
    local text = self.text[txt]
    return text and text.value or txt
end

function CfgData:GetSystemParam()
    return self.systemPara[1]
end

function CfgData:GetModel(modelId)
    return self.modelConfig[modelId]
end

function CfgData:GetLevelCfg(level)
    return self.userLevel[level]
end

--根据stageID取得关卡配置
function CfgData:GetLocation(stageId)
    if not self.location[stageId] then
        local location = require(tostring(stageId))
        if not location then
            print("not design leveldata", stageId)
            return
        end
        self.location[stageId] = location.data
    end
    return self.location[stageId]
end

--怪物配置
function CfgData:GetMonster(monsterId)
    if not self.monster[monsterId] then
        print("<color>no monster ID:", monsterId)
        return
    end
    return self.monster[monsterId]
end
--随机怪物组
function CfgData:GetMonsterGroup(monsterGroupID)
    return self.monsterGroup[monsterGroupID]
end
--场景物体
function CfgData:GetSceneItem(tid)
    return self.mapitem[tid]
end

function CfgData:GetMapItemType(typeId)
    return self.mapitemType[typeId]
end

function CfgData:GetItem(tid)
    return self.item[tid]
end

function CfgData:GetWeapon(tid)
    return self.weapon[tid]
end

function CfgData:GetArmor(tid)
    return self.armor[tid]
end

function CfgData:GetAllBlueprints()
    return self.bluePrint
end

function CfgData:GetBlueprint(tid)
    return self.bluePrint[tid]
end

function CfgData:GetItemGen(tid)
    return self.itemGen[tid]
end

---宠物配置表     -by SunShubin
function CfgData:GetPet(tid)
    return self.pet[tid]
end

function CfgData:GetSettingRes(tid)
    return self.settingRes[1][tid]
end

function CfgData:GetSettingBattle()
    return self.settingBattle[1]
end

---主角配置属性
function CfgData:GetProtagonist()
    return self.protagonist[1]
end

function CfgData:GetSkill(tid)
    return self.skill[tid]
end

function CfgData:GetBuff(tid)
    return self.buff[tid]
end

function CfgData:GetFurniture(furnitureId)
    if not furnitureId then
        return self.floor
    else
        return self.floor[furnitureId]
    end
end

---大地图
function CfgData:GetStageTime()
    return self.stageTime
end

function CfgData:GetSecondlyTypes()
    return self:GetSystemParam().Secondly
end

function CfgData:GetBloodVial()
    return self:GetSystemParam().bloodVial
end

--掉落物品随机位置
function CfgData:GetRandomLocationIndex(randomType)
    if not randomType then
        return self.locationRandom
    end
    local randomLocations={}
    for _,location in pairs(self.locationRandom) do
        if location.type == randomType then
            insert(randomLocations,location)
        end
    end
    return randomLocations
end

----workbench
function CfgData:GetWorkBench(tid)
	local temp = {}
	for i,v in ipairs(self.workingTabel) do
		if v.tableType == tid then
			table.insert(temp,v)
		end
	end
	return temp
end

function CfgData:GetAllWorkBench()
	return self.workingTabel
end

function CfgData:GetWorkBenchId()
	local temp = {}
	local count = 0
	for i,v in ipairs(self.workingTabel) do
		if count ~= v.tableType then 
			count = v.tableType
			table.insert(temp,count)
		end
	end
	return temp
end

--region 常用配置表---宠物

function CfgData:GetPetPara()
    return self.petPara[1]
end

function CfgData:GetRadiusABC()
    return self:GetPetPara().radiusABC
end
--宠物区域1
function CfgData:GetAreaA()
    return self:GetRadiusABC()[1]
end
--宠物区域2
function CfgData:GetAreaB()
    return self:GetRadiusABC()[2]
end
--幼体进食速度
function CfgData:GetBabyEatSpd()
    return self:GetPetPara().babyEatSpeed
end
--跟随进食速度
function CfgData:GetFollowEatSpd()
    return self:GetPetPara().followEatSpeed
end
--看家进食速度
function CfgData:GetInHomeEatSpd()
    return self:GetPetPara().watchHomeEatSpeed
end
--食槽限制
function CfgData:GetFoodLimit()
    return self:GetPetPara().foodLimit
end
--重置所需金币
function CfgData:GetResetCost()
    return self:GetPetPara().reset
end


--endregion


CfgData:Init()

_G.CfgData = CfgData
local PetItemDataModel = class("PetItemDataModel",require("app.object.datamodel.actor.ObjectDataModel"))
local CfgData = _G.CfgData
local pairs = pairs
local _G = _G
local tostring,tonumber = tostring,tonumber
local CfgData = _G.CfgData
local ItemType = Const.ItemType
local Camp = Const.Camp
local random = math.random

--petType 1:幼年 2:成年
local saveFields =  {"petId","petType","name","evolveId","evolveFood","grow","level","exp",
					 "attack","defense","hp",
					 "care","follow",
					 "skillCount","skill1","skill2","skill3","skill4"}


function PetItemDataModel:Init()
    self:InitId()
    self:SetValue("petId",self.cfg.id,true)
	self:SetValue("petType",self.cfg.petType,true)
	self:SetValue("name",CfgData:GetText(self.cfg.name),true)
	self:SetValue("evolveId",self.cfg.evolutionId,true)
	self:SetValue("evolveFood",self.cfg.evolutionFood,true)
	self:SetValue("grow",0,true)
	
	local cfg =CfgData:GetMonster(self.cfg.monsterId)
	self.monsterCfg = cfg
	self:SetValue("level",cfg.monsterLevel,true)
	self:SetValue("exp",cfg.exp,true)
	self:SetValue("attack",cfg.atk,true)
	self:SetValue("defense",cfg.def,true)
	self:SetValue("hp",cfg.hp,true)
end

function PetItemDataModel:PostImport()
end

function PetItemDataModel:MarkSave()
    self:MarkFieldSave(saveFields)
end

--------------------------
function PetItemDataModel:SetCfg(cfg)
    self.cfg = cfg

end

function PetItemDataModel:GetId()
    return self.petId
end

function PetItemDataModel:GetName()
    return CfgData:GetText(self.cfg.name)
end

function PetItemDataModel:SetName(name)
	self:SetValue("name",name,true)
end

function PetItemDataModel:GetPetType()
    return self.cfg.petType
end

function PetItemDataModel:SetPetType(petType)
	self:SetValue("petType",petType,true)
end

function PetItemDataModel:GetPetEvolveId()
	return
end

function PetItemDataModel:SetEvolveId(evolveId)
	self:SetValue("evolveId",evolveId,true)
end

function PetItemDataModel:GetIcon()
	local pet = CfgData:GetPet(self.petId)
    return CfgData:GetItem()
end

function PetItemDataModel:GetLevel()
	return self.monsterLevel
end

function PetItemDataModel:SetLevel(level)
	self:SetValue("level",level,true)
end

function PetItemDataModel:GetExp()
	return self.exp
end

function PetItemDataModel:SetExp(exp)
	
end

function PetItemDataModel:GetGrow()
	return self.grow
end

function PetItemDataModel:SetGrow(exp)
	
end

function PetItemDataModel:GetAttack()
	return self.attack
end

function PetItemDataModel:SetAttack(atk)
	self:SetValue("attack",atk,true)
end

function PetItemDataModel:GetDefense()
	return self.defense
end

function PetItemDataModel:GetModel()
	return CfgData:GetModel(self.monsterCfg.resId).prefab
end


function PetItemDataModel:GetCamp()
	return Camp.NEUTRAL
end
--TODO
--有足够的食物
function PetItemDataModel:HaveEnoughFood()
	return true
end
--

function PetItemDataModel:GetFightSpeed()
	return self.monsterCfg.moveFight
end
--得到移动速度（随机）
function PetItemDataModel:GetMoveSpeed()
	local move = self.monsterCfg.move
	move = random(move[1] * 100,move[2] * 100) / 100
	return move
end

function PetItemDataModel:GetOutofBattleSpeed()
	return self.monsterCfg.moveFight * CfgData:GetSettingBattle().goHomeSpeed / 100
end

function PetItemDataModel:SetDefense(defense)
	self:SetValue("defense",defense,true)
end

function PetItemDataModel:GetHp()
	return self.hp
end

function PetItemDataModel:SetHp(hp)
	self:SetValue("hp",hp,true)
end

function PetItemDataModel:GetSkillCount()
	return self.skillCount
end

function PetItemDataModel:AddSkillCount(count)
	if self.skillCount == 4 then 
		return
	elseif self.skillCount + count > 4 then
		return
	else
		self:SetValue("skillCount",self.skillCount + count)
	end
end

function PetItemDataModel:SetSkillCount(count)
	if count > 4 then return end
	self:SetValue("skillCount",count,true)
end

function PetItemDataModel:GetSkill1()
	return self.skill1
end

function PetItemDataModel:SetSkill1(skill)
	self:SetValue("skill1",skill,true)
end

function PetItemDataModel:GetSkill2()
	return self.skill2
end

function PetItemDataModel:SetSkill2(skill)
	self:SetValue("skill2",skill,true)
end

function PetItemDataModel:GetSkill3()
	return self.skill3
end

function PetItemDataModel:SetSkill3(skill)
	self:SetValue("skill3",skill,true)
end

function PetItemDataModel:GetSkill4()
	return self.skill4
end

function PetItemDataModel:SetSkill4(skill)
	self:SetValue("skill4",skill,true)
end


return PetItemDataModel
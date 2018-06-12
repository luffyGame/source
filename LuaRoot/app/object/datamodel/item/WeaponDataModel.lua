local WeaponDataModel = class("WeaponDataModel",require("app.object.datamodel.item.ItemDataModel"))
local Skill = require("app.module.skill.Skill")
local CfgData = CfgData
local EquipPart = Const.EquipPart
local max,random = math.max,math.random
local ipairs,pairs = ipairs,pairs

WeaponDataModel.isWeapon = true

local saveFields = {"durability"}

function WeaponDataModel:MarkSave()
    WeaponDataModel.super.MarkSave(self)
    self:MarkFieldSave(saveFields)
end

function WeaponDataModel:SetCfg(cfg)
    WeaponDataModel.super.SetCfg(self,cfg)
    self.weaponCfg = CfgData:GetWeapon(cfg.id)
    self.modelCfg = CfgData:GetModel(self.weaponCfg.resId)
    self.atkSkillCount = self.weaponCfg.skillId and #self.weaponCfg.skillId or 0
    self.produceSkillCount = self.weaponCfg.productSkillId and #self.weaponCfg.productSkillId or 0
end

function WeaponDataModel:GetEquipPart()
    return EquipPart.WEAPON
end

function WeaponDataModel:OnInit()
    if self.weaponCfg.maxDurability then
        self:SetValue("maxDurability",self.weaponCfg.maxDurability,true)
        if not self.durability then
            self:SetValue("durability",self.weaponCfg.maxDurability,true)
        end
    end
end

--region 武器耐久度
function WeaponDataModel:CostOnAtk(cost)
    if not self.maxDurability then return end
    local dur = max(0,self.durability and self.durability - cost)
    self:SetValue("durability",dur,true)
end

function WeaponDataModel:GetDuration()
    if self.durability then
        return self.durability
    elseif self.maxDurability then
        return self.maxDurability
    else
        return nil
    end
end

function WeaponDataModel:GetMaxDurability()
    return self.maxDurability
end
--endregion

function WeaponDataModel:GetModel()
    return self.modelCfg.prefab
end

function WeaponDataModel:GetActPost()
    return self.weaponCfg.act
end

function WeaponDataModel:GetPower()
    return self.weaponCfg.power
end
--武器噪音范围
function WeaponDataModel:GetNoiseRange()
    if self.weaponCfg.noise then
        return self.weaponCfg.noise
    end
    return 0
end

function WeaponDataModel:GetDmg(isProduce)
    if isProduce then
        return self.weaponCfg.productDamage
    else
        return random(self.weaponCfg.atkMin,self.weaponCfg.atkMax)
    end
end

function WeaponDataModel:GetAtkSkill()
    if self.atkSkillCount == 0 then
        return
    end
    local skillId
    if self.atkSkillCount == 1 then
        skillId = self.weaponCfg.skillId[1]
    else
        skillId = self.weaponCfg.skillId[random(self.atkSkillCount)]
    end
    return self:GetSkill(skillId)
end

function WeaponDataModel:InitSkill()
    self.skills = {}
    self:GenSkills(self.atkSkillCount,self.weaponCfg.skillId)
    self:GenSkills(self.produceSkillCount,self.weaponCfg.productSkillId,true)
end

function WeaponDataModel:GenSkills(skillCount,skillIds,isProduce)
    if skillCount == 0 then return end
    for i=1,skillCount do
        local skill = Skill.new(skillIds[i])
        skill.weapon = self
        skill.isProduce = isProduce
        self.skills[skill.id] = skill
    end
end

function WeaponDataModel:GetProduceSkill(produceSkillId)
    if self.produceSkillCount == 0 then
        return
    end
    local skill = self:GetSkill(produceSkillId)
    if skill and skill.isProduce then return skill end
end

function WeaponDataModel:GetSkill(skillId)
    if not self.skills then
        self:InitSkill()
    end
    return self.skills[skillId]
end

function WeaponDataModel:Update(deltaTime)
    if self.skills then
        for _,skill in pairs(self.skills) do
            skill:Update(deltaTime)
        end
    end
end

return WeaponDataModel


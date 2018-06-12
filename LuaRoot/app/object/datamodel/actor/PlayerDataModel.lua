local PlayerDataModel = class("PlayerDataModel",require("app.object.datamodel.actor.ObjectDataModel"))
extendMethod(PlayerDataModel,require("app.object.datamodel.extends.FightExtend"))

local StoreItemDataModel = require("app.object.datamodel.StoreItemDataModel")
local _G = _G
local EquipPos = _G.Const.EquipPos
local maxPower = _G.Const.MaxPower
local CfgData = _G.CfgData
local max = math.max
local min = math.min
local Timer = Timer
local random = math.random
local Camp = Const.Camp
local pairs = pairs
local insert = table.insert
local getSystem = Global.GetSystem

local saveFields = {"modelId","level","exp",
                    "maxEnergy","energy","maxHunger","hunger","maxThirst","thirst",
                    "maxCleanliness","cleanliness","maxRadiation","radiation","power", "walkNum","stages","blueprints","bluecondition"}

function PlayerDataModel:MarkSave()
    self:MarkFieldSave(saveFields)
    self:MarkFightSave(true)
end

function PlayerDataModel:Init(modelId)
    self:InitId()
    self:SetValue("modelId",modelId,true)
    self:SetValue("level",10,true)
    --todo:每天定时分配体力值; 定时清空行走数
    self:SetValue("power", maxPower)
    self:SetValue("walkNum", 0)
    self.bag = StoreItemDataModel.new()
    self.equips = StoreItemDataModel.new()
    self.equips:SetCap(8)
    self:PostImport()
    self:Born()
end

function PlayerDataModel:Export(modified)
    local data,mod = PlayerDataModel.super.Export(self,modified)
    if self.bag:IsDirty() then
        local bag,bagMod = self.bag:Export(modified)
        data.bag = bag
        if mod or bagMod then
            mod = mod or {}
            mod.bag = bagMod
        end
    end
    if self.equips:IsDirty() then
        local equip,equipMod = self.equips:Export(modified)
        data.equip = equip
        if mod or equipMod then
            mod = mod or {}
            mod.equip = equipMod
        end
    end
    return data,mod
end

function PlayerDataModel:Import(data)
    PlayerDataModel.super.Import(self,data)
    self.bag = StoreItemDataModel.new()
    if data.bag then
        self.bag:Import(data.bag)
    end
    self.equips = StoreItemDataModel.new()
    if data.equip then
        self.equips:Import(data.equip)
    end
end

function PlayerDataModel:PostImport()
    self.modelCfg = CfgData:GetModel(self.modelId)
    self.levelCfg = CfgData:GetLevelCfg(self.level)
    self.baseProps = CfgData:GetProtagonist()
end

function PlayerDataModel:GetBaseProp(prop)
    return self.baseProps[prop]
end

function PlayerDataModel:Born()
    self:ResetProp(true)
    --能量
    self:SetValue("maxEnergy",self.levelCfg.maxEnergy,true)
    --self:SetValue("energy",self.maxEnergy)
    self:SetValue("energy",50)
    --饥饿
    self:SetValue("maxHunger",self.levelCfg.maxHunger,true)
    self:SetValue("hunger",self.maxHunger)
    --口渴
    self:SetValue("maxThirst",self.levelCfg.maxThirst,true)
    self:SetValue("thirst",self.maxThirst)
    --清洁
    self:SetValue("maxCleanliness",self.levelCfg.maxCleanliness,true)
    self:SetValue("cleanliness",self.maxCleanliness)
    --辐射
    self:SetValue("maxRadiation",self.levelCfg.maxRadiation,true)
    self:SetValue("radiation",self.maxRadiation)

    --健康值
    self:SetValue("health",self.levelCfg.maxhealth)
    self:SetValue("maxHealth",self.levelCfg.maxhealth)

    --尿意值

    --

    --

    self.bag:SetCap(_G.CfgData:GetSystemParam().initBoxSize)

    self:SetValue("exp",0,true)
    self:SetValue("dead",false,true)
    self:SetBornFlag(true)

    self:AutoConsumeThirst()
    self:AutoConsumeHunger()
    if self.energy < self.maxEnergy then
        self:AutoAddEnergy()
    end
end

function PlayerDataModel:Update(deltaTime)
    self:UpdateFight(deltaTime)
    self.bag:Update(deltaTime)
    self.equips:Update(deltaTime)
end

function PlayerDataModel:GetLevel()
    return self.level
end

function PlayerDataModel:GetExpRate()
    return self.exp/self.levelCfg.levelupExp
end

function PlayerDataModel:SetBornFlag(bBorn)
    self.bornFlag = bBorn
end

function PlayerDataModel:AddItem(tid,num)
    return self.bag:Merge(tid,num)
end

function PlayerDataModel:GetProduceSkillFor(sceneItemDataModel)
    local produceSkillIds = sceneItemDataModel:GetProduceSkillId()
    if not produceSkillIds then return end
    local skill = self.equips:GetProduceSkillFrom(produceSkillIds)
    if not skill then
        skill = self.bag:GetProduceSkillFrom(produceSkillIds)
    end
    return skill
end

function PlayerDataModel:GetEquipedWeapon()
    return self:GetEquip(EquipPos.WEAPON)
end

function PlayerDataModel:GetAtkSkill()
    local weapon = self:GetEquipedWeapon()
    return weapon and weapon:GetAtkSkill() or self:GetBaseSkill()
end

function PlayerDataModel:GetEquip(equipPos)
    return self.equips.items[equipPos]
end

function PlayerDataModel:GetBaseSkill()
    local skillIds = self.baseProps.baseSkill
    if skillIds then
        return self:GetSkill(random(#skillIds))
    end
end

function PlayerDataModel:GetBagBuildItems()
    local items = self.bag.items
    local buildItems
    for _,v in pairs(items) do
        if v.isBuild then
            if not buildItems then buildItems = {} end
            insert(buildItems,v)
        end
    end
    return buildItems
end

function PlayerDataModel:GetCamp()
    return Camp.PLAYER
end

--region 玩家属性变化
function PlayerDataModel:AddExp(exp)
    local tempExp = self.exp + exp
    if tempExp >= self.levelCfg.levelupExp then
        tempExp = tempExp - self.levelCfg.levelupExp
        self:LevelUp()
    end
    self:SetValue("exp",tempExp)
end

function PlayerDataModel:LevelUp()
    self:SetValue("level",min(self.level + 1,#CfgData.userLevel))

end

function PlayerDataModel:AddWalkNum(num)
    self:SetValue("walkNum", self.walkNum + num)
end

function PlayerDataModel:MinusWalkNum(num)
    if num > self.walkNum then
        return false
    end

    self:SetValue("walkNum",self.walkNum - num)
    return true
end

--每日重置
function PlayerDataModel:GetPower()
    if getSystem():GetDayDeltaTime() > 0 then
        self:SetValue("power",maxPower)
    end
    return self.power
end

function PlayerDataModel:AddPower(power)
    self:SetValue("power", min(self.power + power, maxPower))
end

function PlayerDataModel:CosumePower(power)
    if power > self.power then
        return false
    end

    self:SetValue("power",self.power - power)
    return true
end

function PlayerDataModel:AddEnergy(energy)
    self:SetValue("energy",min(self.energy + energy,self.maxEnergy))
    if self.energy == self.maxEnergy then
        --todo 能量已满，暂停
        --Timer:DeleteTimer("energy")
    end
end

function PlayerDataModel:ConsumeEnergy(energy)
    if energy > self.energy then
        return false
    end

    if self.energy == self.maxEnergy then
        self:AutoAddEnergy()
    end
    self:SetValue("energy",self.energy - energy)
    return true
end

function PlayerDataModel:AddThirst(thirst)
    self:SetValue("thirst",min(self.thirst + thirst,self.maxThirst))
end

function PlayerDataModel:AddHunger(hunger)
    self:SetValue("hunger",min(self.hunger + hunger,self.maxHunger))
end

function PlayerDataModel:AutoConsumeThirst()
    ---todo
    --[[
    Timer:AddLoopTimer("thirst",0,CfgData:GetSettingRes("lowThirstSpeed"),function (...)
        local newThirst = max(0,self.thirst - 1)
        self:SetValue("thirst",newThirst)
    end )
    ]]
end

function PlayerDataModel:AutoConsumeHunger()
    ---todo
    --[[
    Timer:AddLoopTimer("hunger",0,CfgData:GetSettingRes("lowHungerSpeed"),function (...)
        local newHunger = max(0,self.hunger - 1)
        self:SetValue("hunger",newHunger)
    end )
    ]]
end


function PlayerDataModel:AutoAddEnergy()
    ---todo
    --[[
    self.lastAddEnergyTime = Timer:GetCurrentTime()
    Timer:AddFixedLoopTimer("energy",self.lastAddEnergyTime,CfgData:GetSettingRes("addEnergySpeed") ,function (...)
        self:AddEnergy(1)
    end):SetDeltaCallBack(function (time)
        self:ListenEnergyDelta(time)
    end)
    ]]
end

function PlayerDataModel:ListenEnergyDelta(time)
    if self.energyTimeListener then
        self.energyTimeListener(self.energyTimeListenerOwner, time)
    end
end

function PlayerDataModel:AddEnergyTimeListener(listener,owner)
    self.energyTimeListenerOwner = owner
    self.energyTimeListener = listener
end

function PlayerDataModel:RemoveEnergyTimeListener()
    self.energyTimeListener = nil
end

--暂停饥饿和口渴消耗
function PlayerDataModel:PauseConsume()
    --todo
    --[[
    Timer:PauseTimer("thirst")
    Timer:PauseTimer("hunger")
    ]]
end
--恢复饥饿和口渴消耗
function PlayerDataModel:ResumeConsume()
    --todo
    --[[
    Timer:ResumeTimer("thirst")
    Timer:ResumeTimer("hunger")
    ]]
end

--region 宠物相关
--注册出战出战宠物
function PlayerDataModel:RegisterFightPetChanged(callBack)
    self:RegisterFieldNotify("fightPet",callBack,self)
end

function PlayerDataModel:UnregisterFightPetChanged(callBack)
    self:UnregisterFieldNotify("fightPet",callBack,self)
end

--TODO  得到出战宠物 现在是临时数据
function PlayerDataModel:GetFightPet()
    if not self.fightPet then
        local petData = require("app.object.datamodel.zoon.PetItemDataModel")
        local tempPetData = petData.new()
        tempPetData:SetCfg(CfgData:GetPet(3))
        tempPetData:Init()
        self.fightPet = tempPetData
    end
    return self.fightPet
end
--TODO  设置出战宠物
function PlayerDataModel:SetFightPet(petData)
    self:SetValue("fightPet",petData)
end
--endregion

--region 记录蓝图的相关信息
--已经查看过了这个Item
function PlayerDataModel:HaveSeedItem(tid)
    return true
end
--达成了图纸条件
function PlayerDataModel:IsReachCondition(tid)
    return true
end
--endregion

--region 爬行相关
function PlayerDataModel:IsCrawl()
    return self.isCrawl
end

function PlayerDataModel:SetCrawl(isCrawl)
    self:SetValue("isCrawl",isCrawl)
end

--endregion

--region 潜行相关
function PlayerDataModel:IsSneak()
    return self.isSneak
end

function PlayerDataModel:SetSneak(isSneak)
    self:SetValue("isSneak",isSneak)
end

--endregion

--跑步速度
function PlayerDataModel:GetRunSpeed()
    return CfgData:GetSettingBattle().runspeed
end
--潜行速度
function PlayerDataModel:GetSneakSpeed()
    return CfgData:GetSettingBattle().sneakspeed
end
--爬行速度
function PlayerDataModel:GetCrawlSpeed()
    return CfgData:GetSettingBattle().crawlspeed
end

--endregion

return PlayerDataModel
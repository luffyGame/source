local Player = class("Player",require("app.object.entity.Object"))
extendMethod(Player,require("app.object.entity.extends.MoverExtend"))
extendMethod(Player,require("app.object.entity.extends.FighterExtend"))
Player.isPlayer = true

local PlayerDataModel = require("app.object.datamodel.actor.PlayerDataModel")
local EquipPos = Const.EquipPos
local ViewSprite = require("app.object.view.ViewSprite")

local PopHpType = Const.PopHpType
local Timer = Timer

local PlayerActor = require("app.object.controller.player.PlayerActor")

function Player:ctor()
	Player.super.ctor(self)
	self.actor = PlayerActor.new(self)
end

function Player:Load()
	if self.view then return end
	self.view = ViewSprite.new(self)
	self.view:Init(true)
	self.view:Load(self.dataModel:GetModel(),self.OnLoaded,self)
end

function Player:OnLoaded()
	Player.super.OnLoaded(self)
	self.view:ShowEquips(self.dataModel.equips.items)
end

--出生时的加载
function Player:Born(modelId)
	self.dataModel = PlayerDataModel.new()
	self.dataModel:Init(modelId)
	self:PostImport()
end

function Player:Export(modified)
	if self.dataModel then
		return self.dataModel:Export(modified)
	end
end

function Player:Import(data)
	self.dataModel = PlayerDataModel.new()
	self.dataModel:Import(data)
	self:PostImport()
end

function Player:PostImport()
	self:RegisterEquipNotify(self.OnEquipChanged,self)
end

function Player:Release()
	self.viewWeaponData = nil
	self:EnableUpdate(false)
	self:ReleaseMover()
	Player.super.Release(self)
end

function Player:Ready()
	self:EnableUpdate(true)
	self.actor:Start()
end

function Player:Update()
	local deltaTime = Timer.deltaTime
	self.dataModel:Update(deltaTime)
	if self.actor then
		self.actor:OnUpdate(deltaTime)
	end
	self:ResetNoise()
end

function Player:Idle()
	self.actor:Idle()
end

function Player:Dead()
	self:CancelFollow()
	self.actor:Dead()
end

function Player:Relive()
	self.dataModel:Relive()
	self:Idle()
end

function Player:CanAtk()
	return self.actor:CanAtk()
end

function Player:GetActionPost()
	return self.viewWeaponData and self.viewWeaponData:GetActPost()
end

function Player:GetSneakAnim(anim)
	local tempAnim = anim
	if self:IsSneak() then
		if tempAnim then
			tempAnim = "sneak_" .. tempAnim
		else
			tempAnim = "sneak"
		end
	end
	return tempAnim
end

function Player:CanUse()
	return self.actor:CanUse()
end

function Player:ActSkill(act)
	self.actor:Skill(act)
end

function Player:RegisterEquipNotify(func,obj)
	self.dataModel.equips:RegisterIndexNotify(func,obj)
end

function Player:UnregisterEquipNotify(func,obj)
	self.dataModel.equips:UnregisterIndexNotify(func,obj)
end

function Player:RegisterBagNotify(func,obj)
	self.dataModel.bag:RegisterIndexNotify(func,obj)
end

function Player:UnregisterBagNotify(func,obj)
	self.dataModel.bag:UnregisterIndexNotify(func,obj)
end


function Player:OnEquipChanged(index)
	local equipData = self.dataModel.equips:GetItemByIndex(index)
	if index == EquipPos.WEAPON then
		self:ViewWeapon(equipData)
	end
	if self.view then
		self.view:Equip(index,equipData)
	end
end

function Player:ViewWeapon(equipData)
	if self.viewWeaponData == equipData then return end
	self.viewWeaponData = equipData
	if self.view then
		self.view:Equip(EquipPos.WEAPON,equipData)
	end
	if self.actor then
		self.actor:OnWeaponEquipped()
	end
end

function Player:ResetViewWeapon()
	self:ViewWeapon(self.dataModel:GetEquipedWeapon())
end
function Player:CastSkill(skill,target)
	skill:Perform(self,target,self:IsSneak())
end

function Player:PlayDmgPop(dmg)
	self:PopHpDmg(PopHpType.PLAYER_DMG,dmg)
end

function Player:BindUseChecker(onUseSelect)
	if self.view then
		self.view:BindUseChecker(onUseSelect)
	end
end

return Player
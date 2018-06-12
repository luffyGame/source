local ViewSprite = class("ViewSprite",require("app.object.view.ViewObject"))

local LuaUtility = CS.Game.LuaUtility
local pairs = pairs
local Util = Util
local Vector3 = Vector3
local Const = Const
local EquipPos,EquipPart,EquipPos2Part = Const.EquipPos,Const.EquipPart,Const.EquipPos2Part

function ViewSprite:Init(isRole)
	self.isRole = isRole
end

--======resource
function ViewSprite:Load(res,cb,caller)
	self:DoLoad(res,LuaUtility.LoadSprite,cb,caller)
end

function ViewSprite:GetSprite()
	return self.view
end

function ViewSprite:Release()
	self:ReleaseEquips()
	ViewSprite.super.Release(self)
end

function ViewSprite:SimpleMove(speed)
	if self.view and not speed:isZero() then
		self.view:SimpleMove(speed.x,speed.z)
	end
end

---============追踪使用接口
--开始追踪，目标位置，离目标位置的范围
function ViewSprite:FollowPos(pos,range)
	if self.view then
		self.view:StartFollow(pos.x,pos.y,pos.z,range)
	end
end

function ViewSprite:FollowEntity(viewObj,range)
	if self.view and viewObj.view then
		self.view:StartFollow(viewObj.view,range)
	end
end

function ViewSprite:FollowEntityOffset(viewObj,offset)
	if not offset then
		self:FollowEntity(viewObj,0)
	elseif self.view then
		self.view:StartFollowOffset(viewObj.view,offset.x,offset.y,offset.z)
	end
end

function ViewSprite:FollowInRandomDirection(use)
	if self.view then
		self.view:FollowInRandomDirection(use)
	end
end

function ViewSprite:CancelFollow()
	if self.view then
		self.view:CancelFollow()
	end
end

function ViewSprite:SetSpeed(speed)
	if self.view then
		self.view:SetSpeed(speed)
	end
end

function ViewSprite:BindFollowCallback(onPosSet,onDirSet,onComplete)
	if self.view then
		self.view:BindFollowCallback(onPosSet,onDirSet,onComplete)
	end
end
---==================


function ViewSprite:Alive(alivable)
	if self.view then
		self.view:Alive(alivable)
	end
end

function ViewSprite:HasAni(ani)
	if self.view then
		return self.view:HasAni(ani)
	end
end

function ViewSprite:HasDieAni()
	return self:HasAni("death")
end

function ViewSprite:PlayAni(ani,fade)
	if self.view then
		if fade then
			return self.view:PlayAni(ani,fade)
		else
			return self.view:PlayAni(ani)
		end
	end
end

---=========死亡表现
function ViewSprite:Dismember(part,power,other,dir)
	self.view:Dismember(part,power,other,dir.x,dir.y,dir.z)
end

function ViewSprite:Ragdoll(part,power,dir)
	if self.view then
		self.view:Ragdoll(part,power,dir.x,dir.y,dir.z)
	end
end


function ViewSprite:RecoverFromDeathShow()
	if self.view then
		self.view:RecoverFromDeathShow()
	end
end

--==============装备相关-----------
function ViewSprite:ShowEquips(equipDatas,cb)
	for epos,equipData in pairs(equipDatas) do
		if equipData then
			self:Equip(epos,equipData,cb)
		end
	end
end

function ViewSprite:ReleaseEquips()
	if self.view then
		self.view:WearAllRemove()
	end
end

--equipData == nil的时候相当于释放
function ViewSprite:Equip(equipPos,equipData,cb)
	if equipPos == EquipPos.POCKET then
		return
	end
	if equipData then
		self:WearOn(equipData,cb)
	else
		if equipPos == EquipPos.HAT then
			self:WearOff2(EquipPart.HEAD,EquipPart.HAIR)
		else
			local equipPart = EquipPos2Part[equipPos]
			self:WearOff(equipPart)
		end
	end
end

function ViewSprite:WearOn(equipData,cb)
	if self.view then
		self.view:WearOn(equipData:GetEquipPart(),equipData:GetModel(),cb)
	end
end

function ViewSprite:WearOff(equipPart)
	if self.view then
		self.view:WearOff(equipPart)
	end
end

function ViewSprite:WearOff2(equipPart1,equipPart2)
	if self.view then
		self.view:WearOff(equipPart1,equipPart2)
	end
end

function ViewSprite:GetWeaponFirePos()
	local mountTrans = self.view:GetWeaponFireMount()
	return Util.GetTransformPos(mountTrans)
end

function ViewSprite:PlayWeaponEffect(res,mount,mounted)
	if self.view then
		self.view:PlayWeaponEffect(res,mount,mounted)
	end
end

function ViewSprite:PlayEffect(res)
	if self.view then
		self.view:PlayTimedEffectAtPoint(res)
	end
end

--mounted为false时，只是在该挂点播放
function ViewSprite:PlayMountEffect(res,mount,mounted)
	if self.view then
		self.view:PlayTimedEffectAtMount(res,mount,mounted)
	end
end

function ViewSprite:GetMountPos(mmount)
	if self.view then
		local mountT = self.view:GetMount(mmount)
		if mountT then
			return Util.GetTransformPos(mountT)
		end
	end
end

function ViewSprite:CanSkillReachPos(pos,skillRange)
	if self.view then
		local canReach,x,y,z = self.view:CanSkillReach(pos.x,pos.y,pos.z,skillRange)
		return canReach,Vector3.new(x,y,z)
	end
end

function ViewSprite:CanSkillReachEntity(entity,skillRange)
	if self.view and entity.view then
		local canReach,x,y,z = self.view:CanSkillReach(entity.view.view,skillRange)
		return canReach,Vector3.new(x,y,z)
	end
end

function ViewSprite:GetSkillReachPos(dir,skillRange)
	if self.view then
		local x,y,z = self.view:GetSkillReach(dir.x,dir.y,dir.z,skillRange)
		return Vector3.new(x,y,z)
	end
end

function ViewSprite:BindUseChecker(onTargetSet)
	if self.view then
		self.view:BindUseChecker(onTargetSet)
	end
end

function ViewSprite:SetUsable(usable)
    if self.view then
        self.view:SetUsable(usable)
    end
end

return ViewSprite
local Object = class("Object")
Object.isEntity = true
require("framework.EventDispatcher").Extend(Object)

local objCount = 0
local _G = _G
local linkedAoi
local blockAoi
local UpdateBeat = UpdateBeat
local Vector3 = _G.Vector3
local abs = math.abs
local ObjEvent = ObjEvent

function Object:ctor()
	self.dataModel = nil
	self.objId = objCount
	objCount = objCount + 1
end

function Object:GetId()
	return self.dataModel.id
end

function Object:Release()
	if self.view then
		self.view:Release()
		self.view = nil
	end
	self:UnregisterAoi()
	self:FireEvent(ObjEvent.OBJ_RELEASE)
end

function Object:OnLoaded()
	self:SetObjInfo()
	self:SyncPos()
	self:SyncDir()
	self:SyncScale()
end

function Object:SetObjInfo()
	if self.view then
		self.view:SetObjInfo(self:GetId())
	end
end

function Object:GetGameObj()
	if self.view then
		return self.view:GetGameObj()
	end
end
--====data
function Object:GetModel()
	return self.dataModel:GetModel()
end

--======notify
function Object:RegisterDataNotify(field,func,obj)
	if self.dataModel then
		self.dataModel:RegisterFieldNotify(field,func,obj)
	end
end

function Object:UnregisterDataNotify(field,func,obj)
	if self.dataModel then
		self.dataModel:UnregisterFieldNotify(field,func,obj)
	end
end

function Object:BindChangeNotify(func,obj)
	if self.dataModel then
		self.dataModel:BindChangeNotify(func,obj)
	end
end
--======logic
function Object:RegisterAoi()
	if not linkedAoi then linkedAoi = _G.LinkedAoi end
	linkedAoi:AddEntity(self)
	if not blockAoi then blockAoi = _G.BlockAoi end
	blockAoi:AddEntity(self)
end

function Object:UnregisterAoi()
	if self.aoi then
		linkedAoi:RemoveEntity(self)
	end
	if self.blockId then
		blockAoi:RemoveEntity(self)
	end
end

function Object:AoiOnPosModify(oldPos)
	linkedAoi:Move(self,oldPos)
	blockAoi:Move(self)
end

function Object:EnableUpdate(benable)
	if benable then
		self.updateHandle = UpdateBeat:RegisterListener(self.Update,self)
	elseif self.updateHandle then
		UpdateBeat:RemoveListener(self.updateHandle)
		self.updateHandle = nil
	end
end

--============

function Object:GetPos(clone)
	return self.dataModel:GetPos(clone)
end

function Object:SetPos(pos,clone,nosync)
	if not pos then return end
	local oldPos = self:GetPos()
	self.dataModel:SetPos(pos,clone)
	if not nosync then
		self:SyncPos()
	end
	if not self.aoi then --注册入aoi
		self:RegisterAoi()
	elseif oldPos then --修改坐标，产生移动，不采用发事件的方式
		self:AoiOnPosModify(oldPos)
	end
end

function Object:SyncPos()
	if self.view then
		self.view:SetPos(self.dataModel:GetPos())
	end
end

function Object:SyncPosFromView()
	if self.view then
		self:SetPos(self.view:GetPos(),false,true)
	end
end

function Object:GetDir(clone)
	return self.dataModel:GetDir(clone)
end

function Object:SetDir(dir,clone,nosync)
	if not dir or dir:isZero() then return end
	self.dataModel:SetDir(dir,clone)
	if not nosync then
		self:SyncDir()
	end
end

function Object:SyncDir()
	if self.view then
		self.view:SetDir(self.dataModel:GetDir())
	end
end

function Object:SetScale(scale)
	if not scale then return end
	self.dataModel:SetScale(scale)
	self:SyncScale()
end

function Object:SyncScale()
	if self.view then
		self.view:SetScale(self.dataModel:GetScale())
	end
end


function Object:GetYRad()
	local dir = self.dataModel:GetDir()
	if dir then
		return dir:yRad()
	end
	return 0
end

function Object:DistNH(other)
	local pos = self:GetPos()
	local otherPos = other:GetPos()
	return pos:distNH(otherPos)
end

function Object:IsLookAt(other,accuracy)
	local delta = other:GetPos() - self:GetPos()
	delta.y = 0
	return abs(Vector3.Angle(self:GetDir(),delta))<=(accuracy or 10)
end

function Object:LookAt(otherPos)
	local dir = otherPos - self:GetPos()
	dir.y = 0
	dir:setNormalize()
	self:SetDir(dir)
end

function Object:PlayAni(ani,fade)
	if self.view then
		if fade then
			return self.view:PlayAni(ani,fade)
		else
			return self.view:PlayAni(ani)
		end
	end
end

function Object:IsUseSelectable()
	return false
end

function Object:IsAtkSelectable()
	return false
end

--==================

return Object
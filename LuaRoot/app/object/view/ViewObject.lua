local ViewObject = class("ViewObject")

local Util = Util

function ViewObject:ctor(entity)
	self.entity = entity
end

--======resource
function ViewObject:DoLoad(res,loadfunc,cb,caller,loadParam)
	if not self.view then
		self.isLoading = true
		self.res = res
		local SceneLoader = SceneLoader
		SceneLoader:AddOne(res)
		local loadCb = function (view)
			self.view = view
			if self.isLoading then
				self.isLoading = false
				self:OnLoaded()
				if cb then cb(caller) end
			else
				self:Release()
			end
			SceneLoader:FinishOne(res)
		end
		loadParam = loadParam or self:GetLoadParam()
		if loadParam == nil then
			loadfunc(res,loadCb)
		else
			loadfunc(res,loadParam,loadCb)
		end
	end
end

function ViewObject:GetLoadParam() end

function ViewObject:Release()
	self.isLoading = false
	self.entity = nil
	if self.view then
		self.view:Release()
		self.view = nil
	end
end

function ViewObject:OnLoaded()
end

function ViewObject:GetRes()
	return self.res
end

function ViewObject:SetPos(pos)
	if self.view and pos then
		self.view:SetPos(pos.x,pos.y,pos.z)
	end
end

function ViewObject:GetPos()
	local trans = self:GetTransform()
	return Util.GetTransformPos(trans)
end

function ViewObject:SetDir(dir)
	if self.view and dir then
		self.view:SetForward(dir.x,dir.y,dir.z)
	end
end

function ViewObject:SetRot(rot)
	if self.view and rot then
		self.view:SetRot(rot.x,rot.y,rot.z)
	end
end

function ViewObject:SetScale(scale,uniform,isLocal)
	if self.view and scale then
		if uniform then
			self.view:SetScale(scale,scale,scale,isLocal or false)
		else
			self.view:SetScale(scale.x,scale.y,scale.z,isLocal or false)
		end
	end
end

function ViewObject:SetVisible(v)
    if self.view then
        self.view:SetVisible(v)
    end
end

function ViewObject:SetParent(parentTrans)
	if self.view then
		self.view:SetParent(parentTrans)
	end
end

function ViewObject:GetTransform()
	if self.view then
		return self.view:GetRootTrans()
	end
end

function ViewObject:GetGameObj()
	if self.view then
		return self.view:GetRootGameObj()
	end
end

function ViewObject:SetObjInfo(id)
	if self.view then
		self.view:SetObjInfo(id)
	end
end

return ViewObject
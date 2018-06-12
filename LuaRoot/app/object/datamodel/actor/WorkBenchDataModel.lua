local WorkBenchDataModel = class("WorkBenchDataModel",require("app.base.DataModel"))
local StoreItemDataModel = require("app.object.datamodel.StoreItemDataModel")

local saveFields = {"tid","slip","productTime","productCd"}

function WorkBenchDataModel:Born()
	self.bag:RegisterIndexNotify(self.OnBagChange,self)
end

function WorkBenchDataModel:Init(tid)
	self:InitId()
    self:SetValue("tid",tid,true)
	self.bag:SetCap(10) 
	self.product:SetCap(2)
	self:PostImport()
	return self:GetId()
end

function WorkBenchDataModel:MarkSave()
    self:MarkFieldSave(saveFields)
	self.bag = StoreItemDataModel.new()
	self.product = StoreItemDataModel.new()
end

function WorkBenchDataModel:PostImport()
	
end

function WorkBenchDataModel:Import(data)
	WorkBenchDataModel.super.Import(self,data)
	self.bag = StoreItemDataModel.new()
	if data.bag then
        self.bag:Import(data.bag)
    end
	self.product = StoreItemDataModel.new()
	if data.product then
        self.product:Import(data.product)
    end
end

function WorkBenchDataModel:Export(modified)

    local data,mod = WorkBenchDataModel.super.Export(self,modified)
    if self.bag:IsDirty() then
        local bag,bagMod = self.bag:Export(modified)
        data.bag = bag
        if mod or bagMod then
            mod = mod or {}
            mod.bag = bagMod
        end
    end
	if self.product:IsDirty()  then
        local product,productMod = self.product:Export(modified)
        data.product = product
        if mod or equipMod then
            mod = mod or {}
            mod.product = productMod
        end
    end
    return data,mod
end

function WorkBenchDataModel:OnBagChange(data)
	if data then
		self:MarkWrokDirty()
	end
end

function WorkBenchDataModel:MarkWrokDirty()
	self.benchDirty = true
end

function WorkBenchDataModel:SetTime(time)
	self:SetValue("productTime",time,true)
end

function WorkBenchDataModel:SetCd(cd)
	self:SetValue("productCd",cd,true)
end

function WorkBenchDataModel:SetSlip(slip)
	self:SetValue("slip",slip,true)
end

return WorkBenchDataModel
---建筑数据，继承自场景物体数据，拥有场景物体的数据，---同时拥有功能数据，功能数据类似工作台、怪物数据等---updateTid:基础家具升级tidlocal FurnitureDataModel = class("FurnitureDataModel",require("app.object.datamodel.actor.SceneItemDataModel"))local WorkBenchDataModel = require("app.object.datamodel.actor.WorkBenchDataModel")local furniture_saveFields = {"direct","posIndex","updateId"}local Global = Globalfunction FurnitureDataModel:MarkSave()    FurnitureDataModel.super.MarkSave(self)    self:MarkFieldSave(furniture_saveFields)endfunction FurnitureDataModel:NeedSaveLoc()    return falseendfunction FurnitureDataModel:Born()    FurnitureDataModel.super.Born(self)    self:CreateFunctionData()endfunction FurnitureDataModel:GetLayer()    return self.cfg.floorConditionIdendfunction FurnitureDataModel:CreateFunctionData()    --根据cfg的配置来生成工作台数据	local typeId =  self:IsWorkBench()	if typeId then		self.benchDataModel = WorkBenchDataModel.new()		self.benchDataModel:Born()		self.benchDataModel:Init(typeId)		self.benchDataModel.bag:RegisterIndexNotify(self.OnWorkBenchChange,self)		self:OnWorkBenchChange()	endendfunction FurnitureDataModel:SetLoc(dir,index)    self:SetValue("direct",dir,true)    self:SetValue("posIndex",index,true)    print("posIndex",self.posIndex)endfunction FurnitureDataModel:SetUpdateId(updateId)	self:SetValue("updateId",updateId,true)	print("UpdateID ISSSS:",self.tid,self.updateId)endfunction FurnitureDataModel:SetUpdateInfo(sceneTid,updateId)	self:SetValue("tid",sceneTid,true)	self:SetValue("updateId",updateId,true)	print("UpdateID IS:",self.tid,self.updateId)endfunction FurnitureDataModel:OnWorkBenchChange()
	self.workbenchDirty = true
endfunction FurnitureDataModel:Import(data)
	FurnitureDataModel.super.Import(self, data)
	if data.bench then
		self.benchDataModel = WorkBenchDataModel.new()
		self.benchDataModel:Import(data.bench)
		self.benchDataModel.bag:RegisterIndexNotify(self.OnWorkBenchChange,self)
	end
endfunction FurnitureDataModel:Export(modified)
	local data,mod = FurnitureDataModel.super.Export(self,modified)
	if self:IsDirty() then
		self.workbenchDirty = false
		local benchData,benchMod = self.benchDataModel:Export(modified)
		data.bench = benchData
		if benchMod then
			if not mod then mod = {} end
			mod.bench = benchMod
		end		
	end
	
	return data,mod
endfunction FurnitureDataModel:IsDirty()
	if self.benchDataModel then
		return self.dirty or self.workbenchDirty or self.benchDataModel.dirty
	else
		return self.dirty or self.workbenchDirty
	end
	
endreturn FurnitureDataModel


---建筑数据，继承自场景物体数据，拥有场景物体的数据，
	self.workbenchDirty = true
end
	FurnitureDataModel.super.Import(self, data)
	if data.bench then
		self.benchDataModel = WorkBenchDataModel.new()
		self.benchDataModel:Import(data.bench)
		self.benchDataModel.bag:RegisterIndexNotify(self.OnWorkBenchChange,self)
	end
end
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
end
	if self.benchDataModel then
		return self.dirty or self.workbenchDirty or self.benchDataModel.dirty
	else
		return self.dirty or self.workbenchDirty
	end
	
end

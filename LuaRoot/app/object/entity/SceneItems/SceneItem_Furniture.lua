--SceneItem_Furniture : 负责具体加载，关联FurnitureDataModel
    print("use furniture",self.dataModel.tid)
	if self.dataModel:GetParam() then 
		WorkBenchManager:OpenBench(self.dataModel:GetParam()[1],self.dataModel)
	end
end
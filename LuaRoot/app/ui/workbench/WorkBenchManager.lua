local Manager = class("WorkBenchManager")
local ViewManager = ViewManager
local CurrView
local Timer = Timer
local System = Global.GetSystem

local BenchType ={
	BONFIRE = 1001,--篝火
	STEW = 1002,--炖锅
	GARDEN = 1003,--菜园
	WOOD = 1004,--木工台
	FOURNACE = 1005,--熔炉
	RAIN = 1006,--雨水收集器
	STONE = 1007,--石匠工作台
	AIRDRYER = 1008,--风干机
	SKINFARME = 1009,--制皮架
	SARTORIUS = 1010,--缝纫机
	REFINE = 1011,--精炼车间
	CLOTH = 1012,--衣帽间
	FIREARMS = 1013,--枪械工作台
	CHEMICAL = 1014,--化工厂
	FERMENT = 1015,--发酵池
	--moto dibao
	MOTO = 74,
}
--------------------------------
function Manager:OpenBench(tid,dataModel)
	print("open work bench tid :",tid)
	self.dataModel = dataModel
	self.tid = tid
	self.DataManager = WorkTableManager
	self.dataList = self:InitConfig(self.tid)
	self.bag = self.dataModel.benchDataModel.bag
	self.product = self.dataModel.benchDataModel.product
	self:OpenView()
end

function Manager:OpenView()
	if not self.tid then return end
	ViewManager:OpenView(ViewBackground)
	ViewManager:OpenView(ViewBackPack)
	ViewManager:OpenView(ViewMenu)

	if self.tid == BenchType.FIREARMS then--gun
		ViewManager:OpenView(ViewFirearmsBench)
	elseif self.tid == BenchType.CLOTH or self.tid == BenchType.STEW then --衣帽间 炖锅

		ViewManager:OpenView(ViewClothBench)
		--ViewManager:OpenView(ViewMotoBench)
	else
		ViewManager:OpenView(ViewWorkBench)
	end

	ViewManager:CloseView(DragItemView)
	ViewManager:OpenView(DragItemView)
end

function Manager:ResetGlobal(vie)
	CurrView = vie
	if self.tid == BenchType.MOTO then
		self:InitMotoData()
	else
		self:InitCommonData()
	end

	
end

function Manager:InitMotoData()

end

function Manager:InitCommonData()
	local saveTime = self.dataModel.benchDataModel.productTime	
	if self.product.count > 0 then
		self.slip = self.dataModel.benchDataModel.slip
		self.loop = self.dataModel.benchDataModel.productCd
		local nowTime = System():CurrentTime() 
		local temp = nowTime - saveTime
		if not self.loop then 
			self.loop = 0
		end
		print("CurrentTime :",temp/60,self.loop,self.loop - math.floor(temp / 60))
		if temp / 60 < self.loop then
			self:InitTimer()
		else
			CurrView:ResetProducts()
		end
	end
end

function Manager:CloseBench()
	ViewManager:CloseView(CurrView)
	ViewManager:CloseView(ViewBackPack)
	ViewManager:CloseView(ViewBackground)
	ViewManager:CloseView(ViewMenu)
end

function Manager:InitConfig(tid)
	self.tid = tid
	self.dataList = CfgData:GetWorkBench(tid)
	return self.dataList
end
-------------------------------------

function Manager:CreatSlot(index,sl,luaItem,data)
	local slot = sl.new(index,luaItem,data,self)
	slot:Init()
	slot:SetData(slot.data,false)
	--self.items[index] = slot
	return slot
end

function Manager:CreatFormula(index,sl,luaItem)
	local slot = sl.new(index,luaItem,self)
	for i,v in ipairs(self.dataList) do
		if i == index then
			local name = CfgData:GetItem(v.composeId).name 
			slot:SetData( CfgData:GetText(name))
			slot:InitElmentSlot(v)
		end
	end
end


----------------------------------------
function Manager:GetComposeCount()
	return self:GetMaterialCount() + self:GetFeedCount()
end

function Manager:GetMaterialCount()
	if self.tid == BenchType.FIREARMS then
		return 4
	elseif self.tid == BenchType.CHEMICAL then
		return 2
	else
		return 1
	end
end

function Manager:GetFeedCount()
	if self.tid == BenchType.BONFIRE or self.tid == BenchType.FOURNACE or 
		self.tid == BenchType.REFINE or self.tid == BenchType.FERMENT then
		return 1
	elseif self.tid == BenchType.STEW or self.tid == BenchType.CLOTH then
		return 2
	else 
		return 0
	end
end

function Manager:GetProductCount()
	if self.tid == BenchType.STEW then
		return 1
	else
		return 0
	end
end

function Manager:GetFormulaCount()
	local count = 0
	for i,v in ipairs(self.dataList) do
		--没有学习的
		count =  count + 1
	end
	return count + self:GetHaveFormulaCount()
end

--获取已经学过配方的数量
function Manager:GetHaveFormulaCount()
	return 0
end

function Manager:GetPlayerLevel()
	return _G.HostPlayer.dataModel.level
end
 
function Manager:GetBenchName()
	return self.dataList[1].name
end

function Manager:CanUpper()
	--更具建筑物当前等级进行判断 todo
	return false
end

function Manager:GetLoop()
	return self.loop
end

----------------------------------------
function Manager:OnBeginDrag(item)
	if self.dragView == nil then
		self.dragView = ViewManager:GetView(DragItemView)
	end
	local data = item.data
    if data ~= nil then
        self.dragView:SetSprite(data.cfg.icon)
        item:SetVisible(false)
    end
	self.dragitem = item
	InventoryManager:SetDragItem(item)
end

function Manager:OnEndDrag(item)
	if self.dragView then
        self.dragView:SetVisible(false)
    end

    if item.data then
        item:SetVisible(true)
    end
end

function Manager:OnDrop(item)
	if self.loop then return end
	InventoryManager:DropOff(item,function(drag)
		if drag:IsBenchMaterial() then return end
		self:DropOff(drag,item)
	end)
end

---------------------------------
--通用工作台
---------------------------------
function Manager:DropOff(drag,drop)
	local quantity,typeId = self:GetMaterialsCount(drop,drag) 
	
	local count 

	if self.bag.items[drop.index] then 
		count  = self.bag.items[drop.index].count
	else
		count = 0
	end
	print("quantity,count",quantity,count)
	if  quantity > 0 then
		self:OnAddItem(drag,drop,count,quantity,self.bag,typeId)
	else
		return
	end	

	--self:RefreshData()
end

function Manager:OnAddItem(drag,drop,count,quantity,bag,typeId)
	if count == 0 then 
		if drag.data.count <= quantity then
			local temp = self.bag:AddItem(drag.data.tid,drag.data.count)
			drag.owner:Remove(drag.data)
		else
			local temp = self.bag:AddItem(drag.data.tid,quantity)
			drag.owner:Consume(drag.data.tid,quantity)
		end
		if typeId == 2 then
			self.bag:SetItem(drop.index,temp)
		end
	else 
		if count >= quantity then 
			print("bench is full")
			return 
		elseif count + drag.data.count <= quantity then 
			self.bag:Merge(drag.data.tid,drag.data.count)
			drag.owner:Remove(drag.data)
			
		else
			local count = quantity - count 
			self.bag:Merge(drag.data.tid,count)	
			drag.owner:Consume(drag.data.tid,count)
		
		end
	end
end

function Manager:GetHintData(v)
	local count = 0
	local iconList = {}
	local tempList ={}
	-- 4主2辅
	if v.materials1~= nil then
		table.insert(iconList,CfgData:GetItem(v.materials1).icon)
		table.insert(tempList,v.quantity1)
		count = count + 1
	end
	if v.materials2~= nil then
		table.insert(iconList,CfgData:GetItem(v.materials2).icon)
		table.insert(tempList,v.quantity2)
		count = count + 1
	end
	if v.materials3~= nil then
		table.insert(iconList,CfgData:GetItem(v.materials3).icon)
		table.insert(tempList,v.quantity3)
		count = count + 1
	end
	if v.materials4~= nil then
		table.insert(iconList,CfgData:GetItem(v.materials4).icon)
		table.insert(tempList,v.quantity4)
		count = count + 1
	end
	if v.ingredientId1 ~= nil then
		table.insert(iconList,CfgData:GetItem(v.ingredientId1).icon)
		table.insert(tempList,v.ingredientNum1)
		count = count + 1
	end
	if v.ingredientId2 ~= nil then
		table.insert(iconList,CfgData:GetItem(v.ingredientId2).icon)
		table.insert(tempList,v.ingredientNum2)
		count = count + 1
	end
	if v.arrow ==arrow then
		table.insert(iconList,"main_arrow")
		table.insert(tempList,1)
		count = count + 1
	end
	if v.composeId ~=nil then
		table.insert(iconList,CfgData:GetItem(v.composeId).icon)
		table.insert(tempList,v.composeNum)
		count = count + 1
	end

	return count,iconList,tempList
end

function Manager:GetMaterialsCount(item,drag)
	if item.index % 2 == 0 then --Feed
		for j,v in pairs(self.dataList) do		
			if v.tableType == self.tid then
				if v.ingredientId1 == drag.data.tid then
					return v.ingredientNum1,2
				elseif v.ingredientId2 == drag.data.tid then
					return v.ingredientNum2,2
				end
			end
		end
		return 0,0
	else	
               --主材料
		for i,v in pairs(self.dataList) do
			if v.tableType == self.tid then
				if v.materials1 == drag.data.tid then
					return v.quantity1,1
				elseif v.materials2 == drag.data.tid then
					return v.quantity2,1
				elseif v.materials3 == drag.data.tid then
					return v.quantity3,1
				elseif v.materials4 == drag.data.tid then
					return v.quantity4,1
				end
			end
		end
		return 0,0
	end
end

function Manager:CheckMaterial()	
	local tidCount = 0
	for i,v in ipairs(self.dataList) do
		for j,k in ipairs(self.bag.items) do
			if k.tid == v.materials1 or k.tid == v.materials2 or
			   k.tid == v.materials3 or k.tid == v.materials4 or
			   k.tid == v.byProductId1 or k.tid == v.byProductId2 then
				tidCount = tidCount + 1
			end
		end
		if tidCount == self.bag.count and self.bag.count ~= 0 then	
			return v
		end
	end
	return nil
end

function Manager:CheckQuantity(slip)
	local count = 0
	for i,v in ipairs(self.bag.items) do
		if v.count == slip.quantity1 or v.count == slip.quantity2 or
		   v.count == slip.quantity3 or v.count == slip.quantity4 or
		   v.count == slip.ingredientNum1 or v.count == slip.ingredientNum2 then
			count = count + 1
		end
	end
	
	if self.bag.count == count then
		return true
	else 
		return false
	end
	
end

function Manager:SetBuildState(state)
	self.isBuild = state
end

function Manager:RefreshData()
	self.slip = self:CheckMaterial()
	if self.slip then 
		print("slip-----",self.slip.id)
	end
	if self.slip and self:CheckQuantity(self.slip) then
		self:AddBuildItem(self.slip)
		self:InitLoop(self.slip.cd * 60)
		self:InitTimer()
	else
		self:ResetItems()
	end
	CurrView:SetHint(self.slip)
end

function Manager:ResetItems()
	if self.product.count == 0 then return end
	for i,v in ipairs(CurrView.products) do
		if CurrView.products[i] then
			self.product:Remove(CurrView.products[i].data)
		end
	end
	if self.timer then
		self.timer:Stop()
		self.timer = nil
	end
	if self.loop then
		self:InitLoop(nil)
	end
	self.slip = nil
end

function Manager:AddBuildItem(slip)
	if slip.composeId ~= nil then
		self.product:AddItem(slip.composeId,slip.composeNum)
	end

	if slip.byProduct then
		self:AddProducts(slip)
	end
end

function Manager:AddProducts(slip)
	if slip.byProductId1 ~= nil then
		self.product:AddItem(slip.byProductId1,slip.byProductNum1)
	end
	if slip.byProductId2 ~= nil then
		self.product:AddItem(slip.byProductId2,slip.byProductNum2)
	end
end

function Manager:RefreshFeedIcon()
	if self:GetFeedCount() == 1 then
		CurrView.items[2]:SetFeedIcon(CfgData:GetItem(self.slip.ingredientId1).icon)
	end
	if self:GetFeedCount() == 2 then
		CurrView.items[2]:SetFeedIcon(CfgData:GetItem(self.slip.ingredientId1).icon)
		CurrView.items[3]:SetFeedIcon(CfgData:GetItem(self.slip.ingredientId2).icon)
	end
end

function Manager:RefreshProduct()

end

function Manager:InitLoop(loop)
	self.loop = loop 
end

function Manager:InitTimer()
	if not self.slip then return end
	self.cd = self.slip.cd * 60
	CurrView:InitTimer(self.loop,self.cd)
	self.timer = Timer.Loop(60,self.TimerCall,self,self.loop):Start()
end

function Manager:TimerCall()
	self.loop = self.loop - 1
	if self.loop > 0 then
		CurrView:SetTimer(self.loop,self.cd)
	elseif self.loop == 0 then
		CurrView:FinishProduct()
		self.timer:Stop()
		self.timer = nil	
	end
end

----------------------------------
function Manager:SaveData()
	if self.bag.count == 0 and self.product.count == 0 then return end
	self.dataModel.benchDataModel:SetTime( System():CurrentTime())
	self.dataModel.benchDataModel:SetCd(self.loop)
	self.dataModel.benchDataModel:SetSlip(self.slip)
	Global.GetFurnitureMnger():OnFurnitureChanged(self.dataModel)

end

function Manager:ClearData()
	self.dataModel.benchDataModel:SetTime(nil)
	self.dataModel.benchDataModel:SetCd(nil)
	self.dataModel.benchDataModel:SetSlip(nil)
end
--------------------------------------
function Manager:CloseViewWith()
	if CurrView then
		ViewManager:CloseView(CurrView)
	end
end

function Manager:Release()
	if self.timer then
        self.timer:Stop()
        self.timer = nil
    end
	ViewManager:CloseView(DragItemView)
	self:SaveData()
	self.dataModel = nil
	self.tid = nil
	self.DataManager = nil
	self.dataList = nil
	CurrView = nil
	self.bag = nil
	self.product = nil
	self.loop = nil
	self.dragView = nil
	self.dragitem = nil
	self.slip = nil
	self.isBuild = nil
end

WorkBenchManager = Manager.new()


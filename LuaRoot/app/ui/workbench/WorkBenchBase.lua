local WorkBenchBase = class(" WorkBenchBase",require("app.ui.UiView"))
local FormulaSlot = require("app.ui.workbench.FormulaSlot")
local BenchSlot = require("app.ui.workbench.BenchSlot")
local BuildSlot = require("app.ui.workbench.BuildSlot")
local HintSlot = require("app.ui.workbench.HintSlot")
local luaUtility = CS.Game.LuaUtility

function WorkBenchBase:OnOpenBench()
	self.items = {}
	self.products = {}
	self.manager = WorkBenchManager
	self.bag = self.manager.bag
	self.product = self.manager.product
	self.dataList = self.manager.dataList
	--Manager.bag:RegisterIndexNotify(self.OnBagChange,self)
	--Manager.product:RegisterIndexNotify(self.OnProductChange,self)
	self:RegisterEvents()
end


function WorkBenchBase:InitGloble()
	local name = self.manager:GetBenchName()
	luaUtility.TextSetTxt(self.txt_name,CfgData:GetText(name))
	luaUtility.ComponentGameObjVisible(self.btn_upper,self.manager:CanUpper())
	self:SetFormulaVisible(false)
end

function WorkBenchBase:InitFormula()
	self:SetFormulaVisible(true)
	local index  = 1
	local count  = self.manager:GetFormulaCount()
	luaUtility.SetLuaSimpleListCount(self.grid_formula,count)
	luaUtility.BindOnItemAdd(self.grid_formula,function(item)
		self.manager:CreatFormula(index,FormulaSlot,item)
		index = index + 1
	end)
	luaUtility.LuaSimpleListInit(self.grid_formula)
end

function WorkBenchBase:SetHint(slip)
	if slip == nil then return end
	local count,iconList,countList = self.manager:GetHintData(slip)
	local index = 1
	local name = CfgData:GetItem(slip.composeId).name
	luaUtility.TextSetTxt(self.txt_hintname,CfgData:GetText(name))
	luaUtility.SetLuaSimpleListCount(self.grid_hint,count)
	luaUtility.BindOnItemAdd(self.grid_hint,function(item)
		self:CreatHint(index,HintSlot,item,iconList,countList)
		index = index + 1
	end)
	luaUtility.LuaSimpleListInit(self.grid_hint)
end

function WorkBenchBase:CreatHint(index,sl,luaItem,iconList,countList)
	local slot = sl.new(index,luaItem,self)
	self:SetElementData(slot,iconList,countList)
end

function WorkBenchBase:SetElementData(slot,iconList,countList)
	for i,v in ipairs(countList) do
		for j,k in ipairs(iconList) do
			if i == j and i == slot.index  then
				slot:SetData(k,v)
			end
		end		
	end
end

function WorkBenchBase:SetFormulaVisible(sign)
	luaUtility.ComponentGameObjVisible(self.com_formula,sign)
end
--------------------------------
function WorkBenchBase:ResetProducts()
	for i,v in ipairs(self.products) do
		v:FinishProduct()
	end
end

function WorkBenchBase:InitTimer(loop,cd)
	self.products[1]:SetTime(loop,cd)
end

function WorkBenchBase:SetTimer(loop,cd)
	self.products[1]:SetTime(loop,cd)
end

function WorkBenchBase:FinishProduct()
	self.products[1]:FinishProduct()
	self:ClearAllMaterial()
end

function WorkBenchBase:ClearAllMaterial()
	for i,v in ipairs(self.items) do
		print("ClearAllMaterial--",valStr(v))
		if self.items[i].data then
			self.bag:Remove(self.items[i].data)
		end
	end
end
--------------------
function WorkBenchBase:OnCloseFormula()
	self:SetFormulaVisible(false)
end

function WorkBenchBase:OnFormula()	
	self:InitFormula()
end

function WorkBenchBase:OnAddWorker()

end

function WorkBenchBase:OnSkip()
	
end

function WorkBenchBase:OnUpper()
	
end
----------------------


function WorkBenchBase:RegisterEvents()
	self:RegisterButtonClick(self.btn_close,self.OnCloseFormula,"btn_close")
	self:RegisterButtonClick(self.btn_worker,self.OnAddWorker,"btn_worker")
	self:RegisterButtonClick(self.btn_formula,self.OnFormula,"btn_formula")
	self:RegisterButtonClick(self.btn_skip,self.OnSkip,"btn_skip")
	self:RegisterButtonClick(self.btn_upper,self.OnUpper,"btn_upper")
end

function WorkBenchBase:UnRegisterEvents()
	self:UnregisterButtonClick(self.btn_close,"btn_close")
	self:UnregisterButtonClick(self.btn_worker,"btn_worker")
	self:UnregisterButtonClick(self.btn_formula,"btn_formula")
	self:UnregisterButtonClick(self.btn_skip,"btn_skip")
	self:UnregisterButtonClick(self.btn_upper,"btn_upper")
end


function WorkBenchBase:OnClose()
	self:UnRegisterEvents()
	luaUtility.SetLuaSimpleListCount(self.grid_formula,0)
	luaUtility.LuaSimpleListInit(self.grid_formula)
	luaUtility.SetLuaSimpleListCount(self.grid_hint,0)
	luaUtility.LuaSimpleListInit(self.grid_hint)

	self.manager:Release()
end


return WorkBenchBase
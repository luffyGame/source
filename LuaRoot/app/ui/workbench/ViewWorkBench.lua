local view = class(" ViewWorkBench",require("app.ui.workbench.WorkBenchBase"))
local BenchSlot = require("app.ui.workbench.BenchSlot")
local BuildSlot = require("app.ui.workbench.BuildSlot")
local luaUtility = CS.Game.LuaUtility
view.res = "workbench"


function view:OnOpen()
	self:OnOpenBench()
	self.manager = WorkBenchManager
	self.manager.bag:RegisterIndexNotify(self.OnBagChange,self)
	self.manager.product:RegisterIndexNotify(self.OnProductChange,self)
	self:Init()
end

function view:Init()
	self:InitGloble()
	self:InitMaterial()
	self:InitBuild()
	self.manager:ResetGlobal(self)
end

function view:InitMaterial()
	local index = 1
	luaUtility.SetLuaSimpleListCount(self.grid_data,self.manager:GetComposeCount())
	luaUtility.BindOnItemAdd(self.grid_data,function(item)
		self.items[index] =  self.manager:CreatSlot(index,BenchSlot,item,self.bag)
		index = index + 1
	end)
	luaUtility.LuaSimpleListInit(self.grid_data)
end

function view:InitBuild()
	local index = 1
	luaUtility.SetLuaSimpleListCount(self.grid_build, 1 + self.manager:GetProductCount())
	luaUtility.BindOnItemAdd(self.grid_build,function(item)
		self.products[index] = self.manager:CreatSlot(index,BuildSlot,item,self.product)
		index = index + 1
	end)
	luaUtility.LuaSimpleListInit(self.grid_build)
end

------------------------
function view:OnBagChange(index)
	print("ViewWorkBench - OnBagChange:",index,self.bag.count,#self.items)
	if self.items[index ] then
        self.items[index ]:SetData(self.bag.items[index])
		if self.manager:GetLoop() and self.manager:GetLoop() == 0 then return end
		self.manager:RefreshData()
		self.manager:RefreshFeedIcon()
    end
end

function view:OnProductChange(index)
	print("ViewWorkBench - OnProductChange:",index,self.product.count)
	if self.products[index] then
		self.products[index ]:SetData(self.product.items[index])
	end
	if self.product.count == 0 then
		self.manager:InitLoop(nil)
	end
end
---------------------
function view:OnClose()
	view.super.OnClose(self)
	self.bag:UnregisterIndexNotify(self.OnBagChange,self)
	self.product:UnregisterIndexNotify(self.OnProductChange,self)
	luaUtility.SetLuaSimpleListCount(self.grid_data,0)
	luaUtility.LuaSimpleListInit(self.grid_data)
	luaUtility.SetLuaSimpleListCount(self.grid_build,0)
	luaUtility.LuaSimpleListInit(self.grid_build)
end

ViewWorkBench = view
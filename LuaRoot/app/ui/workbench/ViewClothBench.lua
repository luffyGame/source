local view = class(" ViewClothBench",require("app.ui.workbench.WorkBenchBase"))
local BenchSlot = require("app.ui.workbench.BenchSlot")
local BuildSlot = require("app.ui.workbench.BuildSlot")
local luaUtility = CS.Game.LuaUtility
view.res = "clothbench"


function view:OnOpen()
	self:OnOpenBench()
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
	self.items[1] = self.manager:CreatSlot(1,BenchSlot,self.slot_1,self.bag)
	self.items[2] = self.manager:CreatSlot(2,BenchSlot,self.slot_2,self.bag)
	self.items[3] = self.manager:CreatSlot(3,BenchSlot,self.slot_3,self.bag)
end

function view:InitBuild()
	self.products[1] = self.manager:CreatSlot(1,BuildSlot,self.slot_build,self.product)
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
end

ViewClothBench = view
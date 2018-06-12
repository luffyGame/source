local view = class(" ViewMotoBench",require("app.ui.workbench.WorkBenchBase"))
local BenchSlot = require("app.ui.workbench.BenchSlot")
local BuildSlot = require("app.ui.workbench.BuildSlot")
local luaUtility = CS.Game.LuaUtility
view.res = "motobench"


function view:OnOpen()
	self:OnOpenBench()
	self.manager.bag:RegisterIndexNotify(self.OnBagChange,self)
	self:Init()
end

function view:Init()
	self:InitMaterial()
	self:InitCount()
	self.manager:ResetGlobal(self)
end

function view:InitMaterial()
	self.items[1] = self.manager:CreatSlot(1,BenchSlot,self.slot_1,self.bag)
	self.items[2] = self.manager:CreatSlot(2,BenchSlot,self.slot_2,self.bag)
	self.items[3] = self.manager:CreatSlot(3,BenchSlot,self.slot_3,self.bag)
	self.items[4] = self.manager:CreatSlot(4,BenchSlot,self.slot_4,self.bag)
	self.items[5] = self.manager:CreatSlot(5,BenchSlot,self.slot_5,self.bag)
	self.items[6] = self.manager:CreatSlot(6,BenchSlot,self.slot_6,self.bag)
	self.items[7] = self.manager:CreatSlot(7,BenchSlot,self.slot_7,self.bag)
	self.items[8] = self.manager:CreatSlot(8,BenchSlot,self.slot_8,self.bag)
	self.items[9] = self.manager:CreatSlot(9,BenchSlot,self.slot_9,self.bag)
	self.items[10] = self.manager:CreatSlot(10,BenchSlot,self.slot_10,self.bag)

end

function view:InitCount()
	
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
---------------------
function view:OnClose()
	view.super.OnClose(self)
	self.bag:UnregisterIndexNotify(self.OnBagChange,self)
	self.product:UnregisterIndexNotify(self.OnProductChange,self)
end

ViewMotoBench = view
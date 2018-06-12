local FormulaSlot = class("FormulaSlot")
local FormulaElementSlot = require("app.ui.workbench.FormulaElementSlot")
local luaUtility = CS.Game.LuaUtility
local Const = _G.Const

function FormulaSlot:ctor(index,luaItem,receiver)
	luaItem.Data:Inject(self)
	self.index = index
	self.receiver = receiver
end

function FormulaSlot:InitElmentSlot(slip)
	local index = 1
	local count,iconList,countList = WorkBenchManager:GetHintData(slip)
	luaUtility.SetLuaSimpleListCount(self.grid_formulaSlot,count)
	luaUtility.BindOnItemAdd(self.grid_formulaSlot,function(item)
		self:CreatFormula(index,FormulaElementSlot,item,iconList,countList)
		index = index + 1
	end)
	luaUtility.LuaSimpleListInit(self.grid_formulaSlot)
end

function FormulaSlot:CreatFormula(index,sl,item,iconList,countList)
	local slot = sl.new(index,item,self)
	self:SetElementData(slot,iconList,countList)
end

function FormulaSlot:SetElementData(slot,iconList,countList)
	for i,v in ipairs(countList) do
		for j,k in ipairs(iconList) do
			if i == j and i == slot.index  then
				slot:SetData(k,v)
			end
		end		
	end
end

function FormulaSlot:SetData(txt)
	luaUtility.TextSetTxt(self.txt_name,txt)
end



return FormulaSlot
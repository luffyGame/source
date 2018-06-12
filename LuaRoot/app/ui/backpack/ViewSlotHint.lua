local view = class("ViewSlotHint",require("app.ui.UiView"))
local LuaUtility = CS.Game.LuaUtility
view.res = "viewslothint"

function view:OnOpen()
	self:SetVisible(false)
end


function view:SetContain(slotName,fromTrans)
	self:SetVisible(true)
	LuaUtility.TextSetTxt(self.hintName,slotName)
	self:SetPosition(fromTrans)	
end

function view:SetPosition(fromTrans)
	LuaUtility.TransformSetPos(self.hintRoot,fromTrans)
end

function view:SetVisible(isOpen)
	LuaUtility.ComponentGameObjVisible(self.hintRoot,isOpen)
end

ViewSlotHint = view
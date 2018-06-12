local UiHandler = class("UiHandler")
local bind = require("xlua.util").bind

local LuaUtility = CS.Game.LuaUtility

--一个简化，每个ui注册的同类事件只有一种,如果有多种的话，使用通配符
function UiHandler:AddHandle(func,wildCard)
	local handle = bind(func,self)
	if wildCard then
		if not self.uiHandles then self.uiHandles = {} end
		self.uiHandles[wildCard] = handle
	end
	return handle
end

function UiHandler:RemoveHandle(wildCard)
	if wildCard and self.uiHandles then
		local handle = self.uiHandles[wildCard]
		if not handle then return end
		self.uiHandles[wildCard] = nil
		return handle
	end
end

function UiHandler:RegisterButtonClick(button,onClick,wildCard)
	local handle = self:AddHandle(onClick,wildCard)
	LuaUtility.ButtonBindOnClick(button,handle)
end

function UiHandler:UnregisterButtonClick(button,wildCard)
    local handle = self:RemoveHandle(wildCard)
	LuaUtility.ButtonUnBindOnClick(button,handle)
end

function UiHandler:RegisterLongPress(button,onPress,wildCard)
	local handle = self:AddHandle(onPress,wildCard)
	LuaUtility.UiBindLongPress(button,handle)
end

function UiHandler:UnRegisterLongPress(button,wildCard)
	local handle = self:RemoveHandle(wildCard)
	LuaUtility.UiUnBindLongPress(button,handle)
end

function UiHandler:RegisterLongPressCancel(button,onPressCancel,wildCard)
	local handle = self:AddHandle(onPressCancel,wildCard)
	LuaUtility.UiBindLongPressCancel(button,handle)
end

function UiHandler:UnRegisterLongPressCancel(button,wildCard)
	local handle = self:RemoveHandle(wildCard)
	LuaUtility.UiUnBindLongPressCancel(button,handle)
end

function UiHandler:BindPointerDown(uiElement,onPointerDown,wildCard)
	local handle = self:AddHandle(onPointerDown,wildCard)
	LuaUtility.UiBindOnPointerDown(uiElement,handle)
end

function UiHandler:UnbindPointerDown(uiElement,wildCard)
	local handle = self:RemoveHandle(wildCard)
	LuaUtility.UiBindOnPointerDown(uiElement,handle)
end

function UiHandler:BindPointerUp(uiElement,onPointerUp,wildCard)
	local handle = self:AddHandle(onPointerUp,wildCard)
	LuaUtility.UiBindOnPointerUp(uiElement,handle)
end

function UiHandler:UnbindPointerUp(uiElement,wildCard)
	local handle = self:RemoveHandle(wildCard)
	LuaUtility.UiBindOnPointerUp(uiElement,handle)
end

function UiHandler:RegisterDragEvents(handler, onClick, onBeginDrag, onDrag, onEndDrag, onDrop)
    LuaUtility.BindDragEvents(handler, bind(onClick,self), bind(onBeginDrag,self),
		bind(onDrag,self), bind(onEndDrag,self), bind(onDrop,self))
end

return UiHandler
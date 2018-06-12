local Insert,Remove,Index = table.insert,table.remove,table.index

local UiViewSequence = {
	top = {},
	normal = {},
}

function UiViewSequence:PushView(view,top)
	Insert(top and self.top or self.normal,view)
end

function UiViewSequence:RemoveView(view,top)
	local layer = top and self.top or self.normal
	local index = Index(layer,view)
	if index then
		Remove(layer,index)
	end
end

function UiViewSequence:OnViewOpen(view,top)
	local layer = top and self.top or self.normal
	local index = Index(layer,view)
	if index > 1 then
		for i=index-1,1,-1 do
			if layer[i]:IsLoaded() then
				local preIndex = layer[i]:GetSlibingIndex()
				view:SetSlibingIndex(preIndex+1)
				return
			end
		end
	end
	for i=index+1,#layer do
		if layer[i]:IsLoaded() then
			local nextIndex = layer[i]:GetSlibingIndex()
			view:SetSlibingIndex(nextIndex-1)
		end
	end
end

local UiView =class("UiView",require("app.ui.UiHandler"))
UiView.top = false

local LuaUtility = CS.Game.LuaUtility

function UiView:ctor(mnger)
	self.opened = false
	self.uiMnger = mnger
end

function UiView:Open()
	if self.opened then
		return
	end
	UiViewSequence:PushView(self,self.top)
	local SceneLoader = SceneLoader
	SceneLoader:AddOne(self.res)
	self.opened = true
	self.uiMnger:AddView(self)
	LuaUtility.LoadView(self.res,
		function (view)
			self.view = view
			if not self.opened then
				self.opened = true
				self:Close()
				return
			end
			UiViewSequence:OnViewOpen(self,self.top)
			view.injector:Inject(self)
			LuaUtility.DebuggerLog("open " .. self.res)
			self:OnOpen()
			SceneLoader:FinishOne(self.res)
		end,self.top)
end

function UiView:Close()
	if self.opened then
		UiViewSequence:RemoveView(self,self.top)
		self.opened = false
		self.uiMnger:RemoveView(self)
		self:OnClose()
		LuaUtility.DebuggerLog("close " .. self.res)
		if self.view then
			self.view:Release()
			self.view = nil
		end
	end
end

function UiView:OnOpen()
end

function UiView:OnClose()
end

function UiView:GetSlibingIndex()
	return self.view:GetSlibingIndex() + 1
end

function UiView:SetSlibingIndex(index)
	self.view:SetSlibingIndex(index - 1)
end

function UiView:DisplayAsLast()
	self.view:DisplayAsLast()
end

function UiView:DisplayAsFirst()
	self.view:DisplayAsFirst()
end

function UiView:IsLoaded()
	return self.view ~= nil
end

return UiView
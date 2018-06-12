local M = class("ViewManager")

function M:ctor()
	self.views = {}
end

function M:FindView(res)
	return self.views[res]
end

function M:AddView(view)
	self.views[view.res] = view
end

function M:RemoveView(view)
	self.views[view.res] = nil
end

function M:GetView(viewCls)
	local view = self:FindView(viewCls.res)
	if not view then
		view = viewCls.new(self)
	end
	return view
end

function M:OpenView(viewCls)
	if not viewCls.res then
		print("res not assigned")
		return
	end
	local view = self:GetView(viewCls)
	view:Open()
end

function M:OpenViewWith(viewCls,func,...)
	local view = self:GetView(viewCls)
	func(view,...)
	view:Open()
	return view
end

function M:CloseView(viewCls)
	if not viewCls.res then return end
	local view = self:FindView(viewCls.res)
	if not view then return end
	view:Close()
end

function M:CloseAll()
	for _,view in pairs(self.views) do
		view:Close()
	end
	self.views = {}
end

ViewManager = M.new()
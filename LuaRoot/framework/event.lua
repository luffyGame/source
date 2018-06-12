local list = require("framework.list")
local insert = table.insert

local event = class("event")
local ilist = ilist

function event:ctor()
	self.list = list.new()
	self.opList = {}
	self.lock = false
end

--创建事件监听
function event:CreateListener(func,obj)
	return {value = func,obj = obj,removed = true}
end
--添加事件监听
function event:AddListener(listener)
	if self.lock then
		insert(self.opList,function ()
			self.list:pushnode(listener)
		end)
	else
		self.list:pushnode(listener)
	end
end
--删除事件监听
function event:RemoveListener(listener)
	if self.lock then
		insert(self.opList,function ()
			self.list:remove(listener)
		end)
	else
		self.list:remove(listener)
	end
end
--注册回调作为事件监听
function event:RegisterListener(func,obj)
	local listener = self:CreateListener(func,obj)
	self:AddListener(listener)
	return listener
end
--取消回调监听
function event:UnregisterListener(func,obj)
	local listener = self:GetListener(func,obj)
	if not listener then return end
	self:RemoveListener(listener)
end

function event:Clear()
	self.list:clear()
	self.opList = {}
	self.lock = false
end
--获取回调对应的监听
function event:GetListener(func,obj)
	local _list = self.list
	for i,f in ilist(_list) do
		if i.value == func and i.obj == obj then
			return i
		end
	end
end

event.__call = function (self, ... )
	local _list = self.list	
	self.lock = true			

	for i, f in ilist(_list) do							
		f(i.obj,...)
	end	

	local opList = self.opList	
	self.lock = false		

	for i, op in ipairs(opList) do									
		op()
		opList[i] = nil
	end
end

_G["event"] = event
UpdateBeat = event.new()
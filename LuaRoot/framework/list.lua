local list = class("list")

function list:ctor()
	self:clear()
end

function list:clear()
	self.length = 0
	self._next = self --next node
	self._prev = self --tail node
end

function list:push(value)
	local node = {value = value}
	return self:pushnode(node)
end

function list:pushnode(node)
	self._prev._next = node
	node._next = self
	node._prev = self._prev
	self._prev = node
	self.length = self.length + 1
	node.removed = false
	return node
end

function list:remove(node)
	if node.removed then return end
	if self.length == 0 then return end
	node.removed = true
	local _prev = node._prev
	local _next = node._next
	_next._prev = _prev
	_prev._next = _next
	self.length = self.length-1
end

function list:next(iter)
	local _next = iter._next
	if _next ~= self then
		return _next,_next.value
	end
end

function list:head()
	return self._next.value
end

function list:tail()
	return self._prev.value
end

ilist = function (_list)
	return list.next,_list,_list
end

return list
local EventDispatcher = class("EventDispatcher")
local event = event

function EventDispatcher:ctor()
    self.events = {}
end

function EventDispatcher:AddEventListener(eventName,func,obj)
    local e = self.events[eventName]
    if not e then
        e = event.new()
        self.events[eventName] = e
    end
    e:RegisterListener(func,obj)
end

function EventDispatcher:RemoveEventListener(eventName,func,obj)
    local e = self.events[eventName]
    if not e then return end
    e:UnregisterListener(func,obj)
end

function EventDispatcher:FireEvent(eventName, ... )
    local e = self.events[eventName]
    if not e then return end
    e(...)
end

function EventDispatcher:RemoveEvent(eventName)
    self.events[eventName] = nil
end

function EventDispatcher:Clear()
    self.events = {}
end

function EventDispatcher.Extend(cls)
    cls.AddEventListener = function(self,eventName,func,obj)
        if not self.dispatcher then self.dispatcher = EventDispatcher.new() end
        self.dispatcher:AddEventListener(eventName,func,obj)
    end
    cls.RemoveEventListener = function(self,eventName,func,obj)
        if not self.dispatcher then return end
        self.dispatcher:RemoveEventListener(eventName,func,obj)
    end
    cls.FireEvent = function(self,eventName,...)
        if not self.dispatcher then return end
        self.dispatcher:FireEvent(eventName,...)
    end
end

return EventDispatcher
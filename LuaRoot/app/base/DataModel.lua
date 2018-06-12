--数据模型，支持数据导入、导出
--导出支持差异化方案
--数据的新建：new()->init()->postImport()
--数据的加载：new()->import()->postImport()
local DataModel = class("DataModel")
local EventDispatcher = require("framework.EventDispatcher")
local _G = _G
local pairs,ipairs,next = _G.pairs,_G.ipairs,_G.next
local bind = require("xlua.util").bind
local genId = _G.Global.GenId

--策划配置数据读取到各种cfg里
--要保存的数据在tag中标记，true表示本次有变更,false表示没变更，
function DataModel:ctor()
    self.tag = {id=false}
    self:MarkSave()
end

function DataModel:MarkSave()
end

--标记域需要保存，采用手动的方式为避免类覆盖，简化处理,todo
function DataModel:MarkFieldSave(clsFields)
    if not clsFields then return end
    for _,field in ipairs(clsFields) do
        self.tag[field] = false
    end
end

function DataModel:MarkOneFieldSave(field)
    self.tag[field] = false
end

---强制设所有数据为脏数据，谨慎使用
function DataModel:MarkAllDirty()
    for key,_ in pairs(self.tag) do
        self.tag[key] = true
    end
    self:OnChanged()
end

---导出，结果为table,modified=true同时输出有增量变更
function DataModel:Export(modified)
    if not self.dirty then
        return self.lastImport
    end
    self.dirty = false
    local data = self.lastImport or {}
    local mod = modified and {} or nil
    local tag = self.tag
    for key,changed in pairs(tag) do
        if changed then
            tag[key] = false
            data[key] = self[key]
            if modified then
                mod[key] = self[key]
            end
        end
    end
    self.lastImport = data
    return data,mod
end
---从table导入，
function DataModel:Import(data)
    self.dirty = false
    local tag = self.tag
    for key,val in pairs(data) do
        if tag[key] ~= nil then
            self[key] = val
            tag[key] = false
        end
    end
    self.lastImport = data --存储上次的
    self:PostImport()
end

function DataModel:IsDirty()
    return self.dirty
end

--用于获取配置数据，和一些初始化，方便计算
function DataModel:PostImport()
end

function DataModel:IsFieldMarkSave(field)
    return self.tag[field] ~= nil
end
--设置需要保存的数据
function DataModel:SetValue(field,value,noEvent)
    self[field] = value
    if self.tag[field] ~= nil then
        self.tag[field] = true
        self:OnChanged()
    end
    if not noEvent then
        self:FieldNotify(field,value)
    end
end

function DataModel:BindChangeNotify(func,caller)
    if caller then
        self.onChanged = bind(func,caller)
    else
        self.onChanged = func
    end
end

function DataModel:OnChanged()
    self.dirty = true
    if self.onChanged then
        self.onChanged(self)
    end
end

function DataModel:RegisterFieldNotify(fieldName,func,obj)
    if not self.eventDispatcher then
        self.eventDispatcher = EventDispatcher.new()
    end
    self.eventDispatcher:AddEventListener(fieldName,func,obj)
end

function DataModel:UnregisterFieldNotify(fieldName,func,obj)
    if not self.eventDispatcher then return end
    self.eventDispatcher:RemoveEventListener(fieldName,func,obj)
end

function DataModel:FieldNotify(fieldName,...)
    if not self.eventDispatcher then return end
    self.eventDispatcher:FireEvent(fieldName,...)
end

function DataModel:InitId()
    self:SetValue("id",genId(),true)
end

function DataModel:GetId()
    return self.id
end

return DataModel
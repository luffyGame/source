-------类的基本实现----------------
--对象拷贝
function clone(object)
    local lookup_table = {}
    local function _copy(object)
        if type(object) ~= "table" then
            return object
        elseif lookup_table[object] then
            return lookup_table[object]
        end
        local new_table = {}
        lookup_table[object] = new_table
        for key, value in pairs(object) do
            new_table[_copy(key)] = _copy(value)
        end
        return setmetatable(new_table, getmetatable(object))
    end
    return _copy(object)
end


--以classname为类名创建super的子类
function class(classname, super)
    local superType = type(super)
    local cls

    if superType ~= "table" then
        superType = nil
        super = nil
    end

    if super then
        cls = {}
        setmetatable(cls, {__index = super})
        cls.super = super
    else
        cls = {ctor = function() end}
    end

    cls.__cname = classname
    cls.__index = cls

    function cls.new(...)
        local instance = setmetatable({}, cls)
        instance.class = cls
        instance:ctor(...)
        return instance
    end

    return cls
end

--obj是否是className类
function iskindof(obj, className)
    local t = type(obj)
    if t == "table" then
        local mt = getmetatable(obj)
        while mt and mt.__index do
            if mt.__index.__cname == className then
                return true
            end
            mt = mt.super
        end
    end
    return false
end

---用src的方法扩展dist，多用于一些功能独立，避免继承深度太深的情况，类似c#的extend方法，在功能上依赖于文件模块，类似于c#的partial
function extendMethod(self,extends)
    if self and extends then
        for k,v in pairs(extends) do
            if not self[k] and type(v) == "function" then
                self[k] = v
            end
        end
    end
end
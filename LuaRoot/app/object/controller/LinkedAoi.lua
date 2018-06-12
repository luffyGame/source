--x,z十字链表，xNext,xPrev,zNext,zPrev，只做x,z，因为y轴方向量少
local abs = math.abs
local fields = {"xPrev","xNext","zPrev","zNext"}

local LinkedElement = class("LinkedElement")
function LinkedElement:ctor(entity)
    self.entity = entity
    self.entity.aoi = self
end
function LinkedElement:GetPos()
    return self.entity:GetPos()
end
function LinkedElement:GetOid()
    return self.entity.objId
end
function LinkedElement:Release()
    self.entity.aoi = nil
    self.entity = nil
end

local LinkedAoi = {}

function LinkedAoi:Init()
    self.isValidated = true
    self.head = {}
    self.tail = {}
    self.head.xNext = self.tail
    self.head.zNext = self.tail
    self.tail.xPrev = self.head
    self.tail.zPrev = self.head
end

function LinkedAoi:Release()
    self.head = nil
    self.tail = nil
    self.isValidated = false
end

function LinkedAoi:Insert(element)
    local cur = self.head.xNext
    local x = element:GetPos().x
    while cur ~= self.tail do
        if cur:GetPos().x > x then
            break
        end
        cur = cur.xNext
    end
    element.xNext = cur
    element.xPrev = cur.xPrev
    cur.xPrev.xNext = element
    cur.xPrev = element

    cur = self.head.zNext
    local z = element:GetPos().z
    while cur~= self.tail do
        if cur:GetPos().z > z then
            break
        end
        cur = cur.zNext
    end
    element.zNext = cur
    element.zPrev = cur.zPrev
    cur.zPrev.zNext = element
    cur.zPrev = element
end

function LinkedAoi:Remove(element)
    element.xPrev.xNext = element.xNext
    element.xNext.xPrev = element.xPrev
    element.zPrev.zNext = element.zNext
    element.zNext.zPrev = element.zPrev
    element.xPrev = nil
    element.xNext = nil
    element.zPrev = nil
    element.zNext = nil
end

function LinkedAoi:MoveSimple(element)
    self:Remove(element)
    self:Insert(element)
end

function LinkedAoi:Move(entity,oldPos)
    local element = entity.aoi
    if not element then return end
    local pos = element:GetPos()
    if pos.x ~= oldPos.x then
        self:MoveX(element,pos.x,pos.x>oldPos.x)
    end
    if pos.z ~= oldPos.z then
        self:MoveZ(element,pos.z,pos.z>oldPos.z)
    end
end

function LinkedAoi:MoveX(element,x,isBig)
    if isBig then
        local cur = element.xNext
        while cur~= self.tail do
            if cur:GetPos().x > x then
                break
            end
            cur = cur.xNext
        end
        if cur.xPrev ~= element then
            element.xPrev.xNext = element.xNext
            element.xNext.xPrev = element.xPrev
            cur.xPrev.xNext = element
            element.xPrev = cur.xPrev
            element.xNext = cur
            cur.xPrev = element
        end
    else
        local cur = element.xPrev
        while cur ~= self.head do
            if cur:GetPos().x < x then
                break
            end
            cur = cur.xPrev
        end
        if cur.xNext ~= element then
            element.xPrev.xNext = element.xNext
            element.xNext.xPrev = element.xPrev
            cur.xNext.xPrev = element
            element.xNext = cur.xNext
            element.xPrev = cur
            cur.xNext = element
        end
    end
end

function LinkedAoi:MoveZ(element,z,isBig)
    if isBig then
        local cur = element.zNext
        while cur~= self.tail do
            if cur:GetPos().z > z then
                break
            end
            cur = cur.zNext
        end
        if cur.zPrev ~= element then
            element.zPrev.zNext = element.zNext
            element.zNext.zPrev = element.zPrev
            cur.zPrev.zNext = element
            element.zPrev = cur.zPrev
            element.zNext = cur
            cur.zPrev = element
        end
    else
        local cur = element.zPrev
        while cur ~= self.head do
            if cur:GetPos().z < z then
                break
            end
            cur = cur.zPrev
        end
        if cur.zNext ~= element then
            element.zPrev.zNext = element.zNext
            element.zNext.zPrev = element.zPrev
            cur.zNext.zPrev = element
            element.zNext = cur.zNext
            element.zPrev = cur
            cur.zNext = element
        end
    end
end

function LinkedAoi:AddEntity(entity)
    if not self.isValidated then return end
    local element = LinkedElement.new(entity)
    self:Insert(element)
end

function LinkedAoi:RemoveEntity(entity)
    self:Remove(entity.aoi)
    entity.aoi:Release()
end

function LinkedAoi:FindNearest(entity,dist,filt)
    local element = entity.aoi
    if not element then return end
    local findElement = self:GetNearest(element,dist,filt)
    return findElement and findElement.entity
end

local function get1dNearest(field,isX,element,dist,filt)
    local epos = element:GetPos()
    local mindist,finded
    local cur = element[field]
    while cur.entity do
        local pos = cur:GetPos()
        if isX then
            if abs(epos.x - pos.x) > dist then
                break
            end
        else
            if abs(epos.z-pos.z) > dist then
                break
            end
        end
        local odist = epos:dist(pos)
        if odist <= dist and (not filt or filt(cur.entity)) then
            if not mindist or mindist>odist then
                finded = cur
                mindist = odist
            end
        end
        cur = cur[field]
    end
    return finded,mindist
end

function LinkedAoi:GetNearest(element,dist,filt)
    local finded
    local mindist = dist
    for i=1,4 do
        local one,onedist = get1dNearest(fields[i],i<3,element,mindist,filt)
        if one then
            mindist = onedist
            finded = one
        end
    end
    return finded
end

local function get1dInSight(field,isX,element,range,filt,cb)
    local epos = element:GetPos()
    local cur = element[field]
    while cur.entity do
        local pos = cur:GetPos()
        if isX then
            if abs(epos.x - pos.x) > range then
                break
            end
        else
            if abs(epos.z-pos.z) > range then
                break
            end
        end
        local delta = pos - epos
        delta.y = 0
        local odist = delta:magnitudeH()
        if odist <= range and (not filt or filt(cur.entity)) then
            cb(cur.entity,delta)
        end
        cur = cur[field]
    end
end

--找到视野内的
function LinkedAoi:GetInSight(element,range,filt,cb)
    for i=1,4 do
        get1dInSight(fields[i],i<3,element,range,filt,cb)
    end
end

--找到视野内的，结果回调cb
function LinkedAoi:FindInSight(entity,range,filt,cb)
    local element = entity.aoi
    if not element then return end
    self:GetInSight(element,range,filt,cb)
end

function LinkedAoi:GetInCircle(entity,radius,cb)
    local element = entity.aoi
    if not element then return end
    cb(entity)
    self:GetInSight(element,radius,nil,cb)
end

_G.LinkedAoi = LinkedAoi
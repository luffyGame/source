--包裹数据模块，它并不独立存在，或依附于背包、箱子，或依附于主角本体、僵尸尸体等
--需要关心的事件：1.哪个格子里的东西发生改变，2.哪类东西发生改变（目前不确定）
local StoreItemDataModel = class("StoreItemDataModel",require("app.base.DataModel"))

local _G = _G
local pairs,ipairs,next = _G.pairs,_G.ipairs,_G.next
local min = math.min
local insert = table.insert
local CfgData = _G.CfgData
local ItemDataModelFactory = _G.ItemDataModelFactory
local table = table
local tostring,tonumber = tostring,tonumber

function StoreItemDataModel:ctor()
    StoreItemDataModel.super.ctor(self)
    self.items = {} --所有元素都是ItemDataModel的子类型，index->item
    self.idIndexs = {} --item的id->index
    self.count = 0 --已经有摆放的格子数量
    self.indexTags = {} --记录哪些格子修改了
end

local saveFields = {"cap"}
function StoreItemDataModel:MarkSave()
    self:MarkFieldSave(saveFields)
end

function StoreItemDataModel:SetCap(num)
    self:SetValue("cap",num)
end

---导出数据，方案：导出所有格子发生变动的
function StoreItemDataModel:Export(modified)
    local data,mod = StoreItemDataModel.super.Export(self,modified)
    if not self.cellDirty then
        return data,mod
    end
    self.cellDirty = false
    local items = data.items
    if not items then
        items = {}
        data.items = items
    end
    local modItems
    if modified then
        mod = mod or {}
        modItems = {}
        mod.items = modItems
    end
    for index,gen in pairs(self.indexTags) do
        if gen then
            self.indexTags[index] = false
            local item = self.items[index]
            local indexStr = tostring(index)
            if item then
                local itemExport = item:Export()
                items[indexStr] = itemExport
                if modified then modItems[indexStr] = itemExport end
            else
                items[indexStr] = nil
                if modified then modItems[indexStr] = {} end
            end
        end
    end
    return data,mod
end

function StoreItemDataModel:Import(data)
    StoreItemDataModel.super.Import(self,data)
    if data.items then
        for indexStr,itemData in pairs(data.items) do
            if itemData.tid then
                local item = ItemDataModelFactory:CreateByTid(itemData.tid,false)
                item:Import(itemData)
                self:_PushItem(tonumber(indexStr),item)
            end
        end
    end
end

function StoreItemDataModel:_PushItem(index,item)
    self.items[index] = item
    self.idIndexs[item.id] = index
    self.count = self.count + 1
    item:BindChangeNotify(self.OnItemChanged,self)
end

--寻找位置放入物品，返回true放置成功，注意这个是不merge的
function StoreItemDataModel:_AddItem(item)
    local index = self:GetNextEmptyIndex()
    if index then
        self:_PushItem(index,item)
        self:OnItemChanged(item)
        return true
    end
end

function StoreItemDataModel:GetNextEmptyIndex()
    for i=1,self.cap do
        if not self.items[i] then
            return i
        end
    end
end

--采用合并优先的方式放入物品
function StoreItemDataModel:Merge(tid,num)
    local cfg = CfgData:GetItem(tid)
    local maxPile = cfg.pileUpValue or 1
    local left = num
    for index,item in pairs(self.items) do
        if item.tid == tid and item.count < maxPile then
            local merge = min(maxPile-item.count,left)
            item:SetCount(item.count + merge)
            left = left - merge
            if left == 0 then
                break
            end
        end
    end
    while left>0 do
        local item = ItemDataModelFactory:CreateByTid(tid,true,cfg)
        item:SetCount(min(maxPile,left))
        if not self:_AddItem(item) then
            break
        else
            left = left - item.count
        end
    end
    return left,cfg
end

--是否已经充满
function StoreItemDataModel:IsFull()
    return self.count == self.cap
end
--是否为空
function StoreItemDataModel:IsEmpty()
    return self.count == 0
end

--独立放入一个物体，仅用于不可堆叠的物品
function StoreItemDataModel:Add(item)
    if item:CanPile() then
        return false
    end
    return self:_AddItem(item)
end

function StoreItemDataModel:Remove(item)
    local index = self.idIndexs[item.id]
    if not index then return end
    item:BindChangeNotify()
    self.idIndexs[item.id] = nil
    self.items[index] = nil
    self.count = self.count - 1
    self:IndexNotify(index)
end
--检查是否有足够的数量，如果有传入havelist，会填充list
function StoreItemDataModel:Have(tid,num,havelist)
    local have = 0
    for _,item in pairs(self.items) do
        if item.tid == tid then
            have = have + item.count
            if havelist then
                insert(havelist,item)
            end
            if have >= num then
                return true
            end
        end
    end
end

--总数
function StoreItemDataModel:TotalNum(tid)
    local num = 0
    for _,item in pairs(self.items) do
        if item.tid == tid then
            num = num + item.count
        end
    end
    return num
end

--添加物品，不合并
function StoreItemDataModel:AddItem(tid,count)
    local cfg = CfgData:GetItem(tid)
    local item = ItemDataModelFactory:CreateByTid(tid,true,cfg)
    item:SetCount(count)
    self:_AddItem(item)
    return item
end

--复制一个Item，到另外一个包裹
function StoreItemDataModel:MoveItem(index,item)
    if item then
        self:_PushItem(index,item)
        self:OnItemChanged(item)
    else
        self:SetItem(index,item)
    end
end

--设置一个位置的Item
function StoreItemDataModel:SetItem(index,item)
    local oldItem = self.items[index]
    if oldItem then
        self.idIndexs[oldItem.id] = nil
    end
    self.items[index] = item
    if item ~= nil then
        self.idIndexs[item.id] = index
    end
    self:IndexNotify(index)
end


--消耗指定数量
function StoreItemDataModel:Consume(tid,num)
    local havelist = {}
    if not self:Have(tid,num,havelist) then
        return false
    end
    for _,item in ipairs(havelist) do
        local consume = min(item.count,num)
        num = num - consume
        if item.count == consume then
            self:Remove(item)
        else
            item:SetCount(item.count-consume)
        end
        if num == 0 then
            return true
        end
    end
end

function StoreItemDataModel:ConsumeById(id)
    local index = self:GetItemIndexById(id)
    if index then
        local item = self:GetItemByIndex(index)
        if item then
            self:Remove(item)
        end
    end
end

--根据配置ID得到物品的总数量
function StoreItemDataModel:GetItemCount(tid)
    local count = 0
    for _, v in pairs(self.items) do
        if v.tid == tid then
            count = count + v.count
        end
    end
    return count
end

function StoreItemDataModel:GetProduceSkillFrom(produceSkillIds)
    for i=1,#produceSkillIds do
        local skillId = produceSkillIds[i]
        for _,item in pairs(self.items) do
            if item.isWeapon then
                local skill = item:GetProduceSkill(skillId)
                if skill then return skill end
            end
        end
    end
end

function StoreItemDataModel:Update(deltaTime)
    for _,item in pairs(self.items) do
        if item.isWeapon then
            item:Update(deltaTime)
        end
    end
end

function StoreItemDataModel:GetItemByIndex(index)
    return self.items[index]
end

function StoreItemDataModel:GetItemIndexById(id)
    return self.idIndexs[id]
end

function StoreItemDataModel:Arrange()
    local newItems = {}
    local newIndex = 1

    --region 合并可以合并的相同物品
    for i = 1, self.cap do
        if self.items[i] and self.items[i]:CanPile() then
            for j = self.cap,i + 1,-1 do
                if self.items[j] and self.items[j].tid == self.items[i].tid then
                    if self.items[j].count <= self.items[i]:GetPileMax() - self.items[i].count then
                        self.items[i]:SetCount(self.items[i].count + self.items[j].count)
                        self:Remove(self.items[j])
                    else
                        self.items[j]:SetCount( self.items[j].count - (self.items[i]:GetPileMax() - self.items[i].count))
                        self.items[i]:SetCount(self.items[i]:GetPileMax())
                    end
                end
            end
        end

        if self.items[i] then
            newItems[newIndex] = self.items[i]
            newIndex = newIndex + 1
        end
    end
    --endregion

    --region 排序
    table.sort(newItems,function (a,b)
        return a.tid < b.tid
    end)

    self.items = newItems
    for i = 1, self.cap do
        self:SetItem(i,self.items[i])
    end
    --endregion
end

--============消息注册
--注册包裹格子变动事件
function StoreItemDataModel:RegisterIndexNotify(func,obj)
    self:RegisterFieldNotify("#index",func,obj)
end

function StoreItemDataModel: UnregisterIndexNotify(func,obj)
    self:UnregisterFieldNotify("#index",func,obj)
end

function StoreItemDataModel:IndexNotify(index)
    self.cellDirty = true
    self.indexTags[index] = true
    self:FieldNotify("#index",index)
end

function StoreItemDataModel:OnItemChanged(item)
    local index = self.idIndexs[item.id]
    self:IndexNotify(index)
end
--==============

function StoreItemDataModel:IsDirty()
    return self.dirty or self.cellDirty
end

return StoreItemDataModel
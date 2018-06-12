--宠物收藏包裹数据 类似背包格子
local PetStoreDataModel = class("PetStoreDataModel",require("app.base.DataModel"))
local PetItemDataModel = require("app.object.datamodel.zoon.PetItemDataModel")
local CfgData = CfgData
local saveFields = {"cap"}


function PetStoreDataModel:ctor()
	PetStoreDataModel.super.ctor(self)
    self.items = {} 
    self.idIndexs = {} 
    self.count = 0 
    self.indexTags = {} 
end

function PetStoreDataModel:SetCap(num)
    self:SetValue("cap",num)
end

function PetStoreDataModel:MarkSave()
    self:MarkFieldSave(saveFields)
end

function PetStoreDataModel:Import(data)
	print("PetStoreDataModel-import:",valStr(data))
    PetStoreDataModel.super.Import(self,data)
    if data.items then
        for indexStr,itemData in pairs(data.items) do
            if itemData.petId then
                local item = self:CreatByTid(itemData.petId,false)
                item:Import(itemData)
                self:PushItem(tonumber(indexStr),item)
            end
        end
    end
end

function PetStoreDataModel:Export(modified)
    local data,mod = PetStoreDataModel.super.Export(self,modified)
	print("PetStoreDataModel - Export:",self.cageDirty)
    if not self.cageDirty then
        return data,mod
    end
    self.cageDirty = false
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

--------------------------------
--创建宠物模板(根据cfg-pet创建)
function PetStoreDataModel:CreatByTid(tid,new)
	print("PetStoreDataModel--creat",tid)
	local cfg = CfgData:GetPet(tid)
    local dataModel = PetItemDataModel.new()
    dataModel:SetCfg(cfg)
	if new then
		dataModel:Init()
	end
    return dataModel
end

function PetStoreDataModel:PushItem(index,item)
    self.items[index] = item
    self.idIndexs[item.id] = index
    self.count = self.count + 1
    item:BindChangeNotify(self.OnItemChanged,self)
end

function PetStoreDataModel:GetNextEmpty()
	for i=1,self.cap do
        if not self.items[i] then
            return i
        end
    end
end

function PetStoreDataModel:AddItem(tid,index)
	print("additem:",tid,index)
	local index = index or self:GetNextEmpty()
	local item = self:CreatByTid(tid,true)
	if index then
		self:PushItem(index,item)
        self:OnItemChanged(item)
	end
	return item
end

function PetStoreDataModel:RemoveItem(item)
	local index = self.idIndexs[item.id]
    if not index then return end
    item:BindChangeNotify()
    self.idIndexs[item.id] = nil
    self.items[index] = nil
    self.count = self.count - 1
    self:IndexNotify(index)
end

function PetStoreDataModel:SetItem(index,item)
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

function PetStoreDataModel:SortItem()
    --幼年>>成年
	--高等级>>低等级
	--短时间成熟>>长时间成熟
end

--------------------------
function PetStoreDataModel:IsDirty()
    return self.dirty or self.cageDirty
end

function PetStoreDataModel:RegisterIndexNotify(func,obj)
    self:RegisterFieldNotify("#index",func,obj)
end

function PetStoreDataModel:UnregisterIndexNotify(func,obj)
    self:UnregisterFieldNotify("#index",func,obj)
end

function PetStoreDataModel:OnItemChanged(item)
    local index = self.idIndexs[item.id]
    self:IndexNotify(index)
end

function PetStoreDataModel:IndexNotify(index)
	print("idexNotity-----",index)
    self.cageDirty = true
    self.indexTags[index] = true
    self:FieldNotify("#index",index)
end


return PetStoreDataModel

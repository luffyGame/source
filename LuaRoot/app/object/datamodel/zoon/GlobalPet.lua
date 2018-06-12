local GlobalPet = {}
extendMethod(GlobalPet,require("app.base.Savable"))
local PetStoreDataModel = require("app.object.datamodel.zoon.PetStoreDataModel")


function GlobalPet:Create()
    self.collectpet = PetStoreDataModel.new()
	self.collectpet:SetCap(10)
	self.usepet = PetStoreDataModel.new()
	self.usepet:SetCap(4)
	--test
	self.collectpet:AddItem(1,1)
	self.usepet:AddItem(1,1)
end

function GlobalPet:Import(data)
	print("GlobalPet - import:",valStr(data))
    self.collectpet = PetStoreDataModel.new()
    if data.collectpet then
        self.collectpet:Import(data.collectpet)
    end
	
	self.usepet = PetStoreDataModel.new()
	if data.usepet then
		self.usepet:Import(data.usepet)
	end
	
	print("import--collectpet:",valStr(self.collectpet))
	print("import--usepet:",valStr(self.usepet))
end

function GlobalPet:PostLoadData()
    self.collectpet:BindChangeNotify(self.MarkDirty,self)
	self.usepet:BindChangeNotify(self.MarkDirty,self)
end

function GlobalPet:Export(modified)
	print("GlobalPet- Export:",modified)
    local data = self.lastSave or {}
    local mod
    local collectpet,collectpetMod = self.collectpet:Export(modified)
    data.collectpet = collectpet
    if collectpetMod then
		if not mod then mod = {} end
        --mod = {collectpet = collectpetMod}
		mod[collectpet] = collectpetMod
    end
	
	local usepet,usepetMod = self.usepet:Export(modified)
	data.usepet = usepet
	if usepetMod then
		if not mod then mod = {} end
		mod[usepet] = usepetMod
	end
	
    return data,mod
end
--------------------
function GlobalPet:GetCollectPet()
	if self.collectpet then
		return self.collectpet
	end
end

function GlobalPet:GetUsePet()
	if self.usepet then 
		return self.usepet
	end
end

_G.GlobalPet = GlobalPet
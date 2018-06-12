---
--- Description 宠物管理
--- Created by SunShubin.
--- DateTime: 2018/6/8 4:25 PM
---
local Pet = require("app.object.entity.Pet")
local hostPlayer = _G.HostPlayer

local PetManager = {
    pets = {}
}

local function createPet(owner, petData)
    local pet = Pet.new(owner, petData)
    pet:EnterStage()
    return pet
end

function PetManager:EnterStage()
    hostPlayer.dataModel:RegisterFightPetChanged(self.OnPetChanged)
    self.pets[hostPlayer.objId] = createPet(hostPlayer, hostPlayer.dataModel:GetFightPet())
end

function PetManager:Release()
    for _, pet in pairs(self.pets) do
        pet:Release()
    end
    self.pets = {}
    hostPlayer.dataModel:UnregisterFightPetChanged(self.OnPetChanged)
end

function PetManager:OnPetChanged(petData)

end

_G.PetManager = PetManager
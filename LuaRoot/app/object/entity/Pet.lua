---
--- Description Pet
--- Created by SunShubin.
--- DateTime: 2018/3/26 下午2:37
---
local Vector3 = Vector3
local Timer = Timer
local random = math.random
local Pet = class("Pet", require("app.object.entity.Object"))
extendMethod(Pet, require("app.object.entity.extends.MoverExtend"))
extendMethod(Pet, require("app.object.entity.extends.FighterExtend"))

Pet.isPet = true

local ViewSprite = require("app.object.view.ViewSprite")
local PetActor = require("app.object.controller.monster.MonsterActor")
local PetAI = require("app.object.controller.pet.PetAI")

local CfgData = _G.CfgData

Pet.Area = {
    AreaA = 1,
    AreaB = 2,
    AreaC = 3,
}

function Pet:ctor(owner, petData)
    Pet.super.ctor(self)
    self.owner = owner
    self.dataModel = petData
    self.dataModel:SetPos(owner:GetPos())
    self.actor = PetActor.new(self)
end

function Pet:Load()
    if self.view then
        return
    end
    self.view = ViewSprite.new(self)
    self.view:Load(self.dataModel:GetModel(), self.OnLoaded, self)
end

function Pet:OnLoaded()
    Pet.super.OnLoaded(self)
    self.AI = PetAI.new(self)
    self.view:SetScale(self.dataModel.scale, true, true)
    self.view:SetSpeed(self.dataModel:GetMoveSpeed())
    self:Ready()
end

function Pet:EnterStage()
    self:Load()
end

function Pet:StartFollow()
    self:StartFollowPos(self.owner:GetPos(), 2)
end

function Pet:OnFollowEnd()
    if self.AI then
        self.AI:MoveEnd()
    end
end

function Pet:OnFollowEnd()
    if self.AI then
        self.AI:MoveEnd()
    end
end

function Pet:Ready()
    self:EnableUpdate(true)
    self:InitPetPos()
    if self.actor then
        self.actor:Start()
    end
    if self.AI then
        self.AI:Start()
    end
end

function Pet:InitPetPos()
    local newPos = self.owner:GetPos()
    --宠物初始化的位置
    local offset = Vector3.new(1, 0, 1):mul(random(1, CfgData:GetAreaA()))
    newPos:add(offset)
    self:SetPos(newPos)
    self:SetDir(self.owner:GetDir())
end

function Pet:Update()
    local deltaTime = Timer.deltaTime
    if self.actor then
        self.actor:OnUpdate(deltaTime)
    end
    if self.AI then
        self.AI:OnUpdate(deltaTime)
    end
end

function Pet:GetArea()
    local distance = self:GetPos():distNH(self.owner:GetPos())
    local inArea
    if distance < CfgData:GetAreaA() then
        inArea = Pet.Area.AreaA
    elseif distance < CfgData:GetAreaB() then
        inArea = Pet.Area.AreaB
    else
        inArea = Pet.Area.AreaC
    end
    return inArea
end

function Pet:Release()
    self.AI = nil
    self:EnableUpdate(false)
    self:ReleaseMover()
    Pet.super.Release(self)
end

return Pet
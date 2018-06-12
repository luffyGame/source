local LuaUtility = CS.Game.LuaUtility
local _G = _G
local CfgData = _G.CfgData
local SelectController = require("app.object.controller.player.SelectController")
local PlayerActiveController = require("app.object.controller.player.PlayerActiveController")
local Radar = require("app.object.controller.Radar")

local HostPlayer = class("HostPlayer", require("app.object.entity.Player"))
extendMethod(HostPlayer, require("app.base.Savable"))
local Vector3 = Vector3
local Timer = Timer

function HostPlayer:ctor()
    HostPlayer.super.ctor(self)
    self.selector = SelectController.new(self)
    self.controller = PlayerActiveController.new(self)
    self.radar = Radar.new(self, 10)
end

function HostPlayer:OnLoaded()
    HostPlayer.super.OnLoaded(self)
    _G.SceneEnv:CameraFollow(self.view:GetSprite())
    self.selector:EnableTrigger(true)
    self:BindMoveCallback(true)
    self:SetSneak(false)
    self:SetSpeed(self.dataModel:GetRunSpeed())

end

function HostPlayer:Release()
    self:UnregisterDataNotify("pos", self.OnPosChange, self)
    _G.SceneEnv:CameraFollow(nil)
    self.selector:Release()
    self.radar:Release()
    HostPlayer.super.Release(self)
end

---===== Save Load
function HostPlayer:DoSaveData()
    return self:Export(true)
end

function HostPlayer:Create()
    self:Born(CfgData:GetSystemParam().initialModelId)
end

function HostPlayer:PostLoadData()
    self:BindChangeNotify(self.MarkDirty, self)
    self:RegisterBagNotify(self.MarkDirty, self)
    self:RegisterEquipNotify(self.MarkDirty, self)
end

---===============
function HostPlayer:OnPosChange(pos)
    if _G.SceneEnv:IsPosOut(pos) then
        _G.SceneLoader:LoadMap(true)
    end
end

function HostPlayer:EnterStage()
    self:Load()
    self:SetSceneEnterPos()
    self:RegisterDataNotify("pos", self.OnPosChange, self)
    self.selector:EnterScene()
end

function HostPlayer:Update()
    local deltaTime = Timer.deltaTime
    HostPlayer.super.Update(self, deltaTime)
    self.selector:OnUpdate(deltaTime)
    self.controller:OnUpdate(deltaTime)
    self.radar:OnUpdate(deltaTime)
end

function HostPlayer:GetUseTarget()
    return self.selector:GetUseTarget()
end

function HostPlayer:GetAtkTarget()
    return self.selector:GetAtkTarget()
end

function HostPlayer:Move(x, z, deltaTime)
    if not self.actor:CanRun() then
        return
    end
    if not self.controller:CanCancel() then
        return
    end
    self.controller:Cancel()
    local dir = Vector3.new(x, 0, z)
    if self.dataModel.enableConstraint then
        local ang = Vector3.Dot(dir, self.dataModel.constraintDir)
        local tempConstraintDir = self.dataModel.constraintDir:clone()
        local tempDir = dir:clone()
        dir = tempConstraintDir:mul(tempDir:magnitudeH()) * ang
    end

    self:SetDir(dir)
    self:SimpleMove(dir)
    if self:IsSneak() then
        self.actor:Run()
    else
        self.actor:Run()
    end
end

function HostPlayer:StopMove()
    if self.actor then
        if not self:IsDead() and self.actor:IsRun() then
            self:Idle()
        end
    end
end

function HostPlayer:SetConstraintDir(dir)
    self.dataModel.constraintDir = dir
    self.dataModel.enableConstraint = true
    self:SetCrawl(true)

end

function HostPlayer:CancelConstraint()
    self.dataModel.enableConstraint = false
    self:SetCrawl(false)
end

function HostPlayer:Use()
    if self:CanUse() then
        self.controller:Use(self:GetUseTarget())
    end
    self:SetSneak(false)
end

function HostPlayer:Atk()
    if self:CanAtk() then
        self.controller:Attack(self:GetAtkTarget())
    end
end

function HostPlayer:Attacking(isAttacking)
    self.controller:Attacking(isAttacking)
end

function HostPlayer:OnDeadBy(user)
    _G.ViewManager:OpenView(ViewPlayerDie)
end

function HostPlayer:OnFollowEnd()
    self.controller:OnFollowEnd()
end

function HostPlayer:AddItemToBag(tid, count)
    local left, itemCfg = self.dataModel.bag:Merge(tid, count)
    if itemCfg then
        _G.Pop:Tip(string.format(CfgData:GetText("you got %s "), CfgData:GetText(itemCfg.name)))
    end
    if left > 0 then
        _G.Pop:Warning(CfgData:GetText("bag full"))
    end
end

function HostPlayer:Relive()
    HostPlayer.super.Relive(self)
    self:SetSceneEnterPos()
end

function HostPlayer:SetSceneEnterPos()
    local enterPos, enterDir = _G.SceneEnv:GetHostEnterLoc()
    enterPos.y = _G.SceneEnv:GetTerrainHeight(enterPos.x, enterPos.z) ---todo:临时
    --enterPos = Util.GetNavMeshPos(enterPos)
    self:SetPos(enterPos)
    self:SetDir(enterDir)
end

_G.HostPlayer = HostPlayer.new()
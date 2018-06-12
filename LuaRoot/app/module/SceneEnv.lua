local SceneEnv = {}


local Vector3 = Vector3
local Random = math.random
local RandomF = Math.RandomF

local LuaUtility = CS.Game.LuaUtility
local getLinkedAoi,getBlockAoi = Global.GetLinkedAoi,Global.GetBlockAoi
local BORDER_OUT = 3

function SceneEnv:InitForStage(location)
    self.env = {}
    self:Init()
    self:InitStageLoc(location)
end

function SceneEnv:InitForMap()
    self.env = {}
    self.marker = {}
    self.loc = {}
    self:Init()
end

function SceneEnv:Init()
    LuaUtility.InjectSceneSet(self)
    LuaUtility.InitGlobalSetting(self.env.mainCamera,self.env.cameraFollow)
end

function SceneEnv:InitStageLoc(location)
    local stageLoc = location
    local minPos,maxPos = stageLoc.minPos,stageLoc.maxPos
    self.borderMin = Vector3.new(minPos[1],minPos[2],minPos[3])
    self.borderMax = Vector3.new(maxPos[1],maxPos[2],maxPos[3])
    self.borderSize = self.borderMax - self.borderMin
    self.stageLoc = stageLoc
    getLinkedAoi():Init()
    getBlockAoi():Init(self.borderMin,self.borderSize)
end

function SceneEnv:Release()
    getLinkedAoi():Release()
    getBlockAoi():Release()
    LuaUtility.InitGlobalSetting(nil)
    self.placeManager = nil
    self.env = nil
end

function SceneEnv:ReleaseForMap()
    self.marker = nil
    self.loc = nil
    self:Release()
end

function SceneEnv:ReleaseForStage()
    self.stageLoc = nil
    self.borderMin = nil
    self.borderMax = nil
    self.borderSize = nil
    self:Release()
end

function SceneEnv:GetHostEnterLoc()
    if self.stageLoc.enterPos then
        local enterPos = self.stageLoc.enterPos
        local enterDir = self.stageLoc.enterDir
        enterPos = Vector3.new(enterPos[1],enterPos[2],enterPos[3])
        enterDir = Vector3.new(enterDir[1],enterDir[2],enterDir[3])
        return enterPos,enterDir
    else
        local dist = self.stageLoc.randomDis
        local x,z
        local v = Random(0,1)
        if v > 0 then
            local xR = Random(0,1)
            local x1 = RandomF(self.borderMin.x, self.borderMin.x + dist)
            local x2 = RandomF(self.borderMax.x-dist,self.borderMax.x)
            x = xR>0 and x1 or x2
            z = RandomF(self.borderMin.z + dist, self.borderMax.z - dist)
        else
            local zR = Random(0,1)
            local z1 = RandomF(self.borderMin.z, self.borderMin.z + dist)
            local z2 = RandomF(self.borderMax.z-dist,self.borderMax.z)
            z = zR>0 and z1 or z2
            x = RandomF(self.borderMin.x + dist, self.borderMax.x - dist)
        end

        local enterPos = Vector3.new(x,0,z)
        local center = self.borderMin + self.borderMax
        center:mul(0.5)
        local dir = (center-enterPos):setNormalize()
        return enterPos,dir
    end
end

function SceneEnv:IsPosOut(pos)
    return pos.x < self.borderMin.x - BORDER_OUT or pos.z < self.borderMin.z - BORDER_OUT or
            pos.x > self.borderMax.x + BORDER_OUT or pos.z > self.borderMax.z + BORDER_OUT
end

function SceneEnv:GetBoderXZ()
    return self.borderSize.x,self.borderSize.z
end

function SceneEnv:GetPlayerOffsetXZ(pos)
    offsetX = self.borderMin.x + self.borderSize.x/2 - pos.x
    offsetZ = self.borderMin.z + self.borderSize.z/2 - pos.z
    return offsetX,offsetZ
end
--得到地形高度，如果地面是mesh，则通过LuaInject填写地面高度
function SceneEnv:GetTerrainHeight(x,z)
    if self.env.terrain then
        local posX,posY,posZ = LuaUtility.TransformGetPos(self.env.terrain)
        return posY
    else
        return LuaUtility.GetTerrainHeight(x,z)
    end
end

function SceneEnv:CameraFollow(spriteView)
    LuaUtility.FollowCameraFollow(self.env.cameraFollow,spriteView)
end

function SceneEnv:GetFollowCameraYRot()
    return LuaUtility.GetFollowCameraRotY(self.env.cameraFollow)
end

function SceneEnv:CameraFocus(locTrans)
    local x,y,z = LuaUtility.TransformGetPos(locTrans)
    LuaUtility.DragCameraFocus(self.env.cameraControl, x, y, z)
end

function SceneEnv:CameraSetPos(locTrans)
    local x,y,z = LuaUtility.TransformGetPos(locTrans)
    LuaUtility.DragCameraSetPos(self.env.cameraControl, x, y, z)
end

function SceneEnv:SetBuildmodelOffset()
    LuaUtility.SetBuildmodelOffset(self.env.cameraFollow)
end

function SceneEnv:RecoverBuildmodelOffset()
    if self.env then
        LuaUtility.RecoverBuildmodelOffset(self.env.cameraFollow)
    end
end

function SceneEnv:GetTriggerGroup()
    if self.env and self.env.triggers then
        return self.env.triggers
    end
    return nil
end

_G.SceneEnv = SceneEnv
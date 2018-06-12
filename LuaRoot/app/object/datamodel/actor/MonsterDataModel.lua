local MonsterDataModel = class("MonsterDataModel",require("app.object.datamodel.actor.ObjectDataModel"))
extendMethod(MonsterDataModel,require("app.object.datamodel.extends.MonsterFightExtend"))
extendMethod(MonsterDataModel,require("app.object.datamodel.extends.FightExtend"))
extendMethod(MonsterDataModel,require("app.object.datamodel.extends.SceneElementExtend"))

local _G = _G
local CfgData = _G.CfgData
local random = math.random

local MonsterType = {
    COMMON = 1,--小怪
    POWERFUL = 2,--精英
    BOSS = 3,
    TRAP = 4, --陷阱
}

local saveFields = {"tid","camp","locIndex","locGroup"}

function MonsterDataModel:MarkSave()
    MonsterDataModel.super.MarkSave(self)
    self:MarkFieldSave(saveFields)
    self:MarkFightSave()
end

function MonsterDataModel:Init(tid)
    self:InitId()
    self:SetValue("tid",tid,true)
    self:PostImport()
    self:Born()
end

function MonsterDataModel:SetLocInfo(locIndex,locGroup)
    self:SetValue("locIndex",locIndex,true)
    self:SetValue("locGroup",locGroup,true)
end

function MonsterDataModel:Export(modified)
    local data,mod = MonsterDataModel.super.Export(self,modified)
    local boxData,boxMod = self:ExportBox(modified)
    data.box = boxData
    if mod or boxMod then
        mod = mod or {}
        mod.box = boxMod
    end
    return data,mod
end

function MonsterDataModel:Import(data)
    MonsterDataModel.super.Import(self,data)
    if data.box then
        self:ImportBox(data.box)
    end
end

function MonsterDataModel:PostImport()
    self.cfg = CfgData:GetMonster(self.tid)
    self.modelCfg = CfgData:GetModel(self.cfg.resId)
    self.elementType = CfgData:GetMapItemType(self.cfg.monsterType)
    self:MonsterFightInit()
    self:ResetProp()
end

function MonsterDataModel:Born()
    self:ResetProp(true)
    self:SetValue("dead",false,true)
end

function MonsterDataModel:SetPos(pos,clone,noEvent)
    if not self.initPos and pos then
        self.initPos = pos
    end
    MonsterDataModel.super.SetPos(self, pos,clone,noEvent)
end

function MonsterDataModel:GetCfgScale()
    return self.modelCfg.scale
end

function MonsterDataModel:GetBaseProp(prop)
    return self.cfg[prop]
end

function MonsterDataModel:IsAtkSelectable()
    if self:IsTrap() then
        return false
    end
    return not self.dead
end

function MonsterDataModel:IsUseSelectable()
    return self.dead and self.box ~= nil and not self.box:IsEmpty()
end

function MonsterDataModel:GetUseIcon()
    --todo:箱子的使用图标
    return  CfgData:GetMapItemType(self:GetMonsterType()).act
end

function MonsterDataModel:GetFightSpeed()
    return self.cfg.moveFight
end
--得到移动速度（随机）
function MonsterDataModel:GetMoveSpeed()
    local move = self.cfg.move
    move = random(move[1] * 100,move[2] * 100) / 100
    return move
end

function MonsterDataModel:GetOutofBattleSpeed()
    return self.cfg.moveFight * CfgData:GetSettingBattle().goHomeSpeed / 100
end

function MonsterDataModel:GetSight(isNormal)
    return isNormal and self.cfg.warnArea[1] or self.cfg.warnArea[2]
end

function MonsterDataModel:GetCamp()
    return self.camp or self.cfg.camp
end

function MonsterDataModel:SetCamp(camp)
    self:SetValue("camp",camp,true)
end

function MonsterDataModel:GetMonsterType()
    return self.cfg.monsterType
end
--Boss
function MonsterDataModel:IsBoss()
    return self:GetMonsterType() == MonsterType.BOSS
end
--精英怪
function MonsterDataModel:IsPowerful()
    return self:GetMonsterType() == MonsterType.POWERFUL
end
--陷阱
function MonsterDataModel:IsTrap()
    return self:GetMonsterType() == MonsterType.TRAP
end

function MonsterDataModel:CanDismember()
    return self.cfg.saw
end

--设置开始战斗时候的位置
function MonsterDataModel:SetBeginBattlePos(pos)
    self.beginBattlePos = pos
end
function MonsterDataModel:GetBeginBattlePos()
    return self.beginBattlePos
end

function MonsterDataModel:OnUpdate(deltaTime)
    self:UpdateFight(deltaTime)
    self:MonsterFightUpdate(deltaTime)
end


--region 关卡相关配置 0--不动 1--圆圈内随机点 2--路径
function MonsterDataModel:GetPatrolType()
    return self.loc.patrolType
end
--巡逻路径点
function MonsterDataModel:GetPatrolPoints()
    return self.loc.patrolPoints
end
-- 0--Once 1 -- Loop 2 -- 闭环
function MonsterDataModel:GetPatrolWrap()
    return self.loc.wrapMode
end
--巡逻范围
function MonsterDataModel:GetPatrolRange()
    return self.loc.patrolRange
end
--返回怪物属于哪一组
function MonsterDataModel:GetGroup()
    return self.loc.monsterGroup
end

--返回怪物的触发事件
function MonsterDataModel:GetEventTriggers()
    return self.loc.trigger
end

--endregion

function MonsterDataModel:OnBoxClose(callBack)
    print("on box closed")
    if self.box then
        if self.box.count == 0 then
            self.box = nil
            if callBack then
                callBack()
            end
        end
    end
end

function MonsterDataModel:OnDead()
    self:InitBox()
end

return MonsterDataModel
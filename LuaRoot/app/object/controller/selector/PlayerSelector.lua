local TriggerSelector = require("app.object.controller.selector.TriggerSelector")
local Selector = require("app.object.controller.selector.Selector")

local ViewObject = require("app.object.view.ViewObject")
local LuaUtility = CS.Game.LuaUtility
local HostEvent = HostEvent
local Const = Const
local AssetType = Const.AssetType
local UseTag = Const.UseTag
local Global = Global
local getSceneItemMnger = Global.GetSceneItemMnger
local getMonsterMnger = Global.GetMonsterMnger
local getFurnitureMnger = Global.GetFurnitureMnger

local Camp = Const.Camp
--UseSelector选中的对象由触发器触发，
local UseSelector = class("UseSelector",TriggerSelector)

function UseSelector:EnterScene()
    self.selectMark = ViewObject.new()
    self.selectMark:DoLoad("use_select",LuaUtility.LoadBasicModel,self.OnSelectMarkLoaded,self,AssetType.MODEL_DUMMY)
end

function UseSelector:IsSelectable(entity)
    if self.user:IsCrawl() then return false end
    if entity.isItem then return entity:IsUseSelectable() end
    if entity.isMonster then return entity:IsUseSelectable() end
    return false
end

function UseSelector:BindTriggerCallback(cb)
    self.user:BindUseChecker(cb)
end

function UseSelector:OnTargetSelect()
    self.user:FireEvent(HostEvent.USE_SELECT)
end

function UseSelector:Select(id,useTag)
    if id < 0 or self.user:IsCrawl() then
        self:SetTarget()
    elseif useTag == UseTag.SCENE_ITEM then
        local sceneItem = getSceneItemMnger():GetItem(id)
        self:SetTarget(sceneItem,useTag)
    elseif useTag == UseTag.MONSTER then
        local monster = getMonsterMnger():GetMonster(id)
        self:SetTarget(monster,useTag)
    else--
        local furniture = getFurnitureMnger():GetFurniture(id)
        self:SetTarget(furniture,useTag)
    end
end

local AtkSelector = class("AtkSelector",Selector)

function AtkSelector:EnterScene()
    self.selectMark = ViewObject.new()
    self.selectMark:DoLoad("enemy_select",LuaUtility.LoadBasicModel,self.OnSelectMarkLoaded,self,AssetType.MODEL_DUMMY)
end

function AtkSelector:IsSelectable(entity)
    return entity.isMonster and entity:IsAtkSelectable() and Camp:IsEnemy(self.user:GetCamp(),entity:GetCamp())
end

function AtkSelector:OnTargetSelect()
    self.user:FireEvent(HostEvent.ATK_SELECT)
end

return {UseSelector = UseSelector,AtkSelector = AtkSelector}
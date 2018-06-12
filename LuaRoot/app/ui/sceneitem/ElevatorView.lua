---
--- Description 电梯界面
--- Created by SunShubin
--- DateTime: 2018/5/31 7:39 PM
---

local view = class("ElevatorView", require("app.ui.UiView"))
local LuaUtility = CS.Game.LuaUtility

view.res = "elevatorview"

function view:OnOpen()
    if not self.param then
        return
    end

    self.floors = {}
    for i, v in ipairs(self.param) do
        self.floors[i] = {}
        self.floors[i].luaItem = LuaUtility.CloneLuaItem(self.floorTemplate)
        self.floors[i].luaItem.Data:Inject(self.floors[i])

        if self.curStageID == v then
            LuaUtility.SetButtonInteractable(self.floors[i].floorBtn,false)
        end

        LuaUtility.TextSetTxt(self.floors[i].txt_floorId,i)
        self:RegisterButtonClick(self.floors[i].floorBtn,function ()
            self:ClickFloor(v)
        end)
    end
end

function view:OnClose()
    for i, _ in ipairs(self.floors) do
        self:UnregisterButtonClick(self.floors[i].floorBtn)
    end
    self.floors = nil
    self.param = nil
    self.curStageID = nil
end

function view:Init(curStageID,param)
    self.param = param
    self.curStageID = curStageID
end

function view:ClickFloor(stageID)
    Global.GetSceneLoader():LoadStageByStageId(stageID,true)
    ViewManager:CloseView(ElevatorView)
end

ElevatorView = view

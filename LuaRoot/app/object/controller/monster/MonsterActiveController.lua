---
--- Description 怪物的命令
--- Created by SunShubin.
--- DateTime: 2018/4/25 下午5:38
---

local metaCmds = require("app.object.controller.monster.MonsterMetaCmd")
local CmdMove,CmdIdle = metaCmds.CmdMove,metaCmds.CmdIdle
local ComposedCmd = require("app.object.controller.monster.MonsterComposeCmd")
local MonsterActiveController = class("MonsterActiveController",Commander)

function MonsterActiveController:MoveAndIdle(pos,time,callback)
    self:ExecCmd(ComposedCmd.CmdMoveAndIdle.new(pos,time,callback))
end

function MonsterActiveController:Move(pos,callback)
    self:ExecCmd(CmdMove.new(pos,callback))
end

function MonsterActiveController:Idle(time,callback)
    self:ExecCmd(CmdIdle.new(time,callback))
end

function MonsterActiveController:OnFollowEnd()
    if self.currentCmd and self.currentCmd.isMoving then
        self.currentCmd:OnFollowEnd()
    end
end

return MonsterActiveController
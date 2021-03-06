---
--- Generated by EmmyLua(https://github.com/EmmyLua)
--- Created by wade.
--- DateTime: 2018/4/26 下午2:16
---
local CmdD = CmdD
local CmdMoveAndIdle = class("CmdMoveAndIdle",CmdD)
local metaCmds = require("app.object.controller.monster.MonsterMetaCmd")
local CmdMove,CmdIdle = metaCmds.CmdMove,metaCmds.CmdIdle
CmdMoveAndIdle.isMoving = true
function CmdMoveAndIdle:ctor(pos,time,callback)
    self.pos = pos
    self.time = time
    self.callback = callback
end

function CmdMoveAndIdle:Start()
    local moveCmd = CmdMove.new(self.pos)
    self:PushCmd(CmdIdle.new(self.time,self.callback))
    self:PushCmd(moveCmd)
    CmdMoveAndIdle.super.Start(self)
end

function CmdMoveAndIdle:OnFollowEnd()
    if self.curCmd and self.curCmd.isMoving then
        self.curCmd:OnFollowEnd()
    end
end

return {CmdMoveAndIdle = CmdMoveAndIdle}
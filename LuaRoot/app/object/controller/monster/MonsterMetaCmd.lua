---
--- Description 怪物基础指令
--- Created by SunShubin.
--- DateTime: 2018/4/25 下午5:38
---
local Cmd = Cmd

local CmdMove = class("CmdMove",Cmd)
CmdMove.isMoving = true
function CmdMove:ctor(pos,callBack)
    self.pos = pos
    self.callBack = callBack
end
function CmdMove:Start()
    self.owner.actor:Run()
    self.owner:MoveToPos(self.pos)
end
function CmdMove:OnFollowEnd()
    self.isFinished = true
end
function CmdMove:Cancel()
    self.owner:CancelFollow()
end
function CmdMove:Over()
    if self.callBack then
        self.callBack()
    end
end

function CmdMove:CanTransTo(otherCmd)
    return true
end



local CmdIdle = class("CmdIdle",Cmd)

function CmdIdle:ctor(time,callBack)
    self.time = time
    self.callBack = callBack
end
function CmdIdle:Start()
    --print("call cmd idle start")
    self.owner.actor:Idle()
end

function CmdIdle:OnUpdate(deltaTime)
    if self.time then
        self.time = self.time - deltaTime
        if self.time <= 0 then
            self.isFinished = true
        end
    end
end

function CmdIdle:Over()
    --print("idle cmd over!")
    if self.callBack then
        self.callBack()
    end
end


return {CmdMove = CmdMove,CmdIdle=CmdIdle}
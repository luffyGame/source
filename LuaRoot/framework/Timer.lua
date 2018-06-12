---
---计时器，使用Loop,Once.FrameLoop,FrameOnce来创建，
---例子
---简单用法：一次性或有限次数的，Timer.Loop(10,cb,caller,2):Start(),
---管理开关的：local timer = Timer.Loop(4,cb,caller),timer:Start()或者 local timer = Timer.Loop(4,cb,caller):Start(),
---timer:Stop()
--- Created by wangliang.
--- DateTime: 2018/5/24 下午12:11
---
local UpdateBeat = UpdateBeat
local max = math.max

local Timer = class("Timer")
Timer.deltaTime = 0
Timer.unscaledDeltaTime = 0

local nextId = 1
local timers = {}

function Timer.SetDeltaTime(deltaTime,unscaledDeltaTime)
    Timer.deltaTime = deltaTime
    Timer.unscaledDeltaTime = unscaledDeltaTime
end

---循环，不填loop次数表示无限循环
function Timer.Loop(duration,cb,caller,loop)
    local timer = Timer.new(cb,caller)
    timer:Set(duration,loop or -1)
    return timer
end

---一次性时钟
function Timer.Once(duration,cb,caller)
    local timer = Timer.new(cb,caller)
    timer:Set(duration,1)
    return timer
end

---unscaled循环，不填loop次数表示无限循环
function Timer.UnscaledLoop(duration,cb,caller,loop)
    local timer = Timer.new(cb,caller)
    timer:Set(duration,loop or -1,true)
    return timer
end

---一unscaled次性时钟
function Timer.UnscaledOnce(duration,cb,caller)
    local timer = Timer.new(cb,caller)
    timer:Set(duration,1,true)
    return timer
end

---帧循环
function Timer.FrameLoop(duration,loop,cb,caller)
    local timer = Timer.new(cb,caller)
    timer:Set(duration,loop or -1,nil,true)
    return timer
end
---帧一次性的
function Timer.FrameOnce(duration,cb,caller)
    local timer = Timer.new(cb,caller)
    timer:Set(duration,1,nil,true)
    return timer
end

function Timer:ctor(cb,caller)
    self.cb = cb
    self.caller = caller
    self.id = nextId
    timers[nextId] = self
    nextId = nextId + 1
end

function Timer:Set(duration,loop,unscaled,framed)
    self.duration = duration
    self.loop = loop or 1
    self.unscaled = unscaled
    self.framed = framed
end

--delay为延迟，不填没有首次延迟
function Timer:Start(delay)
    if self:IsStopped() then return self end
    self.running = true
    if not self.handler then
        self.handler = UpdateBeat:RegisterListener(self.Update,self)
    end
    self.time = self.duration
    if delay then
        self.time = self.time + delay
    end
    return self
end

function Timer:Update()
    if not self.running then return end
    local delta
    if self.framed then
        delta = 1
    else
        delta = self.unscaled and Timer.unscaledDeltaTime or Timer.deltaTime
    end
    self:OnCd()
    self.time = self.time - delta
    if self.time <=0 then
        self:OnTime()
        if self.loop > 0 then
            self.loop = self.loop - 1
            self.time = self.time + self.duration
        end
        if self.loop == 0 then
            self:Stop()
        elseif self.loop < 0 then
            self.time = self.time + self.duration
        end
    end
end

function Timer:OnTime()
    self.cb(self.caller)
end

function Timer:Stop()
    if not self.id then return end
    self.running = false
    if self.cb then
        self.cb = nil
        self.caller = nil
    end
    if self.onCd then
        self.onCd = nil
        self.onCdCaller = nil
    end
    if self.handler then
        UpdateBeat:RemoveListener(self.handler)
        self.handler = nil
    end
    timers[self.id] = nil
    self.id = nil
end

function Timer:Pause()
    self.running = false
end

function Timer:Resume(restart)
    if restart then
        self:Start()
    else
        self.running = true
    end
end

function Timer:IsStopped()
    return self.id == nil
end

--注册时钟cd，用于响应当前已持续时间
function Timer:RegisterCd(onCd,caller)
    self.onCd = onCd
    self.onCdCaller = caller
    return self
end

function Timer:OnCd()
    if self.onCd then
        local cd = max(0,self.duration-self.time)
        if self.onCdCaller then
            self.onCd(self.onCdCaller,cd)
        else
            self.onCd(cd)
        end
    end
end

_G.Timer = Timer
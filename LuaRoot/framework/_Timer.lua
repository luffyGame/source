local UpdateBeat = UpdateBeat
local Timer = class("Timer")
local time = os.time
local floor = math.floor

--region Clock
local Clock = class("Clock")
function Clock:ctor(delay,callBack)
    self.delay = delay
    self.callBack = callBack
end

function Clock:SetDeltaCallBack(callBack)
    self.deltaCallBack = callBack
end

local function CreateClock(time,delay,callBack,loop)
    local clock = Clock.new(delay,callBack)
    clock.time = delay - time
    clock.loop = loop
    return clock
end

local function CreateFixedClock(time,delay,callBack,loop)
    local clock = Clock.new(delay,callBack)
    clock.loop = loop
    clock.time = time
    return clock
end
--endregion

function Timer:ctor()
	-- body
    self.updateHandle = UpdateBeat:RegisterListener(self.Update,self)
    self.clocks = {}
    self.fixedClocks = {}
end

function Timer:Release()
    UpdateBeat:RemoveListener(self.updateHandle)
    self.clocks = nil
    self.fixedClocks = nil
end

function Timer:GetCurrentTime()
    return time()
end

function Timer:Update(deltaTime)
    for i, v in pairs(self.clocks) do
        if not v.isPause then
            v.time = v.time - deltaTime
            if v.time <= 0 then
                v.callBack(time)
                if v.loop then
                    v.time = v.delay
                else
                    self.clocks[i] = nil
                end
            else
                if v.deltaCallBack then
                    v.deltaCallBack(v.time)
                end
            end
        end
    end

    for i, v in pairs(self.fixedClocks) do
        local tempTime = self:GetCurrentTime() - v.time
        if(tempTime >= v.delay) then
            local times = floor(tempTime / v.delay)
            for i = 1, times do
                v.callBack(time)
                if v.loop then
                    v.time = self:GetCurrentTime()
                else
                    self.fixedClocks[i] = nil
                end
            end
        else
            if v.deltaCallBack then
                v.deltaCallBack(v.delay - tempTime)
            end
        end
    end
end

function Timer:DeleteTimer(timerName)
    if self.clocks[timerName] then
        self.clocks[timerName] = nil
    end

    if self.fixedClocks[timerName] then
        self.fixedClocks[timerName] = nil
    end
end

function Timer:PauseTimer(timerName)
    if self.clocks[timerName] then
        self.clocks[timerName].isPause = true
    end
end

function Timer:ResumeTimer(timerName)
    if self.clocks[timerName] then
        self.clocks[timerName].isPause = false
    end
end

function Timer:AddTimer(timerName,time,delay,callBack)
    if not self.clocks[timerName] then
        local tempClock = CreateClock(time, delay,callBack,false)
        self.clocks[timerName] = tempClock
        return tempClock
    end

end

function Timer:AddLoopTimer(timerName, time,delay,callBack)
    if not self.clocks[timerName] then
        local tempClock = CreateClock(time, delay,callBack,true)
        self.clocks[timerName] = tempClock
        return tempClock
    end
end

function Timer:AddFixedTimer(timerName, time,delay,callBack)
    if not self.fixedClocks[timerName] then
        local tempClock = CreateFixedClock(time, delay,callBack,false)
        self.fixedClocks[timerName] = tempClock
        return tempClock
    end
end

function Timer:AddFixedLoopTimer(timerName, time,delay,callBack)
    if not self.fixedClocks[timerName] then
        local tempClock = CreateFixedClock(time, delay,callBack,true)
        self.fixedClocks[timerName] = tempClock
        return tempClock
    end
end

_G._Timer = Timer.new()
---
--- 由触发区来选择，
--- Created by wangliang.
--- DateTime: 2018/6/6 下午12:30
---
local TriggerSelector = class("TriggerSelector")
local bind = require("xlua.util").bind
function TriggerSelector:ctor(user)
    self.user = user
    self.selectMark = nil
end

function TriggerSelector:OnSelectMarkLoaded()
    self.selectMark:SetVisible(false)
end

function TriggerSelector:Release()
    if self.selectMark then
        self.selectMark:Release()
        self.selectMark = nil
    end
end

function TriggerSelector:EnableTrigger(benable)
    if benable then
        self:BindTriggerCallback(bind(self.Select,self))
    else
        self:BindTriggerCallback()
    end
end

function TriggerSelector:BindTriggerCallback(cb) end

function TriggerSelector:Select(id,useTag) end

--
function TriggerSelector:SetTarget(target,tag)
    if self.target ~= target or self.tag ~= tag then
        self.target = target
        self.tag = tag
        if self.selectMark then
            self:LocSelectMark(target)
        end
        self:OnTargetSelect()
    end
end

function TriggerSelector:OnTargetSelect()
end

function TriggerSelector:LocSelectMark(target)
    if not target then
        self.selectMark:SetVisible(false)
    else
        self.selectMark:SetVisible(true)
        self:UpdateMarkPos()
    end
end

function TriggerSelector:UpdateMarkPos()
    if self.target then
        local pos = self.target:GetPos(true)
        pos.y = pos.y + 0.01
        self.selectMark:SetPos(pos)
    end
end

return TriggerSelector
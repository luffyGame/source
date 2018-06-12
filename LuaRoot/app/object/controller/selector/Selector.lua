local Selector = class("Selector")
local bind = require("xlua.util").bind

local linkedAoi
function Selector:ctor(user,range)
    if not linkedAoi then
        linkedAoi = LinkedAoi
    end
    self.user = user
    self.selectMark = nil
    self.range = range or 5
end

function Selector:OnSelectMarkLoaded()
    self.selectMark:SetVisible(false)
end

function Selector:Release()
    self.selectFilter = nil
    if self.selectMark then
        self.selectMark:Release()
        self.selectMark = nil
    end
end

function Selector:GetSelectFilter()
    if not self.selectFilter then
        self.selectFilter = bind(self.IsSelectable,self)
    end
    return self.selectFilter
end

function Selector:IsSelectable(entity)
end

function Selector:SelectNearest()
    self:SetTarget(self:GetNearest())
end

function Selector:GetNearest()
    return linkedAoi:FindNearest(self.user,self.range,self:GetSelectFilter())
end

function Selector:SetTarget(target)
    if self.target ~= target then
        self.target = target
        if self.selectMark then
            self:LocSelectMark(target)
        end
        self:OnTargetSelect()
    end
end

function Selector:OnTargetSelect()
end

function Selector:LocSelectMark(target)
    if not target then
        self.selectMark:SetVisible(false)
    else
        self.selectMark:SetVisible(true)
        self:UpdateMarkPos()
    end
end

function Selector:UpdateMarkPos()
    if self.target then
        local pos = self.target:GetPos(true)
        pos.y = pos.y + 0.01
        self.selectMark:SetPos(pos)
    end
end

return Selector
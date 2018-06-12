local ObjectDataModel = class("ObjectDataModel",require("app.base.DataModel"))

--[[local saveFields = {"pos","dir"}

function ObjectDataModel:MarkSave()
    self:MarkFieldSave(saveFields)
end
]]--

function ObjectDataModel:SetPos(pos,clone,noEvent)
    if not pos then
        return
    end
    self:SetValue("pos",clone and pos:clone() or pos,noEvent)
end

function ObjectDataModel:GetPos(clone)
    if self.pos then
        return clone and self.pos:clone() or self.pos
    end
end

function ObjectDataModel:SetDir(dir,clone,noEvent)
    if not dir then return end
    self:SetValue("dir",clone and dir:clone() or dir,noEvent)
end

function ObjectDataModel:GetDir(clone)
    if self.dir then
        return clone and self.dir:clone() or self.dir
    end
end

function ObjectDataModel:SetScale(scale,clone,noEvent)
    if not scale then return end
    self:SetValue("scale",clone and scale:clone() or scale,noEvent)
end

function ObjectDataModel:GetScale(clone)
    if self.scale then
        return clone and self.scale:clone() or self.scale
    end
end

function ObjectDataModel:GetModel()
    return self.modelCfg.prefab
end

function ObjectDataModel:GetSpeed()
    return 5
end

return ObjectDataModel
local ViewSceneItem = class("ViewSceneItem", require("app.object.view.ViewObject"))
local LuaUtility = CS.Game.LuaUtility

--======resource
function ViewSceneItem:Load(res, cb, caller)
    self:DoLoad(res, LuaUtility.LoadSceneItem, cb, caller)
end

function ViewSceneItem:GetSceneItem()
    return self.view
end

function ViewSceneItem:PlayAni(ani)
    if self.view then
        return self.view:PlayAni(ani)
    end
end

function ViewSceneItem:SetUsable(usable)
    if self.view then
        self.view:SetUsable(usable)
    end
end

function ViewSceneItem:GetMount(mmount)
    if self.view then
        local mountT = self.view:GetMount(mmount)
        return mountT
    end
    return nil
end

function ViewSceneItem:ColliderEnable(benable)
    if self.view then
        self.view:ColliderEnable(benable)
    end
end


function ViewSceneItem:PlayDefaultAnim(fade)
    if self.view then
        if fade then
            return self.view:PlayDefaultAnim(fade)
        else
            return self.view:PlayDefaultAnim(0)
        end
    end
end

return ViewSceneItem
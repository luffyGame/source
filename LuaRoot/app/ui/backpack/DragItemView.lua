local view = class("DragItemView", require("app.ui.UiView"))
local LuaUtility = CS.Game.LuaUtility

view.res = "dragitemview"
--view.layer = view.LayerEnum.LAYER_UP

function view:OnOpen()
    --LuaUtility.ImgSetSprite(self.img_pickIcon, self.sprite, false)
end

function view:Init(sprite)
    self.sprite = sprite
end

function view:SetSprite(sprite)
	print("SetSprite:",sprite)
    self.sprite = sprite
    LuaUtility.ImgSetSprite(self.img_pickIcon, self.sprite, true)
    self:SetVisible(true)
end

function view:SetVisible(visible)
    LuaUtility.SetComponentEnabled(self.img_pickIcon,visible)
end



DragItemView = view


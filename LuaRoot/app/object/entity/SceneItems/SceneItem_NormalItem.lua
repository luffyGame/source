---
--- Description 一般的场景物体
--- Created by SunShubin.
--- DateTime: 2018/6/04 11:31 AM
---
local SceneItem_NormalItem = class("SceneItem_NormalItem",require("app.object.entity.SceneItem"))
local Vector3 = _G.Vector3

function SceneItem_NormalItem:OnLoaded()
    SceneItem_NormalItem.super.OnLoaded(self)
    if self.dataModel.parent then
        self.dataModel:SetPos(Vector3.zero())
        self.view:SetParent(self.dataModel.parent)
    end
end

return SceneItem_NormalItem

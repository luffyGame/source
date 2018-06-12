local SceneMap = class("SceneMap",require("app.scene.entity.Scene"))

function SceneMap:GetLevelName()
    return "map_a_001"
end

function SceneMap:Start()
    SceneMap.super.Start(self)
    SceneEnv:InitForMap()
    ViewManager:OpenView(ViewMap)
    ViewManager:OpenView(ViewMapUI)
end

function SceneMap:Leave()
    ViewManager:CloseView(ViewMap)
    ViewManager:CloseView(ViewMapUI)
    ViewManager:CloseView(ViewMenu)
    SceneEnv:ReleaseForMap()
end

return SceneMap
local ViewPlayerDie =class("ViewPlayerDie",require("app.ui.UiView"))

ViewPlayerDie.res = "viewplayerdie"

--===========================

function ViewPlayerDie:OnOpen()
    self:RegisterButtonClick(self.btn,self.OnReliveClick)
end

function ViewPlayerDie:OnClose()
    self:UnregisterButtonClick(self.btn)
end

function ViewPlayerDie:OnReliveClick()
    HostPlayer:Relive()
    self:Close()
end

_G.ViewPlayerDie = ViewPlayerDie

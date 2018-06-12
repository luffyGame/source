local TestImageShow =class("TestImageShow",require("app.ui.UiView"))

TestImageShow.res = "testimageshow"

--===========================

function TestImageShow:OnOpen()
    self:RegisterButtonClick(self.btn,self.OnReliveClick)
end

function TestImageShow:OnClose()
    self:UnregisterButtonClick(self.btn)
end

function TestImageShow:OnReliveClick()
    self:Close()
end

_G.TestImageShow = TestImageShow

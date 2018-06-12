local view = class("ViewBluePrints", require("app.ui.UiView"))
local ViewObject = require("app.object.view.ViewObject")
local CfgData = CfgData
local Const = Const
local UpdateBeat = UpdateBeat
local Timer = Timer
local LuaUtility = CS.Game.LuaUtility
local AssetType = Const.AssetType
view.res = "viewblueprints"
view.rotateSpd = 3

view.lastIndex = 0
local host
local bag
local ipairs = ipairs

local Page = {
    ALL = 1,
    WEAPON = 2,
    ARMOR = 3,
    OTHER = 4,
}


function view:SetButtonsState(page)
    LuaUtility.SetBtnExtendInteractable(self.btn_all, page ~= Page.ALL)
    LuaUtility.SetBtnExtendInteractable(self.btn_weapon, page ~= Page.WEAPON)
    LuaUtility.SetBtnExtendInteractable(self.btn_armor, page ~= Page.ARMOR)
    LuaUtility.SetBtnExtendInteractable(self.btn_other, page ~= Page.OTHER)
end

function view:OnOpen()
    self:_SetLabelName()
    host = HostPlayer
    bag = host.dataModel.bag

    self.pageIdx = 1
    self.deltaDraged = 0
    self:SetButtonsState(self.pageIdx)
    self.items = {}

    self.updateHandle = UpdateBeat:RegisterListener(self.Update,self)

    if not view.haveOpened then
        view.haveOpened = true
        view.data = CfgData:GetAllBlueprints()--蓝图所有物品

        --table.sort(view.data,function (a,b)
        --    if a.showInPlan and b.showInPlan then
        --        return  a.levelOpen < b.levelOpen
        --    else
        --        return false
        --    end
        --end)
    end


    self.data = {}
    self.data[Page.ALL] = {}
    self.data[Page.ALL].index = 1
    self.data[Page.WEAPON] = {}
    self.data[Page.WEAPON].index = 1
    self.data[Page.ARMOR] = {}
    self.data[Page.ARMOR].index = 1
    self.data[Page.OTHER] = {}
    self.data[Page.OTHER].index = 1

    for i,v in pairs(view.data) do
        if v.showInPlan then
            self.data[Page.ALL][self.data[Page.ALL].index] = view.data[i]
            self.data[Page.ALL].index = self.data[Page.ALL].index + 1
            if v.showInPlan == Page.WEAPON then
                self.data[Page.WEAPON][self.data[Page.WEAPON].index] = view.data[i]
                self.data[Page.WEAPON].index = self.data[Page.WEAPON].index + 1
            elseif v.showInPlan == Page.ARMOR then
                self.data[Page.ARMOR][self.data[Page.ARMOR].index] = view.data[i]
                self.data[Page.ARMOR].index = self.data[Page.ARMOR].index + 1
            elseif v.showInPlan == Page.OTHER then
                self.data[Page.OTHER][self.data[Page.OTHER].index] = view.data[i]
                self.data[Page.OTHER].index = self.data[Page.OTHER].index + 1
            end
        end
    end

    local onSetValue = function (...)
        self.OnSetValue(self,...)
    end
    local onClick = function(...)
        self.OnClick(self,...)
    end
    LuaUtility.BindLoopListEvent(self.ll_loopList,onSetValue)
    self.ll_loopList:Init(#self.data[self.pageIdx])
    LuaUtility.BindLoopListClick(self.ll_loopList,onClick)


    self:SetRightPart(view.lastIndex)
    LuaUtility.TextSetTxt(self.txt_build,CfgData:GetText("UIDesc_1205_desc"))
    LuaUtility.TextSetTxt(self.txt_viewtitle,CfgData:GetText("UIDesc_1204_desc"))
    self.go_process:SetActive(false)
    self:RegisterButtonClick(self.btn_build, self.OnCraftBtn)
    self:RegisterButtonClick(self.btn_all,self.OnBtnAll)
    self:RegisterButtonClick(self.btn_weapon,self.OnBtnWeapon)
    self:RegisterButtonClick(self.btn_armor,self.OnBtnArmor)
    self:RegisterButtonClick(self.btn_other,self.OnBtnOther)

    LuaUtility.BindDragEvents(self.event_dragHandler, nil, function(...)
        self:OnDragBegin(...)
    end, function(...)
        self:OnDrag(...)
    end, function(...)
        self:OnDragEnd(...)
    end, nil)
end

function view:Update()
    if not self.draged then
        if self.deltaDraged >= 0 then
            LuaUtility.Rotate(self.tran_modelRoot, -view.rotateSpd)
        else
            LuaUtility.Rotate(self.tran_modelRoot, view.rotateSpd)
        end

    end
end

function view:OnClose()
    ViewBluePrints.super.OnClose(self)
    self:UnregisterButtonClick(self.btn_build)
    self:UnregisterButtonClick(self.btn_all)
    self:UnregisterButtonClick(self.btn_weapon)
    self:UnregisterButtonClick(self.btn_armor)
    self:UnregisterButtonClick(self.btn_other)
    LuaUtility.UnBindLoopListEvent(self.ll_loopList)
    LuaUtility.UnBindLoopListClick(self.ll_loopList)

    UpdateBeat:RemoveListener(self.updateHandle)

    self.ll_loopList:ResetList(0)
    view.lastIndex = 0
    LuaUtility.BindDragEvents(self.event_dragHandler)
    if self.model then
        self.model:Release()
    end
end

function view:OnCraftBtn()
    --for i, v in ipairs(self.consumList) do
    --    bag:Consume(v,self.consumCountList[i])
    --end 临时
    bag:Merge(self:GetComposeTar(view.lastIndex),1)
    self.go_process:SetActive(true)
    self.go_build:SetActive(false)
    self:UpdateMaterials(view.lastIndex)
end

function view:OnBtnAll()
    self:SetPageIndex(Page.ALL)
end

function view:OnBtnWeapon()
    self:SetPageIndex(Page.WEAPON)
end

function view:OnBtnArmor()
    self:SetPageIndex(Page.ARMOR)
end

function view:OnBtnOther()
    self:SetPageIndex(Page.OTHER)
end

function view:SetPageIndex(pageIdx)
    self.pageIdx = pageIdx
    self:SetButtonsState(pageIdx)
    self:RefreshAllItems()
end

function view:OnSetValue(index,luaItem)
    if self.items[index] == nil then
        self.items[index] = {}
    end
    luaItem.Data:Inject(self.items[index])
    self:SetItemValue(index)
end

function view:RefreshAllItems()
    view.lastIndex = 0
    self.ll_loopList:ResetList(#self.data[self.pageIdx])
end

function view:SetItemValue(index)
    LuaUtility.SetComponentEnabled(self.items[index].txt_name,true)
    LuaUtility.SetComponentEnabled(self.items[index].img_icon,true)
    if(view.lastIndex ~= nil and view.lastIndex == index) then
        LuaUtility.ImgSetColor(self.items[index].img_icon,Const.Color.GREEN)
    else
        LuaUtility.ImgSetColor(self.items[index].img_icon,Const.Color.WHITE)
    end
    LuaUtility.ImgSetSprite(self.items[index].img_icon,self:GetBlueprintIcon(index))
    LuaUtility.TextSetTxt(self.items[index].txt_name, self:GetBluePrintsName(index))
end

local Mats = {"materials1","materials2","materials3","materials4","materials5","materials6","materials7","materials8","materials9","materials10"}
local MatCounts = {"quantity1","quantity2","quantity3","quantity4","quantity5","quantity6","quantity7","quantity8","quantity9","quantity10"}

local WarningType = {
    LvAndLearn = 1,
    Lv = 2,
    Learn = 3,
    NoWarning = 4,
}

function view:SetRightPart(index)
    local icon,des = self:GetBlueprintContent(index)
    LuaUtility.TextSetTxt(self.txt_title, self:GetBluePrintsName(index))
    LuaUtility.TextSetTxt(self.txt_description,des)
    local blueCfg = self:_GetData(index)

    local warningType = WarningType.NoWarning
    local lvReach = host.dataModel.level >= blueCfg.levelOpen
    local reachCondition = false
    if blueCfg.planCondition then
        reachCondition = host.dataModel:IsReachCondition(blueCfg.id)
    else
        reachCondition = true
    end

    if not lvReach and not reachCondition then
        warningType = WarningType.LvAndLearn
    elseif lvReach and not reachCondition then
        warningType = WarningType.Learn
    elseif reachCondition and not lvReach then
        warningType = WarningType.Lv
    end

    warningType = WarningType.NoWarning --临时

    self.go_warning:SetActive(warningType ~= WarningType.NoWarning)
    self.go_build:SetActive(warningType == WarningType.NoWarning)

    local str_warning
    if warningType == WarningType.LvAndLearn then
        str_warning = CfgData:GetText("UIDesc_1202_desc")
        str_warning = string.format(str_warning,blueCfg.levelOpen)
    elseif warningType == WarningType.Lv then
        str_warning = CfgData:GetText("UIDesc_1201_desc")
        str_warning = string.format(str_warning,blueCfg.levelOpen)
    elseif warningType == WarningType.Learn then
        str_warning = CfgData:GetText("UIDesc_1203_desc")
    end


    LuaUtility.TextSetTxt(self.txt_warning,str_warning)
    self:UpdateMaterials(index)
    self:LoadModel(index)
end

function view:UpdateMaterials(index)
    local matData = self:_GetData(index)
    local materialItems = {self.injector_mat00,self.injector_mat01,self.injector_mat02,
                           self.injector_mat03,self.injector_mat04,self.injector_mat05}
    local com = {}
    local matEnough = true
    self.consumList = {}
    self.consumCountList = {}
    for i, v in ipairs(materialItems) do
        materialItems[i]:Inject(com)
        if i <= #Mats and matData[Mats[i]] then
            local tempMat = matData[Mats[i]]
            local tempCount = matData[MatCounts[i]]

            if bag:GetItemCount(tempMat) < tempCount then
                matEnough = false
                LuaUtility.TextSetTxt(com.txt_count, "<color=red>".. bag:GetItemCount(tempMat).. "</color>/<color=green>" .. tempCount.."</color>")
            else
                LuaUtility.TextSetTxt(com.txt_count,  bag:GetItemCount(tempMat).. "/<color=green>" .. tempCount .."</color>")
            end
            print(tempMat)
            LuaUtility.ImgSetSprite(com.img_icon, CfgData:GetItem(tempMat).icon)
            LuaUtility.SetComponentEnabled(com.txt_count,true)
            LuaUtility.SetComponentEnabled(com.img_icon,true)
            self.consumList[i] = tempMat
            self.consumCountList[i] = tempCount
        else
            LuaUtility.SetComponentEnabled(com.txt_count,false)
            LuaUtility.SetComponentEnabled(com.img_icon,false)
        end
    end
    --LuaUtility.SetBtnExtendInteractable(self.btn_build,matEnough) 临时
end

function view:GetBluePrintsName(index)
    return CfgData:GetText(self:_GetData(index).name)
end

function view:GetBlueprintContent(index)
    local blueCfg = CfgData:GetBlueprint(self:_GetData(index).id)
    local composeTar = blueCfg.itemID
    local icon = nil --CfgData:GetItem(composeTar).icon
    local des =  blueCfg.desc
    return icon,CfgData:GetText(des)
end

function view:GetBlueprintIcon(index)
    local composeTar = self:GetComposeTar(index)
    if composeTar then
        local tarItem = CfgData:GetItem(composeTar)
        if tarItem then
            local icon = tarItem.icon
            return icon
        else
            print("<color=red>there is no id:",composeTar,"in items table</color>",index)
        end
    end
end

function view:GetComposeTar(index)
    return CfgData:GetBlueprint(self:_GetData(index).id).itemID
end

function view:GetComposeTarItem(index)
    local composeTar = self:GetComposeTar(index)
    if composeTar then
        local tarItem = CfgData:GetItem(composeTar)
        return tarItem
    end
    return nil
end

function view:_GetData(index)
    return self.data[self.pageIdx][index + 1]
end

function view:_SetLabelName()
    LuaUtility.TextSetTxt(self.txt_all,CfgData:GetText("UIDesc_1101_desc"))
    LuaUtility.TextSetTxt(self.txt_weapon,CfgData:GetText("UIDesc_1102_desc"))
    LuaUtility.TextSetTxt(self.txt_armor,CfgData:GetText("UIDesc_1103_desc"))
    LuaUtility.TextSetTxt(self.txt_other,CfgData:GetText("UIDesc_1104_desc"))
end


function view:LoadModel(index)
    if self.model then
        self.model:Release()
    end

    local composeTarItem = self:GetComposeTarItem(index)
    print(composeTarItem.id,index,composeTarItem.resId,CfgData:GetText(composeTarItem.name))
    local resId
    if composeTarItem then
        if composeTarItem.itemType == Const.ItemType.WEAPON then
            resId = CfgData:GetWeapon(composeTarItem.id).resId
        elseif composeTarItem.itemType == Const.ItemType.ARMOR then
            resId = CfgData:GetArmor(composeTarItem.id).resId
        elseif composeTarItem.itemType == Const.ItemType.BUILD then
            --resId = CfgData:GetSceneItem(composeTarItem.id).resId
        else
            resId = composeTarItem.resId
        end
    end
    local modelCfg = CfgData:GetModel(resId)
    if modelCfg and modelCfg.prefab then
        self.model = ViewObject.new()
        self.model:DoLoad(modelCfg.prefab,LuaUtility.LoadBasicModel,self.OnModelLoaded,self,AssetType.MODEL_EXHIBIT)
    end
end

function view:OnModelLoaded()
    self.model:SetParent(self.tran_modelRoot)
    LuaUtility.SetRendererLayer(self.tran_modelRoot,Const.UILayer)
end

function view:OnClick(index)
    LuaUtility.ImgSetColor(self.items[view.lastIndex].img_icon,Const.Color.WHITE)
    LuaUtility.ImgSetColor(self.items[index].img_icon,Const.Color.GREEN)
    view.lastIndex = index
    self:SetRightPart(index)
end

function view:OnDragBegin()
    self.draged = true
end

function view:OnDrag(deltaX)
    LuaUtility.Rotate(self.tran_modelRoot, -deltaX)
end

function view:OnDragEnd(deltaX)
    self.draged = false
    self.deltaDraged = deltaX
end


ViewBluePrints = view

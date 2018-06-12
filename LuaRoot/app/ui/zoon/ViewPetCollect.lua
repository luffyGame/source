local view = class("ViewPetCollect",require("app.ui.UiView"))
local PetCollectSlot = require("app.ui.zoon.PetCollectSlot")
local PetSkillSlot = require("app.ui.zoon.PetSkillSlot")
local PetCareSlot = require("app.ui.zoon.PetCareSlot")
local ViewManager = ViewManager
local luaUtility = CS.Game.LuaUtility
local Global = Global
local CfgData = CfgData
view.res = "petcollect"


function view:OnOpen()
	self.cageCount = 15 --收藏容量15  看家和跟随4
	self.collectPet = Global.GetGlobalPet():GetCollectPet()
	self.usePet = Global.GetGlobalPet():GetUsePet()
	print("onopen view pet collect:",valStr(self.collectPet),"---------",valStr(self.usePet))
	self.collectPet:RegisterIndexNotify(self.OnCollectChange,self)
	self.usePet:RegisterIndexNotify(self.OnUseChange,self)
	self.dragView = ViewManager:GetView(DragItemView)	
	
	self.collectItems = {}
	self.useItems = {}
	self.skills = {}
	self:RegisterEvent()
	self:Init()
end

function view:Init()	
	self:InitGlobal()
	self:InitCollect()
	self:InitCare()
	self:InitSkill()
	self:InitShow()
	self:SetShowInfo()
end

function view:InitGlobal()
	luaUtility.ComponentGameObjVisible(self.com_hint,false)
	luaUtility.ComponentGameObjVisible(self.com_free,false)
	luaUtility.ComponentGameObjVisible(self.com_rename,false)
	print("InitGlobal")
end

function view:InitShow(data)
	if data == nil then 
		if self.collectItems[1].data then 
			self.showPet = self.collectItems[1].data
		end 
	else
		self.showPet = data
	end
end

function view:InitCollect()
	local index  = 1
	luaUtility.SetLuaSimpleListCount(self.grid_data,self.cageCount)
	luaUtility.BindOnItemAdd(self.grid_data,function(item)
		self.collectItems[index] = self:CreatSlot(index,PetCollectSlot,item,self.collectPet)
		index = index + 1
	end)
	luaUtility.LuaSimpleListInit(self.grid_data)

end

function view:InitCare()
	self.useItems[1] =  self:CreatSlot(1,PetCareSlot,self.slot_care1,self.usePet)
	self.useItems[2] =  self:CreatSlot(2,PetCareSlot,self.slot_care2,self.usePet)
	self.useItems[3] =  self:CreatSlot(3,PetCareSlot,self.slot_care3,self.usePet)
	self.useItems[4] =  self:CreatSlot(4,PetCareSlot,self.slot_follow,self.usePet)
end

function view:InitSkill()
	self.skills[1] = self:CreatSlot(1,PetSkillSlot,self.slot_skill)
end

function view:CreatSlot(index,sl,item,data)
	local slot = sl.new(index,item,data,self)
	slot:Init()
	slot:SetData(slot.data,false)
	return slot
end

function view:SetShowInfo()
	luaUtility.TextSetTxt(self.txt_name,self.showPet.name)
	--luaUtility.ImgSetSprite(self.img_show,CfgData:GetItem()) --show的显示
	luaUtility.TextSetTxt(self.txt_lv,string.format("Lv.%d",self.showPet.level))
	luaUtility.TextSetTxt(self.txt_attack,self.showPet.attack)
	luaUtility.TextSetTxt(self.txt_defense,self.showPet.defense)
	luaUtility.TextSetTxt(self.txt_hp,self.showPet.hp.."/"..self.showPet.hp)
	--exp
	--Skill
	--skip
	--Time
end

-------------------------
function view:OnCloseView()
	ViewManager:CloseView(ViewPetCollect)
end

function view:OnSkip()
	
end

function view:OnCloseHint()
	luaUtility.ComponentGameObjVisible(self.com_hint,false)
end

function view:OnHint()
	luaUtility.ComponentGameObjVisible(self.com_hint,true)
end

function view:OnRename()
	luaUtility.ComponentGameObjVisible(self.com_hint,false)
	luaUtility.ComponentGameObjVisible(self.com_rename,true)
end

function view:OnFree()
	luaUtility.ComponentGameObjVisible(self.com_hint,false)
	luaUtility.ComponentGameObjVisible(self.com_free,true)
end

function view:OnFreeCancel()
	luaUtility.ComponentGameObjVisible(self.com_free,false)
end

function view:OnFreeOK()
	--删除当前宠物  todo
end

function view:OnRenameOk()
	--改名 todo
end

function view:OnRenameCancel()
	luaUtility.ComponentGameObjVisible(self.com_rename,false)
end

--收藏仓库变动
function view:OnCollectChange(index)
	print("collectPetChange:",index,self.collectPet.count)
end

--巡逻、跟随仓库变动
function view:OnUseChange(index)
	print("usePetChange:",index,self.usePet.count)
end
-------------------------
function view:OnClick(slot)
	
end

function view:OnChoosed(slot)

end

function view:OnBeginDrag(slot)
	print("OnBeginDrag:",slot.index)
end

function view:OnEndDrag(slot)

end

function view:OnDrop(slot)

end
-------------------------
function view:RegisterEvent()
	self:RegisterButtonClick(self.btn_close,self.OnCloseView)
	self:RegisterButtonClick(self.btn_skip,self.OnSkip)
	self:RegisterButtonClick(self.btn_closehint,self.OnCloseHint)
	self:RegisterButtonClick(self.btn_hint,self.OnHint)
	self:RegisterButtonClick(self.btn_rename,self.OnRename)
	self:RegisterButtonClick(self.btn_free,self.OnFree)
	self:RegisterButtonClick(self.btn_cancel,self.OnFreeCancel)
	self:RegisterButtonClick(self.btn_ok,self.OnFreeOK)
	self:RegisterButtonClick(self.btn_renamecancel,self.OnRenameCancel)
	self:RegisterButtonClick(self.btn_renameok,self.OnRenameOk)
end

function view:UnregisterEvent()
	self:UnregisterButtonClick(self.btn_close)
	self:UnregisterButtonClick(self.btn_skip)
	self:UnregisterButtonClick(self.btn_closehint)
	self:UnregisterButtonClick(self.btn_hint)
	self:UnregisterButtonClick(self.btn_rename)
	self:UnregisterButtonClick(self.btn_free)
	self:UnregisterButtonClick(self.btn_cancel)
	self:UnregisterButtonClick(self.btn_ok)
	self:UnregisterButtonClick(self.btn_renamecancel)
	self:UnregisterButtonClick(self.btn_renameok)
end
-------------------------
function view:OnClose()
	ViewManager:CloseView(DragItemView)
	self.collectPet:UnregisterIndexNotify(self.OnBagChange,self)
	self.usePet:UnregisterIndexNotify(self.OnProductChange,self)
	luaUtility.SetLuaSimpleListCount(self.grid_data,0)
	luaUtility.LuaSimpleListInit(self.grid_data)
end

ViewPetCollect = view
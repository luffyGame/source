--[[存储数据
    1.主角本体数据:本体数值（40+）,背包物品（id+tid+数量+耐久）*?、装备、buff状态（id+时长）*? = 100+？
    2.关卡数据 * 50 = 2300 + ？
    【
        1.id
        2.重置需要的时间
        3.进入的次数
        5.怪物数据： (id+tid+血量)*50 + 部分尸体包裹，召唤出来的怪不保存
        6.场景物品数据： (id+tid+血量)*100  + 部分箱子
     】
    3.大地图数据
    4.建筑数据 ：（id+tid+pos+dir+hp）* 500 = 2500？
    5.工作台数据：
        【
            1.剩余生成物品+工作进度+已产出物品 = (id+tid+数量)*?+ 1 +(id+tid+数量）*？
            2.野兽：（id+tid+血量 + 属性*11）= 20*数量
         】
    6.宠物数据 ： (id+tid ） + 技能*
    7.天赋数据 ： 天赋点？
    8.账号数据：账号、 游戏时长，nextId
]]

--[[存储时机
周期存储 + 立即存储
周期存储：以时间周期来存储
立即存储：若当前不在存储就立即存储，若在则设置下次存储时间间隔=一个小值

]]--
--[[存储方案
增量存储?

--[[同步差异化修改

]]
local pairs = pairs
local bind = require("xlua.util").bind
local rapidjson = require('rapidjson')
local LuaUtility = CS.Game.LuaUtility
local Global = Global
local UpdateBeat = UpdateBeat
local StorageEvent = StorageEvent
local getSystem = Global.GetSystem
local Timer = Timer
local Sync = require("app.module.storage.Sync")

--sync必须放在最后一个，这个保证修改的
local DATA_FILE = {"system","host","stage","globalmap","globalpet","sync"}
local SAVE_COUNT = #DATA_FILE

local Storage = {}
local SAVE_PERIOD_DURATION = 20
local SAVE_SMALL_DURATION = 0.5

function Storage:Init()
    self.savable = {}
    self.savable[DATA_FILE[1]] = getSystem()
    self.savable[DATA_FILE[2]] = Global.GetHost()
    self.savable[DATA_FILE[3]] = Global.GetStageAll()
    self.savable[DATA_FILE[4]] = Global.GetGlobalMap()
	self.savable[DATA_FILE[5]] = Global.GetGlobalPet()
    self.savable[DATA_FILE[6]]  = Sync

    LuaUtility.SetStorageCb(bind(self.OnLoad,self),bind(self.OnSave,self))
    self.updateHandle = UpdateBeat:RegisterListener(self.Update,self)
    Global:AddEventListener(StorageEvent.INST_SAVE,self.Save,self)

    Sync:Init()
end

function Storage:Release()
    Global:RemoveEventListener(StorageEvent.INST_SAVE,self.Save,self)
    LuaUtility.SetStorageCb()

    if self.updateHandle then
        UpdateBeat:RemoveListener(self.updateHandle)
        self.updateHandle = nil
    end

    Sync:Release()
end

function Storage:Save()
    if self.isLoading then
        return
    end
    if self.isSaving then
        print("set next save")
        self:SetNextSavePeriod(false)
        return
    end
    local needSave,needSync = self:NeedSave()
    if self:NeedSave() then
        print("start save")
        self.isSaving = true
        self.saveCount = SAVE_COUNT
        if needSync then
            getSystem():MarkNeedSaveTime() --存时间
        end
        for i=1,SAVE_COUNT do
            local file = DATA_FILE[i]
            self:SaveOne(self.savable[file],file)
        end
    else
        self:OnSaveDone()
    end
end

function Storage:NeedSave()
    local needSave
    for _,savable in pairs(self.savable) do
        if savable:IsDirty() then
            if savable == Sync then
                needSave = true
            else
                return true,true
            end
        end
    end
    return needSave,false
end

--执行存储，如果small表示仅仅导出mod,
function Storage:SaveOne(savable,file)
    local newSave,mod = savable:SaveData()
    if mod then
        Sync:PushDataMod(file,mod)
    end
    if not newSave then
        self.saveCount = self.saveCount - 1
        return
    end

    local saveStr = rapidjson.encode(newSave)
    LuaUtility.StartSave(file,saveStr)
    if savable == Sync then
        Sync:SyncData(saveStr)
    end
end

function Storage:Load()
    if self.isLoading or self.isSaving then return end
    print("start load")
    self.isLoading = true
    self.loadCount = SAVE_COUNT
    for i=1,SAVE_COUNT do
        LuaUtility.StartLoad(DATA_FILE[i])
    end
end

function Storage:OnLoad(file,dataStr)
    self.loadCount = self.loadCount - 1
    local savable = self.savable[file]
    local data
    if dataStr then
        data = rapidjson.decode(dataStr)
    end
    savable:LoadData(data)
    if self.loadCount == 0 then
        self.isLoading = false
        self:OnLoadDone()
    end
end

function Storage:OnLoadDone()
    print("load done")
    getSystem():NotifyPassTimeFromLastSave()
    self:EnableSave(true)
end

function Storage:OnSave(file,bSuc)
    self.saveCount = self.saveCount - 1
    if not bSuc then
        local savable = self.savable[file]
        savable:MarkDirty(true) --存储失败，等待下次存储
    end
    self:CheckSaveDone()
end

function Storage:CheckSaveDone()
    if self.isSaving and self.saveCount == 0 then
        self.isSaving = false
        self:OnSaveDone()
    end
end

function Storage:OnSaveDone()
    print("save done")
    self:StartNextSaveCounter()
end

function Storage:Update()
    if not self.canSave then return end
    if self.isLoading or self.isSaving then return end
    self.saveTime = self.saveTime - Timer.unscaledDeltaTime
    if self.saveTime <= 0 then
        self:Save()
    end
end

function Storage:EnableSave(bEnable)
    self.canSave = bEnable
    self:StartNextSaveCounter()
end

function Storage:SetNextSavePeriod(periodly)
    self.nextSavePeriod = periodly and SAVE_PERIOD_DURATION or SAVE_SMALL_DURATION
end

--启动下次存储计时
function Storage:StartNextSaveCounter()
    self.saveTime = self.nextSavePeriod or SAVE_PERIOD_DURATION
    self:SetNextSavePeriod(true)
end

_G.Storage = Storage
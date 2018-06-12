---
--- 将数据与服务器同步
--- 每次仅同步差异化数据和行为
--- 方案：每触发一次关键行为，存储模块会存储，同时生成数据差异导入到sync模块
--- sync会Push关键行为，并将关键行为和差异数据同步到服务器
--- 同步成功的差异数据会从本地删除，
--- sync本身会跟随存储模块存储未同步的信息，本次存储产生的差异也一并保存，保证sync和存储模块的相对独立
--- Created by wangliang
--- DateTime: 2018/5/26 下午3:17
---
local insert = table.insert
local LuaUtility = CS.Game.LuaUtility
local bind = require("xlua.util").bind
local rapidjson = require('rapidjson')

local Sync = {}
extendMethod(Sync,require("app.base.Savable"))

function Sync:Init()
    LuaUtility.SetSyncUrl("http://192.168.1.124:8080/v1/action/sync")
    LuaUtility.SetSyncCb(bind(self.OnSyncDone,self))
end

function Sync:Release()
    LuaUtility.SetSyncCb()
end

function Sync:Create()
    self.lastVersion = 0
    self.mods = {}
end

function Sync:StartNewMod()
    self.curMod = {}
    self:AllocModId()
end

function Sync:AllocModId()
    self.lastVersion = self.lastVersion + 1
    self.curMod.version = self.lastVersion
    self:MarkDirty()
end

function Sync:PushDataMod(field,mod)
    if not self.curMod then
        self:StartNewMod()
    end
    self.curMod[field] = mod
end

function Sync:EndDataMod()
    if self.curMod then
        print("endDataMod")
        insert(self.mods,self.curMod)
        self.curMod = nil
    end
end

function Sync:Export()
    self:EndDataMod() --sync为最后一个存储的，在此时可以将最后一次的差异存入
    local data = self.lastSave or {}
    data.lastVersion = self.lastVersion
    data.mods = self.mods
    return data
end

function Sync:Import(data)
    self.lastVersion = data.lastVersion
    self.mods = data.mods
end

function Sync:SyncData(dataStr)
    LuaUtility.HttpSync(dataStr)
end

function Sync:OnSyncDone(isSuc,msg)
    if isSuc then
        local ret = rapidjson.decode(msg)
        if ret.code == 0 then
            self.mods = {}
            self:MarkDirty()
        end
    end
    print(isSuc,msg)
end

return Sync
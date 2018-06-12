local ipairs,pairs = ipairs,pairs
local Find,Sub,Format = string.find,string.sub,string.format
local Insert = table.insert
local Floor = math.floor
local tostring = tostring

function table.index(_table,_val)
    for i,val in ipairs(_table) do
        if val == _val then
            return i
        end
    end
end

function string.split(str, delimiter)
    if not str or not delimiter or delimiter=='' then
        return false
    end
    local pos,arr = 0, {}
    for st,sp in function() return Find(str, delimiter, pos, true) end do
        Insert(arr, Sub(str, pos, st - 1))
        pos = sp + 1
    end
    Insert(arr, Sub(str, pos))
    return arr
end

--精确到小数点后3位
function math.exact3(val)
    return Floor(val*1000+0.5)/1000
end

--转为值字符串
function valStr(val,notOnlyData)
    local lookup_table = {}
    local function getStr(val,notOnlyData)
        local typ = type(val)
        if typ == "function" then
            return "func"
        elseif typ == "table" then
            if lookup_table[val] then
                return "table"
            else
                lookup_table[val] = true
                local str,bFirst = "{",true
                for k,v in pairs(val) do
                    if bFirst then
                        bFirst = false
                    else
                        str = str .. ","
                    end
                    if k == "super" or k == "class" then
                        if notOnlyData then
                            str = str .. Format("\"%s\":\"%s\"",k,v.__cname or "unknow")
                        end
                    elseif k == "__index" then
                        if notOnlyData then
                            str = str .. Format("\"%s\":\"%s\"",k,v.__cname or "unknow")
                        end
                    else
                        if type(v) == "function" then
                            if notOnlyData then
                                str = str .. Format("\"%s\":\"func\"",k,getStr(v))
                            end
                        elseif type(v) == "string" then
                            str = str .. Format("\"%s\":\"%s\"",tostring(k),v)
                        else
                            str = str .. Format("\"%s\":%s",tostring(k),getStr(v))
                        end
                    end

                end
                str = str .. "}"
                return str
            end
        else
            return tostring(val)
        end
    end
    return getStr(val,notOnlyData)
end
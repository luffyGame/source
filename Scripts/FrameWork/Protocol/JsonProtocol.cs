using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace FrameWork
{
    [System.Serializable]
    public abstract class JsonProtocol : Protocol
    {
        public virtual string JsonStr()
        {
            return JsonUtility.ToJson(this);
        }
        public virtual void Load(string json)
        {
            JsonUtility.FromJsonOverwrite(json, this);
        }
        public static T DecodeFromStr<T>(string json) where T:JsonProtocol
        {
            return JsonUtility.FromJson<T>(json);
        }
        public static T DecodeFromByte<T>(ByteBuf buf,int start,int count) where T : JsonProtocol
        {
            string str = Encoding.UTF8.GetString(buf.buf, start, count);
            return JsonUtility.FromJson<T>(str);
        }
        public override ByteBuf Serialize()
        {
            string str = JsonStr();
            int count = Encoding.UTF8.GetByteCount(str);
            ByteBuf buf = ByteCache.Alloc(count+4);
            buf.Write(type(),0);
            Encoding.UTF8.GetBytes(str, 0, str.Length, buf.buf, 4);
            buf.len = count+4;
            return buf;
        }
        public override void Deserialize(ByteBuf buf, int start, int count)
        {
            string str = Encoding.UTF8.GetString(buf.buf, start, count);
            Load(str);
        }
    }
}
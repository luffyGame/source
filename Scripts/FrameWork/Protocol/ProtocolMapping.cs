using System;
using System.Collections.Generic;

namespace FrameWork
{
    public class ProtocolMapping
    {
        private static ProtocolMapping s_instance = new ProtocolMapping();
        public static ProtocolMapping Instance { get { return s_instance; } }
        private readonly Dictionary<int, Type> id2Clazz = new Dictionary<int, Type>();
        protected ProtocolMapping() { }
        public void Register<T>(int type)
        {
            Type clazz = typeof(T);
            id2Clazz.Add(type, clazz);
        }
        public Protocol CreatePrtcl(int type)
        {
            if(!id2Clazz.ContainsKey(type))
            {
                Debugger.LogError("prtcl type = {0} unknow", type);
                return null;
            }
            Type clazz = id2Clazz[type];
            return Activator.CreateInstance(clazz) as Protocol;
        }
    }
}

using System;
using System.Collections.Generic;

namespace FrameWork
{
    public class NetStateMsg : Message
    {
        public enum NetCode:int
        {
            NONE = 0,
            ConnectSuccess = 1,
            ConnectFail = 2,
            NetError = 3,
        }
        public const int TYPE = -1;
        public NetCode code;
        public string msg;

        public override int type()
        {
            return TYPE;
        }
        
        public NetStateMsg ConnectSuccess()
        {
            code = NetCode.ConnectSuccess;
            return this;
        }
        public NetStateMsg ConnectFail(string msg)
        {
            code = NetCode.ConnectFail;
            this.msg = msg;
            return this;
        }
        public NetStateMsg Error(string msg)
        {
            code = NetCode.NetError;
            this.msg = msg;
            return this;
        }
    }
}

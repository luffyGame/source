using System;
using System.Collections.Generic;

namespace FrameWork
{
    public abstract class NetClient
    {
        public abstract void SendPrtcl(Protocol protocol);
        protected ByteBuf EncodeAndPackage(Protocol p)
        {
            ByteBuf buf = p.Serialize();
            ByteBuf zip = SimpleZlibExt.Zip(buf);
            //ByteBuf zip = DotNetZlibExt.ZipUnsafe(buf);
            ByteBuf ret = ByteCache.Alloc(zip.len + 4);
            ret.Write(zip.len,0);
            ret.len = 4;
            ret.Append(zip);
            buf.Dispose();
            zip.Dispose();
            return ret;
        }
        protected Protocol Decode(ByteBuf raw)
        {
            ByteBuf data = SimpleZlibExt.Unzip(raw);
            //ByteBuf data = DotNetZlibExt.UnzipUnsafe(raw);
            int prtclType = data.GetInt(0);
            Protocol p = ProtocolMapping.Instance.CreatePrtcl(prtclType);
            if (null != p)
            {
                p.Deserialize(data, 4, data.len - 4);
            }
            raw.Dispose();
            data.Dispose();
            return p;
        }
    }
}

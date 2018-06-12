using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace FrameWork
{
    public class SimpleZlibExt
    {
        public static ByteBuf Unzip(ByteBuf data)
        {
            try
            {
                SimpleZlib ds = new SimpleZlib(new MemoryStream(data.buf, 0, data.len), CompressionMode.Decompress);
                ByteBuf ret = null;
                ByteBuf buf = ByteCache.Alloc(1024);
                int len = ds.Read(buf.buf, 0, 1024);
                while (len > 0)
                {
                    buf.len = len;
                    if (ret == null)
                    {
                        ret = ByteCache.Alloc(len);
                    }
                    else if (ret.maxSize < len + ret.len)
                    {
                        ret = ByteCache.Reserve(ret, len + ret.len);
                    }
                    ret.Append(buf);
                    len = ds.Read(buf.buf, 0, 1024);
                }
                ds.Close();
                return ret;
            }
            catch (Exception e)
            {
                Debugger.LogError(e);
            }
            return null;
        }

        public static ByteBuf Zip(ByteBuf toCompress)
        {
            ByteBuf buf = ByteCache.Alloc(1024);
            SimpleZlib ds = new SimpleZlib(new MemoryStream(buf.buf), CompressionMode.Compress, true);
            ds.Write(toCompress.buf, 0, toCompress.len);
            MemoryStream ms = (MemoryStream)ds.BaseStream;
            ds.Close();
            buf.len = (int)ms.Position;
            ms.Close();
            return buf;
        }
    }
}

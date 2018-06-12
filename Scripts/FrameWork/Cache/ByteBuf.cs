using System;
using System.Collections.Generic;
using System.IO;

namespace FrameWork
{
    public class ByteBuf : IDisposable
    {
        public int len { get; set; }
        public bool IsEmpty { get { return len == 0; } }
        public int index { get; private set; }
        public byte[] buf { get; private set; }
        public int maxSize { get { return buf.Length; } }
        private bool disposed;
        private MemoryStream ms;
        public ByteBuf(int index,int l)
        {
            this.index = index;
            buf = new byte[l];
        }
        ~ByteBuf()
        {
            Dispose();
        }
        public void Init()
        {
            len = 0;
            disposed = false;
        }
        public void Dispose()
        {
            if (disposed)
                return;
            disposed = true;
            if(null!=ms)
                ms = null;
            ByteCache.Free(this);
        }
        
        public MemoryStream GetStream()
        {
            if(ms == null)
            {
                ms = new MemoryStream(buf);
            }
            return ms;
        }
        public override string ToString()
        {
            return BitConverter.ToString(buf, 0, len);
        }
        public ByteBuf Clone()
        {
            ByteBuf clone = ByteCache.Alloc(len);
            System.Buffer.BlockCopy(buf, 0, clone.buf, 0, len);
            clone.len = len;
            return clone;
        }
        public bool CanAppend(ByteBuf other,int offset = 0)
        {
            return maxSize >= len + other.len - offset;
        }
        public void Append(ByteBuf other,int offset = 0)
        {
            if (!CanAppend(other,offset))
                throw new Exception("ByteBuf append out of memory");
            GetStream().Seek(len, SeekOrigin.Begin);
            GetStream().Write(other.buf, offset, other.len-offset);
            len += (other.len-offset);
        }
        public int GetInt(int pos)
        {
            GetStream().Seek(pos, SeekOrigin.Begin);
            byte[] bytes = new byte[sizeof(int)];
            GetStream().Read(bytes, 0, sizeof(int));
            return BitConverterExt.BytesToInt32(bytes);
        }
        public void Write(int v,int pos)
        {
            GetStream().Seek(pos, SeekOrigin.Begin);
            byte[] bytes = BitConverterExt.Int32ToBytes(v);
            GetStream().Write(bytes, 0, bytes.Length);
        }
        public void Copy(ByteBuf src,int offset,int length)
        {
            System.Buffer.BlockCopy(src.buf, offset,buf,0,length);
            len = length;
        }
    }
    public class ByteCache
    {
        private static int[] allocSize = new int[] { 32,64,128, 256, 512, 1024, 2048,4096 };
        private const int size_len = 8;
        private static List<ByteBuf>[] caches = new List<ByteBuf>[size_len];
        public static ByteBuf Alloc(int len,bool clear = false)
        {
            int index = GetAllocIndex(len);
            ByteBuf ret = null;
            lock(caches)
            {
                if(caches[index] != null&&caches[index].Count>0)
                {
                    ret = caches[index][0];
                    caches[index].RemoveAt(0);
                    //Debugger.Log("{0} consume to {1}", index, caches[index].Count);
                }
            }
            if (null != ret)
            {
                if (clear)
                    Array.Clear(ret.buf, 0, ret.buf.Length);
                ret.Init();
            }
            else
            {
                ret = new ByteBuf(index, allocSize[index]);
            }
            return ret;
        }
        public static void Free(ByteBuf bb)
        {
            lock (caches)
            {
                if (caches[bb.index] == null)
                    caches[bb.index] = new List<ByteBuf>();
                caches[bb.index].Add(bb);
                //Debugger.Log("{0} produce to {1}", bb.index, caches[bb.index].Count);
            }
        }
        public static ByteBuf Reserve(ByteBuf src, int len)
        {
            ByteBuf dst = Alloc(len);
            if (dst.maxSize == src.maxSize)
                throw new Exception("Bytebuf reserve already to maxsize");
            System.Buffer.BlockCopy(src.buf, 0, dst.buf, 0, src.len);
            dst.len = src.len;
            src.Dispose();
            return dst;
        }
        private static int GetAllocIndex(int len)
        {
            for(int i=0;i<size_len;++i)
            {
                if (allocSize[i] >= len)
                    return i;
            }
            return size_len - 1;
        }
    }
}

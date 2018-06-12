using Ionic.Zlib;
using System;
using System.Collections.Generic;
using System.IO;

namespace FrameWork
{
    //这里强制认为是某个固定线程使用
    public class DotNetZlibExt
    {
        private static MemoryStream unzipOut = new MemoryStream();
        private static MemoryStream zipOut = new MemoryStream();
        public static ByteBuf UnzipUnsafe(ByteBuf data)
        {
            Unzip(data.buf, 0, data.len);
            int count = (int)unzipOut.Length;
            ByteBuf buf = ByteCache.Alloc(count);
            unzipOut.Seek(0, SeekOrigin.Begin);
            unzipOut.Read(buf.buf, 0, count);
            buf.len = count;
            unzipOut.SetLength(0);
            return buf;
        }

        public static ByteBuf ZipUnsafe(ByteBuf toCompress)
        {
            Zip(toCompress.buf, 0, toCompress.len);
            int count = (int)zipOut.Length;
            ByteBuf buf = ByteCache.Alloc(count);
            zipOut.Seek(0, SeekOrigin.Begin);
            zipOut.Read(buf.buf, 0, count);
            buf.len = count;
            zipOut.SetLength(0);
            return buf;
        }

        private static void Unzip(byte[] compressed, int start, int count)
        {
            using (ZlibStream deCompressor = new ZlibStream(unzipOut, CompressionMode.Decompress, CompressionLevel.Default,true))
            {
                deCompressor.Write(compressed, start, count);
            }
        }

        private static void Zip(byte[] buffer, int start, int count)
        {
            using (ZlibStream compressor = new ZlibStream(zipOut, CompressionMode.Compress, CompressionLevel.Default,true))
            {
                compressor.Write(buffer, start, count);
            }
        }
    }
}

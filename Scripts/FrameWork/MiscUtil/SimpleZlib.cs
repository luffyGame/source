using System;
using System.IO;
using System.IO.Compression;
using System.Net;

namespace FrameWork
{
	// TODO: 目前没有检验解压数据的checksum
	public class SimpleZlib: DeflateStream
	{
		static private readonly byte HeaderByte1 = 0x78;
		static private readonly byte HeaderByte2 = 0x9c;

		private Stream stream;
		private bool nowrap;
		private bool leaveOpen;
		private uint checksum = 1;
		private CompressionMode mode;

		public SimpleZlib (Stream stream, CompressionMode mode, bool leaveOpen = false, bool nowrap = false): base(stream, mode, true)
		{
			this.stream = stream;
			this.nowrap = nowrap;
			this.mode = mode;
			this.leaveOpen = leaveOpen;
			if (!nowrap) {
				if (mode == CompressionMode.Compress)
					wrapHeader ();
				else if (mode == CompressionMode.Decompress) {
					removeChecksum ();
					unwrapHeader ();
				}
			}
		}

		public override void Write(byte[] src, int src_offset, int count)
		{
			base.Write (src, src_offset, count);
			if (!nowrap) {
				checksum = Adler32 (checksum, src, src_offset, count);
			}
		}

		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback cback, object state)
		{
			if (!nowrap) {
				checksum = Adler32 (checksum, buffer, offset, count);
			}
			return base.BeginWrite (buffer, offset, count, cback, state);
		}

		public override void Close()
		{
			base.Close ();
			if (this.mode.Equals (CompressionMode.Compress)) {
				this.stream.Write(BitConverterExt.GetBytesWithNetworkOrder((int)checksum), 0, sizeof(uint));
			}
			if (!this.leaveOpen) {
				this.stream.Close ();
			}
		}

		private void wrapHeader()
		{
			this.BaseStream.WriteByte (HeaderByte1);
			this.BaseStream.WriteByte (HeaderByte2);
		}

		private void unwrapHeader()
		{
			byte h1 = (byte)this.BaseStream.ReadByte ();
			byte h2 = (byte)this.BaseStream.ReadByte ();
			if (h1 != HeaderByte1 || h2 != HeaderByte2)
				throw new Exception ("Error zlib stream header");
		}

		private void removeChecksum()
		{
			this.BaseStream.Seek (4, SeekOrigin.End);
			byte[] buffer = new byte[4];
			this.BaseStream.Read (buffer, 0, 4);
			this.checksum = (uint)BitConverterExt.GetIntWithNetworkOrder (buffer);
			this.BaseStream.Seek (0, SeekOrigin.Begin);
		}

//		private int Adler32(byte[] bytes) {
//			const uint a32mod = 65521;
//			uint s1 = 1, s2 = 0;
//			foreach (byte b in bytes) {
//				s1 = (s1 + b) % a32mod;
//				s2 = (s2 + s1) % a32mod;
//			}
//			return unchecked((int) ((s2 << 16) + s1));
//		}

		private uint Adler32(uint adler32, byte[] bytes, int offset, int count) {
			const uint a32mod = 65521;
			uint s1 = adler32 & 0xffff;
			uint s2 = (adler32 >> 16);
			int max = offset + count;
			for (int i = offset; i < max; ++ i) {
				byte b = bytes[i];
				s1 = (s1 + b) % a32mod;
				s2 = (s2 + s1) % a32mod;
			}
			return ((s2 << 16) + s1);
		}
	}
}


using System;
using System.Net;

namespace FrameWork
{
	public class BitConverterExt
	{
		public enum Endian
		{
			LITTLE = 0,
			BIG,
		}

		static public byte[] ShortToBytes(short value, Endian endian = Endian.BIG)
		{
			byte[] bytes = BitConverter.GetBytes (value);
			if (BitConverter.IsLittleEndian && !endian.Equals (Endian.LITTLE)) {
				Array.Reverse (bytes);
			}
			return bytes;
		}

		static public byte[] UShortToBytes(ushort value, Endian endian = Endian.BIG)
		{
			
			byte[] bytes = BitConverter.GetBytes (value);
			if (BitConverter.IsLittleEndian && !endian.Equals (Endian.LITTLE)) {
				Array.Reverse (bytes);
			}
			return bytes;
		}

		static public byte[] Int32ToBytes(int value, Endian endian = Endian.BIG)
		{
			byte[] bytes = BitConverter.GetBytes (value);
			if (BitConverter.IsLittleEndian && !endian.Equals (Endian.LITTLE)) {
				Array.Reverse (bytes);
			}
			return bytes;
		}

		static public byte[] UInt32ToBytes(uint value, Endian endian = Endian.BIG)
		{
			byte[] bytes = BitConverter.GetBytes (value);
			if (BitConverter.IsLittleEndian && !endian.Equals (Endian.LITTLE)) {
				Array.Reverse (bytes);
			}
			return bytes;
		}

		static public byte[] Int64ToBytes(long value, Endian endian = Endian.BIG)
		{
			byte[] bytes = BitConverter.GetBytes (value);
			if (BitConverter.IsLittleEndian && !endian.Equals (Endian.LITTLE)) {
				Array.Reverse (bytes);
			}
			return bytes;
		}

		static public byte[] UInt64ToBytes(ulong value, Endian endian = Endian.BIG)
		{
			byte[] bytes = BitConverter.GetBytes (value);
			if (BitConverter.IsLittleEndian && !endian.Equals (Endian.LITTLE)) {
				Array.Reverse (bytes);
			}
			return bytes;
		}
		
		static public short BytesToShort(byte[] bytes, int offset = 0, Endian endian = Endian.BIG)
		{
			int size = sizeof(short);
			byte[] tmp = new byte[size];
			System.Buffer.BlockCopy(bytes, offset, tmp, 0, size);
			if (BitConverter.IsLittleEndian && !endian.Equals (Endian.LITTLE)) {
				Array.Reverse (tmp);
			}
			return BitConverter.ToInt16(tmp, 0);
		}

		static public ushort BytesToUShort(byte[] bytes, int offset = 0, Endian endian = Endian.BIG)
		{
			int size = sizeof(ushort);
			byte[] tmp = new byte[size];
			System.Buffer.BlockCopy(bytes, offset, tmp, 0, size);
			if (BitConverter.IsLittleEndian && !endian.Equals (Endian.LITTLE)) {
				Array.Reverse (tmp);
			}
			return BitConverter.ToUInt16 (tmp, 0);
		}

		static public int BytesToInt32(byte[] bytes, int offset = 0, Endian endian = Endian.BIG)
		{
			int size = sizeof(int);
			byte[] tmp = new byte[size];
			System.Buffer.BlockCopy(bytes, offset, tmp, 0, size);
			if (BitConverter.IsLittleEndian && !endian.Equals (Endian.LITTLE)) {
				Array.Reverse (tmp);
			}
			return BitConverter.ToInt32(tmp, 0);
		}

		static public uint BytesToUInt32(byte[] bytes, int offset = 0, Endian endian = Endian.BIG)
		{
			int size = sizeof(uint);
			byte[] tmp = new byte[size];
			System.Buffer.BlockCopy(bytes, offset, tmp, 0, size);
			if (BitConverter.IsLittleEndian && !endian.Equals (Endian.LITTLE)) {
				Array.Reverse (tmp);
			}
			return BitConverter.ToUInt32(tmp, 0);
		}

		static public long BytesToInt64(byte[] bytes, int offset = 0, Endian endian = Endian.BIG)
		{
			int size = sizeof(long);
			byte[] tmp = new byte[size];
			System.Buffer.BlockCopy(bytes, offset, tmp, 0, size);
			if (BitConverter.IsLittleEndian && !endian.Equals (Endian.LITTLE)) {
				Array.Reverse (tmp);
			}
			return BitConverter.ToInt64(tmp, 0);
		}

		static public ulong BytesToUInt64(byte[] bytes, int offset = 0, Endian endian = Endian.BIG)
		{
			int size = sizeof(ulong);
			byte[] tmp = new byte[size];
			System.Buffer.BlockCopy(bytes, offset, tmp, 0, size);
			if (BitConverter.IsLittleEndian && !endian.Equals (Endian.LITTLE)) {
				Array.Reverse (tmp);
			}
			return BitConverter.ToUInt64(tmp, 0);
		}
		static public byte[] HexStrToBytes(string hexStr)
		{
			string[] ss = hexStr.Split(new char[] { '-' });
			byte[] ret = new byte[ss.Length];
			for (int i = 0; i < ss.Length; ++i)
				ret[i] = Convert.ToByte(ss[i],16);
			return ret;
		}
		
		static public byte[] GetBytesWithNetworkOrder(int value)
		{
			return BitConverter.GetBytes (IPAddress.HostToNetworkOrder (value));
		}

		static public int GetIntWithNetworkOrder(byte[] bytes)
		{
			int value = BitConverter.ToInt32 (bytes, 0);
			return IPAddress.NetworkToHostOrder (value);
		}
	}
}


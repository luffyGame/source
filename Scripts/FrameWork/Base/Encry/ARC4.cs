using System;
using System.Collections.Generic;

namespace Encry
{
	public class ARC4
	{
		private readonly byte[] noChangePerm = new byte[256];//say key
		private readonly byte[]	perm = new byte[256];//use for encry
		private byte x, y;
		public bool ready { get; private set; }
        public byte[] key { get; private set; }

		public ARC4(){
			ready = false;
		}
		public ARC4(byte[] key)
		{
			x = y = 0;
			KeySetup(key);
		}

		public void Process(byte[] buffer, int start, int count)
		{
			if(!ready||null == buffer)
				return;
			InternalTransformBlock(buffer, start, count, buffer, start);
		}
		
		public void KeySetup(byte[] key)
		{
            this.key = key;
			byte index1 = 0;
			byte index2 = 0;
			
			for (int counter = 0; counter < 256; counter++)
			{
				noChangePerm[counter] = (byte)counter;
			}
			x = 0;
			y = 0;
			for (int counter = 0; counter < 256; counter++)
			{
				index2 = (byte)(key[index1] + noChangePerm[counter] + index2);
				// swap byte
				byte tmp = noChangePerm[counter];
				noChangePerm[counter] = noChangePerm[index2];
				noChangePerm[index2] = tmp;
				index1 = (byte)((index1 + 1) % key.Length);
			}
			ready = true;
		}

		private void Reset()
		{
			Array.Copy(noChangePerm,perm,perm.Length);
			x = 0;
			y = 0;
		}
		
		private void InternalTransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
		{
			Reset();
			for (int counter = 0; counter < inputCount; counter++)
			{
				x = (byte)(x + 1);
				y = (byte)(perm[x] + y);
				// swap byte
				byte tmp = perm[x];
				perm[x] = perm[y];
				perm[y] = tmp;
				
				byte xorIndex = (byte)(perm[x] + perm[y]);
				outputBuffer[outputOffset + counter] = (byte)(inputBuffer[inputOffset + counter] ^ perm[xorIndex]);
			}
		}
	}
	
}
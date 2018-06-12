using System;
using System.Collections.Generic;
using UnityEngine;

namespace FrameWork.MathF
{
    public sealed class Util
    {
        public static int BYTE_BIT = 8;
        public static int Abs(int a)
        {
            byte[] bs = BitConverter.GetBytes(a);
            Debug.Log(BitConverter.ToString(bs));
            int mask = (a >> (sizeof(int) * BYTE_BIT - 1));
            Debug.Log(mask);
            return (a + mask) ^ mask;
        }
    }
}

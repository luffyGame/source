using System;
using System.Collections.Generic;

namespace FrameWork.MathF
{
    public sealed class FixMath
    {
        public static FP Pi = FP.Pi;
        public static FP PiOver2 = FP.PiOver2;
        public static FP Epsilon = FP.Epsilon;
        public static FP Deg2Rad = FP.Deg2Rad;
        public static FP Rad2Deg = FP.Rad2Deg;
        public static FP Sqrt(FP number)
        {
            return FP.Sqrt(number);
        }
        public static FP Max(FP val1, FP val2)
        {
            return (val1 > val2) ? val1 : val2;
        }
        public static FP Min(FP val1, FP val2)
        {
            return (val1 < val2) ? val1 : val2;
        }
        public static FP Max(FP val1, FP val2, FP val3)
        {
            FP max12 = (val1 > val2) ? val1 : val2;
            return (max12 > val3) ? max12 : val3;
        }
        public static FP Clamp(FP value, FP min, FP max)
        {
            value = (value > max) ? max : value;
            value = (value < min) ? min : value;
            return value;
        }
    }
}

using System;
using System.Collections.Generic;

namespace FrameWork.MathF
{
    public struct FixVector
    {
        private static FP ZeroEpsilonSq = FixMath.Epsilon;
        public FP x;
        public FP y;
        public FP z;
        #region Static readonly variables
        public static readonly FixVector zero;
        public static readonly FixVector left;
        public static readonly FixVector right;
        public static readonly FixVector up;
        public static readonly FixVector down;
        public static readonly FixVector back;
        public static readonly FixVector forward;
        public static readonly FixVector one;
        public static readonly FixVector MinValue;
        public static readonly FixVector MaxValue;

        static FixVector()
        {
            one = new FixVector(1, 1, 1);
            zero = new FixVector(0, 0, 0);
            left = new FixVector(-1, 0, 0);
            right = new FixVector(1, 0, 0);
            up = new FixVector(0, 1, 0);
            down = new FixVector(0, -1, 0);
            back = new FixVector(0, 0, -1);
            forward = new FixVector(0, 0, 1);
            MinValue = new FixVector(FP.MinValue);
            MaxValue = new FixVector(FP.MaxValue);
        }
        #endregion
        public FixVector(int x, int y, int z)
        {
            this.x = (FP)x;
            this.y = (FP)y;
            this.z = (FP)z;
        }
        public FixVector(FP x, FP y, FP z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public FixVector(FP xyz)
        {
            this.x = xyz;
            this.y = xyz;
            this.z = xyz;
        }
        public static FixVector Abs(FixVector other)
        {
            return new FixVector(FP.Abs(other.x), FP.Abs(other.y), FP.Abs(other.z));
        }
        public FP sqrMagnitude
        {
            get
            {
                return (((this.x * this.x) + (this.y * this.y)) + (this.z * this.z));
            }
        }
        public FP magnitude
        {
            get
            {
                FP num = ((this.x * this.x) + (this.y * this.y)) + (this.z * this.z);
                return FP.Sqrt(num);
            }
        }
        public FixVector normalized
        {
            get
            {
                FixVector result = new FixVector(this.x, this.y, this.z);
                result.Normalize();

                return result;
            }
        }
        public void Scale(FixVector other)
        {
            this.x = x * other.x;
            this.y = y * other.y;
            this.z = z * other.z;
        }
        public void Set(FP x, FP y, FP z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public void Normalize()
        {
            FP num2 = ((this.x * this.x) + (this.y * this.y)) + (this.z * this.z);
            FP num = FP.One / FP.Sqrt(num2);
            this.x *= num;
            this.y *= num;
            this.z *= num;
        }
        public override string ToString()
        {
            return string.Format("({0:f1}, {1:f1}, {2:f1})", x.AsFloat(), y.AsFloat(), z.AsFloat());
        }
        public override bool Equals(object obj)
        {
            if (!(obj is FixVector)) return false;
            FixVector other = (FixVector)obj;

            return (((x == other.x) && (y == other.y)) && (z == other.z));
        }
        public override int GetHashCode()
        {
            return x.GetHashCode() ^ y.GetHashCode() ^ z.GetHashCode();
        }
        public void MakeZero()
        {
            x = FP.Zero;
            y = FP.Zero;
            z = FP.Zero;
        }
        public void Negate()
        {
            this.x = -this.x;
            this.y = -this.y;
            this.z = -this.z;
        }
        public bool IsZero()
        {
            return (this.sqrMagnitude == FP.Zero);
        }
        public bool IsNearlyZero()
        {
            return (this.sqrMagnitude < ZeroEpsilonSq);
        }
        public static FixVector Lerp(FixVector from, FixVector to, FP percent)
        {
            return from + (to - from) * percent;
        }
        
        public static FixVector Scale(FixVector vecA, FixVector vecB)
        {
            FixVector result;
            result.x = vecA.x * vecB.x;
            result.y = vecA.y * vecB.y;
            result.z = vecA.z * vecB.z;

            return result;
        }
        public static bool operator ==(FixVector value1, FixVector value2)
        {
            return (((value1.x == value2.x) && (value1.y == value2.y)) && (value1.z == value2.z));
        }
        public static bool operator !=(FixVector value1, FixVector value2)
        {
            if ((value1.x == value2.x) && (value1.y == value2.y))
            {
                return (value1.z != value2.z);
            }
            return true;
        }
        public static FixVector Min(FixVector value1, FixVector value2)
        {
            FixVector result;
            FixVector.Min(ref value1, ref value2, out result);
            return result;
        }

        public static void Min(ref FixVector value1, ref FixVector value2, out FixVector result)
        {
            result.x = (value1.x < value2.x) ? value1.x : value2.x;
            result.y = (value1.y < value2.y) ? value1.y : value2.y;
            result.z = (value1.z < value2.z) ? value1.z : value2.z;
        }
        public static FixVector Max(FixVector value1, FixVector value2)
        {
            FixVector result;
            FixVector.Max(ref value1, ref value2, out result);
            return result;
        }
        public static void Max(ref FixVector value1, ref FixVector value2, out FixVector result)
        {
            result.x = (value1.x > value2.x) ? value1.x : value2.x;
            result.y = (value1.y > value2.y) ? value1.y : value2.y;
            result.z = (value1.z > value2.z) ? value1.z : value2.z;
        }
        public static FP Distance(FixVector v1, FixVector v2)
        {
            return FP.Sqrt((v1.x - v2.x) * (v1.x - v2.x) + (v1.y - v2.y) * (v1.y - v2.y) + (v1.z - v2.z) * (v1.z - v2.z));
        }
        public static FP Dot(FixVector vector1, FixVector vector2)
        {
            return FixVector.Dot(ref vector1, ref vector2);
        }
        public static FP Dot(ref FixVector vector1, ref FixVector vector2)
        {
            return ((vector1.x * vector2.x) + (vector1.y * vector2.y)) + (vector1.z * vector2.z);
        }
        public static FixVector Add(FixVector value1, FixVector value2)
        {
            FixVector result;
            FixVector.Add(ref value1, ref value2, out result);
            return result;
        }
        public static void Add(ref FixVector value1, ref FixVector value2, out FixVector result)
        {
            result.x = value1.x + value2.x;
            result.y = value1.y + value2.y;
            result.z = value1.z + value2.z;
        }
        public static FixVector Divide(FixVector value1, FP scaleFactor)
        {
            FixVector result;
            FixVector.Divide(ref value1, scaleFactor, out result);
            return result;
        }
        public static void Divide(ref FixVector value1, FP scaleFactor, out FixVector result)
        {
            result.x = value1.x / scaleFactor;
            result.y = value1.y / scaleFactor;
            result.z = value1.z / scaleFactor;
        }
        public static FixVector Subtract(FixVector value1, FixVector value2)
        {
            FixVector result;
            FixVector.Subtract(ref value1, ref value2, out result);
            return result;
        }
        public static void Subtract(ref FixVector value1, ref FixVector value2, out FixVector result)
        {
            result.x = value1.x - value2.x;
            result.y = value1.y - value2.y;
            result.z = value1.z - value2.z;
        }
        public static FixVector Cross(FixVector vector1, FixVector vector2)
        {
            FixVector result;
            FixVector.Cross(ref vector1, ref vector2, out result);
            return result;
        }
        public static void Cross(ref FixVector vector1, ref FixVector vector2, out FixVector result)
        {
            FP num3 = (vector1.y * vector2.z) - (vector1.z * vector2.y);
            FP num2 = (vector1.z * vector2.x) - (vector1.x * vector2.z);
            FP num = (vector1.x * vector2.y) - (vector1.y * vector2.x);
            result.x = num3;
            result.y = num2;
            result.z = num;
        }
        public static FixVector Negate(FixVector value)
        {
            FixVector result;
            FixVector.Negate(ref value, out result);
            return result;
        }
        public static void Negate(ref FixVector value, out FixVector result)
        {
            result.x = -value.x;
            result.y = -value.y;
            result.z = -value.z;
        }
        public static FixVector Normalize(FixVector value)
        {
            FixVector result;
            FixVector.Normalize(ref value, out result);
            return result;
        }
        public static void Normalize(ref FixVector value, out FixVector result)
        {
            FP num2 = ((value.x * value.x) + (value.y * value.y)) + (value.z * value.z);
            FP num = FP.One / FP.Sqrt(num2);
            result.x = value.x * num;
            result.y = value.y * num;
            result.z = value.z * num;
        }
        public static void Swap(ref FixVector vector1, ref FixVector vector2)
        {
            FP temp;

            temp = vector1.x;
            vector1.x = vector2.x;
            vector2.x = temp;

            temp = vector1.y;
            vector1.y = vector2.y;
            vector2.y = temp;

            temp = vector1.z;
            vector1.z = vector2.z;
            vector2.z = temp;
        }
        public static FixVector Multiply(FixVector value1, FP scaleFactor)
        {
            FixVector result;
            FixVector.Multiply(ref value1, scaleFactor, out result);
            return result;
        }
        public static void Multiply(ref FixVector value1, FP scaleFactor, out FixVector result)
        {
            result.x = value1.x * scaleFactor;
            result.y = value1.y * scaleFactor;
            result.z = value1.z * scaleFactor;
        }
        public static FixVector operator %(FixVector value1, FixVector value2)
        {
            FixVector result;
            FixVector.Cross(ref value1, ref value2, out result);
            return result;
        }
        public static FP operator *(FixVector value1, FixVector value2)
        {
            return FixVector.Dot(ref value1, ref value2);
        }
        public static FixVector operator *(FixVector value1, FP value2)
        {
            FixVector result;
            FixVector.Multiply(ref value1, value2, out result);
            return result;
        }
        public static FixVector operator *(FP value1, FixVector value2)
        {
            FixVector result;
            FixVector.Multiply(ref value2, value1, out result);
            return result;
        }
        public static FixVector operator -(FixVector value1, FixVector value2)
        {
            FixVector result;
            FixVector.Subtract(ref value1, ref value2, out result);
            return result;
        }
        public static FixVector operator +(FixVector value1, FixVector value2)
        {
            FixVector result;
            FixVector.Add(ref value1, ref value2, out result);
            return result;
        }
        public static FixVector operator /(FixVector value1, FP value2)
        {
            FixVector result;
            FixVector.Divide(ref value1, value2, out result);
            return result;
        }
        public static FP Angle(FixVector a, FixVector b)
        {
            return FP.Acos(a.normalized * b.normalized) * FP.Rad2Deg;
        }

        #region To Unity
        public static explicit operator UnityEngine.Vector3(FixVector value)
        {
            return new UnityEngine.Vector3((float)value.x, (float)value.y, (float)value.z);
        }
        public static explicit operator FixVector(UnityEngine.Vector3 value)
        {
            return new FixVector((FP)value.x, (FP)value.y, (FP)value.z);
        }
        #endregion
    }
}

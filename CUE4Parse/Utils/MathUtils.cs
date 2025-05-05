using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using CUE4Parse.UE4.Objects.Core.Math;

namespace CUE4Parse.Utils;

public static class MathUtils
{
	public static bool IsNumericType(this object o)
	{
		TypeCode typeCode = Type.GetTypeCode(o.GetType());
		if ((uint)(typeCode - 5) <= 10u)
		{
			return true;
		}
		return false;
	}

	public static float InvSqrt(this float x)
	{
		float num = 0.5f * x;
		int num2 = BitConverter.SingleToInt32Bits(x);
		num2 = 1597463007 - (num2 >> 1);
		x = BitConverter.Int32BitsToSingle(num2);
		x *= 1.5f - num * x * x;
		return x;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int DivideAndRoundUp(this int dividend, int divisor)
	{
		return (dividend + divisor - 1) / divisor;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float ToDegrees(this float radVal)
	{
		return radVal * (180f / (float)Math.PI);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float ToRadians(this float degVal)
	{
		return degVal * ((float)Math.PI / 180f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float Square(this float val)
	{
		return val * val;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int TruncToInt(this float f)
	{
		return (int)f;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int TruncToInt(this double f)
	{
		return (int)f;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int FloorToInt(this float f)
	{
		return Math.Floor(f).TruncToInt();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int RoundToInt(this float f)
	{
		return (f + 0.5f).FloorToInt();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int Clamp(this int i, int min, int max)
	{
		if (i >= min)
		{
			if (i >= max)
			{
				return max;
			}
			return i;
		}
		return min;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float Clamp(this float f, float min, float max)
	{
		if (!(f < min))
		{
			if (!(f < max))
			{
				return max;
			}
			return f;
		}
		return min;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float FloatSelect(float comparand, float valueGEZero, float valueLTZero)
	{
		if (!(comparand >= 0f))
		{
			return valueLTZero;
		}
		return valueGEZero;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FVector Lerp(FVector a, FVector b, float alpha)
	{
		return a + alpha * (b - a);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float Lerp(float a, float b, float alpha)
	{
		return a + alpha * (b - a);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2 ToVector2(this FVector2D v)
	{
		return new Vector2(v.X, v.Y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 ToVector3(this FVector v)
	{
		return new Vector3(v.X, v.Y, v.Z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector4 ToVector4(this FVector v)
	{
		return new Vector4(v.X, v.Y, v.Z, 0f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector4 ToVector4(this FVector4 v)
	{
		return new Vector4(v.X, v.Y, v.Z, v.W);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FVector2D ToFVector2D(this Vector2 v)
	{
		return new FVector2D(v.X, v.Y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FVector ToFVector(this Vector3 v)
	{
		return new FVector(v.X, v.Y, v.Z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FVector4 ToFVector4(this Vector3 v)
	{
		return new FVector4(v.X, v.Y, v.Z, 0f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FVector4 ToFVector4(this Vector4 v)
	{
		return new FVector4(v.X, v.Y, v.Z, v.W);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Quaternion ToQuaternion(this FQuat q)
	{
		return new Quaternion(q.X, q.Y, q.Z, q.W);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FQuat ToFQuat(this Quaternion q)
	{
		return new FQuat(q.X, q.Y, q.Z, q.W);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double Ulpc(double value)
	{
		return BitConverter.Int64BitsToDouble(BitConverter.DoubleToInt64Bits(value) + 1) - value;
	}
}

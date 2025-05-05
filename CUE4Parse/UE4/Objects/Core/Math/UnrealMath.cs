using System;
using System.Runtime.CompilerServices;
using CUE4Parse.Utils;

namespace CUE4Parse.UE4.Objects.Core.Math;

public static class UnrealMath
{
	public const float SmallNumber = 1E-08f;

	public const float KindaSmallNumber = 0.0001f;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float Min3(float a, float b, float c)
	{
		return MathF.Min(a, MathF.Min(b, c));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float Max3(float a, float b, float c)
	{
		return MathF.Max(a, MathF.Max(b, c));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsNearlyEqual(float a, float b, float err = 1E-08f)
	{
		return MathF.Abs(a - b) <= err;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsNearlyZero(float x, float tolerance = 0.0001f)
	{
		return MathF.Abs(x) <= tolerance;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsNearlyZero(double x, double tolerance = 9.999999747378752E-05)
	{
		return System.Math.Abs(x) <= tolerance;
	}

	public static float Fmod(float x, float y)
	{
		float num = MathF.Abs(y);
		if (num <= 1E-08f)
		{
			return 0f;
		}
		float num2 = x / y;
		float num3 = ((MathF.Abs(num2) < 8388608f) ? ((float)num2.TruncToInt()) : num2);
		float num4 = y * num3;
		if (MathF.Abs(num4) > MathF.Abs(x))
		{
			num4 = x;
		}
		return (x - num4).Clamp(0f - num, num);
	}
}

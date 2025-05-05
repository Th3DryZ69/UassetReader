using System;

namespace CUE4Parse.UE4.Objects.Core.Math;

public class FRotationTranslationMatrix : FMatrix
{
	public FRotationTranslationMatrix(FRotator rot, FVector origin)
	{
		float x = rot.Pitch / 180f * (float)System.Math.PI;
		float x2 = rot.Yaw / 180f * (float)System.Math.PI;
		float x3 = rot.Roll / 180f * (float)System.Math.PI;
		float num = MathF.Sin(x);
		float num2 = MathF.Sin(x2);
		float num3 = MathF.Sin(x3);
		float num4 = MathF.Cos(x);
		float num5 = MathF.Cos(x2);
		float num6 = MathF.Cos(x3);
		M00 = num4 * num5;
		M01 = num4 * num2;
		M02 = num;
		M03 = 0f;
		M10 = num3 * num * num5 - num6 * num2;
		M11 = num3 * num * num2 + num6 * num5;
		M12 = (0f - num3) * num4;
		M13 = 0f;
		M20 = 0f - (num6 * num * num5 + num3 * num2);
		M21 = num5 * num3 - num6 * num * num2;
		M22 = num6 * num4;
		M23 = 0f;
		M30 = origin.X;
		M31 = origin.Y;
		M32 = origin.Z;
		M33 = 1f;
	}
}

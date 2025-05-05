namespace CUE4Parse.UE4.Objects.Core.Math;

public class FQuatRotationTranslationMatrix : FMatrix
{
	public FQuatRotationTranslationMatrix(FQuat q, FVector origin)
	{
		float num = q.X + q.X;
		float num2 = q.Y + q.Y;
		float num3 = q.Z + q.Z;
		float num4 = q.X * num;
		float num5 = q.X * num2;
		float num6 = q.X * num3;
		float num7 = q.Y * num2;
		float num8 = q.Y * num3;
		float num9 = q.Z * num3;
		float num10 = q.W * num;
		float num11 = q.W * num2;
		float num12 = q.W * num3;
		M00 = 1f - (num7 + num9);
		M10 = num5 - num12;
		M20 = num6 + num11;
		M30 = origin.X;
		M01 = num5 + num12;
		M11 = 1f - (num4 + num9);
		M21 = num8 - num10;
		M31 = origin.Y;
		M02 = num6 - num11;
		M12 = num8 + num10;
		M22 = 1f - (num4 + num7);
		M32 = origin.Z;
		M03 = 0f;
		M13 = 0f;
		M23 = 0f;
		M33 = 1f;
	}
}

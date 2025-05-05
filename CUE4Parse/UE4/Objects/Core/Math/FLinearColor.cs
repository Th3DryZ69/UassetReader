using System;
using CUE4Parse.Utils;

namespace CUE4Parse.UE4.Objects.Core.Math;

public struct FLinearColor : IUStruct
{
	public float R;

	public float G;

	public float B;

	public float A;

	public string Hex => ToFColor(sRGB: true).Hex;

	public FColor ToFColor(bool sRGB)
	{
		float num = R.Clamp(0f, 1f);
		float num2 = G.Clamp(0f, 1f);
		float num3 = B.Clamp(0f, 1f);
		float num4 = A.Clamp(0f, 1f);
		if (sRGB)
		{
			num = ((num <= 0.0031308f) ? (num * 12.92f) : (MathF.Pow(num, 5f / 12f) * 1.055f - 0.055f));
			num2 = ((num2 <= 0.0031308f) ? (num2 * 12.92f) : (MathF.Pow(num2, 5f / 12f) * 1.055f - 0.055f));
			num3 = ((num3 <= 0.0031308f) ? (num3 * 12.92f) : (MathF.Pow(num3, 5f / 12f) * 1.055f - 0.055f));
		}
		int num5 = (num4 * 255.999f).FloorToInt();
		int num6 = (num * 255.999f).FloorToInt();
		int num7 = (num2 * 255.999f).FloorToInt();
		int num8 = (num3 * 255.999f).FloorToInt();
		return new FColor((byte)num6, (byte)num7, (byte)num8, (byte)num5);
	}

	public FLinearColor ToSRGB()
	{
		float num = R.Clamp(0f, 1f);
		float num2 = G.Clamp(0f, 1f);
		float num3 = B.Clamp(0f, 1f);
		num = ((num <= 0.0031308f) ? (num * 12.92f) : (MathF.Pow(num, 5f / 12f) * 1.055f - 0.055f));
		num2 = ((num2 <= 0.0031308f) ? (num2 * 12.92f) : (MathF.Pow(num2, 5f / 12f) * 1.055f - 0.055f));
		num3 = ((num3 <= 0.0031308f) ? (num3 * 12.92f) : (MathF.Pow(num3, 5f / 12f) * 1.055f - 0.055f));
		return new FLinearColor(num, num2, num3, A);
	}

	public FLinearColor(float r, float g, float b, float a)
	{
		R = r;
		G = g;
		B = b;
		A = a;
	}

	public override string ToString()
	{
		return Hex;
	}

	public FLinearColor LinearRGBToHsv()
	{
		float num = UnrealMath.Min3(R, G, B);
		float num2 = UnrealMath.Max3(R, G, B);
		float num3 = num2 - num;
		float r = ((num2 == num) ? 0f : ((num2 == R) ? UnrealMath.Fmod((G - B) / num3 * 60f + 360f, 360f) : ((num2 == G) ? ((B - R) / num3 * 60f + 120f) : ((num2 == B) ? ((R - G) / num3 * 60f + 240f) : 0f))));
		float g = ((num2 == 0f) ? 0f : (num3 / num2));
		return new FLinearColor(r, g, num2, A);
	}

	public FLinearColor HSVToLinearRGB()
	{
		float r = R;
		float g = G;
		float b = B;
		float num = r / 60f;
		float num2 = MathF.Floor(num);
		float num3 = num - num2;
		float[] array = new float[4]
		{
			b,
			b * (1f - g),
			b * (1f - num3 * g),
			b * (1f - (1f - num3) * g)
		};
		uint[][] array2 = new uint[6][]
		{
			new uint[3] { 0u, 3u, 1u },
			new uint[3] { 2u, 0u, 1u },
			new uint[3] { 1u, 0u, 3u },
			new uint[3] { 1u, 2u, 0u },
			new uint[3] { 3u, 1u, 0u },
			new uint[3] { 0u, 1u, 2u }
		};
		uint num4 = (uint)num2 % 6;
		return new FLinearColor(array[array2[num4][0]], array[array2[num4][1]], array[array2[num4][2]], A);
	}
}

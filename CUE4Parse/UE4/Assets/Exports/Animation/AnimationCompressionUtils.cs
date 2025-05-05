using System;
using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Assets.Exports.Animation;

public static class AnimationCompressionUtils
{
	public const float Quant16BitDiv = 32767f;

	public const int Quant16BitOffs = 32767;

	public const float Quant10BitDiv = 511f;

	public const int Quant10BitOffs = 511;

	public const float Quant11BitDiv = 1023f;

	public const int Quant11BitOffs = 1023;

	public static FQuat ReadQuatFixed48NoW(this FArchive Ar, int componentMask = 7)
	{
		int num = (((componentMask & 1) != 0) ? Ar.Read<ushort>() : 32767);
		int num2 = (((componentMask & 2) != 0) ? Ar.Read<ushort>() : 32767);
		int num3 = (((componentMask & 4) != 0) ? Ar.Read<ushort>() : 32767);
		float num4 = (float)(num - 32767) / 32767f;
		float num5 = (float)(num2 - 32767) / 32767f;
		float num6 = (float)(num3 - 32767) / 32767f;
		float num7 = 1f - num4 * num4 - num5 * num5 - num6 * num6;
		return new FQuat(num4, num5, num6, (num7 > 0f) ? MathF.Sqrt(num7) : 0f);
	}

	public static FQuat ReadQuatFixed32NoW(this FArchive Ar)
	{
		uint num = Ar.Read<uint>();
		uint num2 = num >> 21;
		uint num3 = (num & 0x1FFC00) >> 10;
		uint num4 = num & 0x3FF;
		float num5 = (float)(int)(num2 - 1023) / 1023f;
		float num6 = (float)(int)(num3 - 1023) / 1023f;
		float num7 = (float)(int)(num4 - 511) / 511f;
		float num8 = 1f - num5 * num5 - num6 * num6 - num7 * num7;
		return new FQuat(num5, num6, num7, (num8 > 0f) ? MathF.Sqrt(num8) : 0f);
	}

	public static FQuat ReadQuatFloat96NoW(this FArchive Ar)
	{
		float num = Ar.Read<float>();
		float num2 = Ar.Read<float>();
		float num3 = Ar.Read<float>();
		float num4 = 1f - num * num - num2 * num2 - num3 * num3;
		return new FQuat(num, num2, num3, (num4 > 0f) ? MathF.Sqrt(num4) : 0f);
	}

	public static FVector ReadVectorFixed48(this FArchive Ar)
	{
		ushort num = Ar.Read<ushort>();
		ushort num2 = Ar.Read<ushort>();
		ushort num3 = Ar.Read<ushort>();
		float num4 = (float)(num - 32767) / 32767f;
		float num5 = (float)(num2 - 32767) / 32767f;
		float num6 = (float)(num3 - 32767) / 32767f;
		return new FVector(num4 * 128f, num5 * 128f, num6 * 128f);
	}

	public static FVector ReadVectorIntervalFixed32(this FArchive Ar, FVector mins, FVector ranges)
	{
		uint num = Ar.Read<uint>();
		uint num2 = num >> 21;
		uint num3 = (num & 0x1FFC00) >> 10;
		float x = (float)(int)((num & 0x3FF) - 511) / 511f * ranges.X + mins.X;
		float y = (float)(int)(num3 - 1023) / 1023f * ranges.Y + mins.Y;
		float z = (float)(int)(num2 - 1023) / 1023f * ranges.Z + mins.Z;
		return new FVector(x, y, z);
	}

	public static FQuat ReadQuatIntervalFixed32NoW(this FArchive Ar, FVector mins, FVector ranges)
	{
		uint num = Ar.Read<uint>();
		uint num2 = num >> 21;
		uint num3 = (num & 0x1FFC00) >> 10;
		uint num4 = num & 0x3FF;
		float num5 = (float)(int)(num2 - 1023) / 1023f * ranges.X + mins.X;
		float num6 = (float)(int)(num3 - 1023) / 1023f * ranges.Y + mins.Y;
		float num7 = (float)(int)(num4 - 511) / 511f * ranges.Z + mins.Z;
		float num8 = 1f - num5 * num5 - num6 * num6 - num7 * num7;
		return new FQuat(num5, num6, num7, (num8 > 0f) ? MathF.Sqrt(num8) : 0f);
	}

	public static FQuat ReadQuatFloat32NoW(this FArchive Ar)
	{
		uint num = Ar.Read<uint>();
		uint num2 = num >> 21;
		uint num3 = (num & 0x1FFC00) >> 10;
		uint num4 = num & 0x3FF;
		float num5 = BitConverter.Int32BitsToSingle((int)((((num2 >> 7) & 7) + 123 << 23) | (((num2 & 0x7F) | (32 * (num2 & 0xFFFFFC00u))) << 16)));
		float num6 = BitConverter.Int32BitsToSingle((int)((((num3 >> 7) & 7) + 123 << 23) | (((num3 & 0x7F) | (32 * (num3 & 0xFFFFFC00u))) << 16)));
		float num7 = BitConverter.Int32BitsToSingle((int)((((num4 >> 6) & 7) + 123 << 23) | (((num4 & 0x3F) | (32 * (num4 & 0xFFFFFE00u))) << 17)));
		float num8 = 1f - num5 * num5 - num6 * num6 - num7 * num7;
		return new FQuat(num5, num6, num7, (num8 > 0f) ? MathF.Sqrt(num8) : 0f);
	}

	public static float DecodeFixed48_PerTrackComponent(ushort value, int log2)
	{
		int num = (1 << 15 - log2) - 1;
		float num2 = 1f / (float)(num >> log2);
		return (float)(value - num) * num2;
	}
}

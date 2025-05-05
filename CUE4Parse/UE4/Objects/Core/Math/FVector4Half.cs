using CUE4Parse.Utils;

namespace CUE4Parse.UE4.Objects.Core.Math;

public struct FVector4Half : IUStruct
{
	public readonly ushort X;

	public readonly ushort Y;

	public readonly ushort Z;

	public readonly ushort W;

	public FVector4Half(ushort x, ushort y, ushort z, ushort w)
	{
		X = x;
		Y = y;
		Z = z;
		W = w;
	}

	public static implicit operator FVector(FVector4Half v)
	{
		return new FVector(TypeConversionUtils.HalfToFloat(v.X), TypeConversionUtils.HalfToFloat(v.Y), TypeConversionUtils.HalfToFloat(v.Z));
	}

	public static implicit operator FVector4(FVector4Half v)
	{
		return new FVector4(TypeConversionUtils.HalfToFloat(v.X), TypeConversionUtils.HalfToFloat(v.Y), TypeConversionUtils.HalfToFloat(v.Z), TypeConversionUtils.HalfToFloat(v.W));
	}
}

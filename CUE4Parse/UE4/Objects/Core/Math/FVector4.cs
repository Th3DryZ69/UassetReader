using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Objects.Core.Math;

public struct FVector4 : IUStruct
{
	public readonly float X;

	public readonly float Y;

	public readonly float Z;

	public readonly float W;

	public static readonly FVector4 ZeroVector = new FVector4(0f, 0f, 0f, 0f);

	public FVector4(float x, float y, float z, float w)
	{
		X = x;
		Y = y;
		Z = z;
		W = w;
	}

	public FVector4(float x)
		: this(x, x, x, x)
	{
	}

	public FVector4(FArchive Ar)
	{
		if (Ar.Ver >= EUnrealEngineObjectUE5Version.LARGE_WORLD_COORDINATES)
		{
			X = (float)Ar.Read<double>();
			Y = (float)Ar.Read<double>();
			Z = (float)Ar.Read<double>();
			W = (float)Ar.Read<double>();
		}
		else
		{
			X = Ar.Read<float>();
			Y = Ar.Read<float>();
			Z = Ar.Read<float>();
			W = Ar.Read<float>();
		}
	}

	public FVector4(FVector v, float w = 1f)
		: this(v.X, v.Y, v.Z, w)
	{
	}

	public FVector4(FLinearColor color)
		: this(color.R, color.G, color.B, color.A)
	{
	}

	public static explicit operator FVector(FVector4 v)
	{
		return new FVector(v.X, v.Y, v.Z);
	}

	public override string ToString()
	{
		return $"X={X,3:F3} Y={Y,3:F3} Z={Z,3:F3} W={W,3:F3}";
	}
}

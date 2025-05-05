using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Objects.Core.Math;

public struct FVector2D : IUStruct
{
	public static readonly FVector2D ZeroVector = new FVector2D(0f, 0f);

	public readonly float X;

	public readonly float Y;

	public FVector2D(float x, float y)
	{
		X = x;
		Y = y;
	}

	public FVector2D(FArchive Ar)
	{
		if (Ar.Ver >= EUnrealEngineObjectUE5Version.LARGE_WORLD_COORDINATES)
		{
			X = (float)Ar.Read<double>();
			Y = (float)Ar.Read<double>();
		}
		else
		{
			X = Ar.Read<float>();
			Y = Ar.Read<float>();
		}
	}

	public override string ToString()
	{
		return $"X={X,3:F3} Y={Y,3:F3}";
	}
}

using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Objects.Core.Math;

public class FBox2D : IUStruct
{
	public readonly FVector2D Min;

	public readonly FVector2D Max;

	public readonly byte bIsValid;

	public FBox2D()
	{
	}

	public FBox2D(FArchive Ar)
	{
		Min = new FVector2D(Ar);
		Max = new FVector2D(Ar);
		bIsValid = Ar.Read<byte>();
	}

	public override string ToString()
	{
		return $"bIsValid={bIsValid}, Min=({Min}), Max=({Max})";
	}
}

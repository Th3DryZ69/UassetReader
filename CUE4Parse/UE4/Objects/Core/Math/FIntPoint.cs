namespace CUE4Parse.UE4.Objects.Core.Math;

public readonly struct FIntPoint : IUStruct
{
	public readonly uint X;

	public readonly uint Y;

	public override string ToString()
	{
		return $"{"X"}: {X}, {"Y"}: {Y}";
	}
}

namespace CUE4Parse.UE4.Objects.Core.Math;

public readonly struct TIntVector2<T> : IUStruct
{
	public readonly T X;

	public readonly T Y;

	public override string ToString()
	{
		return $"{"X"}: {X}, {"Y"}: {Y}";
	}
}

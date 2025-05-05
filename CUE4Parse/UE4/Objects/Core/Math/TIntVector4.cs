namespace CUE4Parse.UE4.Objects.Core.Math;

public readonly struct TIntVector4<T> : IUStruct
{
	public readonly T X;

	public readonly T Y;

	public readonly T Z;

	public readonly T W;

	public override string ToString()
	{
		return $"{"X"}: {X}, {"Y"}: {Y}, {"Z"}: {Z}, {"W"}: {W}";
	}
}

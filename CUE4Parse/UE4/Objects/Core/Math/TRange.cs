namespace CUE4Parse.UE4.Objects.Core.Math;

public readonly struct TRange<T> : IUStruct
{
	public readonly TRangeBound<T> LowerBound;

	public readonly TRangeBound<T> UpperBound;

	public override string ToString()
	{
		return $"{"LowerBound"}: {LowerBound}, {"UpperBound"}: {UpperBound}";
	}
}

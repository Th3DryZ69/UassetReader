namespace CUE4Parse.UE4.Objects.Core.Misc;

public readonly struct FFrameRate : IUStruct
{
	public readonly int Numerator;

	public readonly int Denominator;

	public override string ToString()
	{
		return $"{"Numerator"}: {Numerator}, {"Denominator"}: {Denominator}";
	}
}

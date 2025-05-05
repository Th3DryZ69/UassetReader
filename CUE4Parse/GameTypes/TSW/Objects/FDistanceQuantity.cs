using CUE4Parse.UE4;

namespace CUE4Parse.GameTypes.TSW.Objects;

public readonly struct FDistanceQuantity : IUStruct
{
	public readonly float Value;

	public FDistanceQuantity(float InValue)
	{
		Value = InValue;
	}
}

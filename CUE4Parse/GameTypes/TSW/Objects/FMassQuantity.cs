using CUE4Parse.UE4;

namespace CUE4Parse.GameTypes.TSW.Objects;

public readonly struct FMassQuantity : IUStruct
{
	public readonly float Value;

	public FMassQuantity(float InValue)
	{
		Value = InValue;
	}
}

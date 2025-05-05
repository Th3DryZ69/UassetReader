using CUE4Parse.UE4;

namespace CUE4Parse.GameTypes.TSW.Objects;

public readonly struct FSpeedQuantity : IUStruct
{
	public readonly float Value;

	public FSpeedQuantity(float InValue)
	{
		Value = InValue;
	}
}

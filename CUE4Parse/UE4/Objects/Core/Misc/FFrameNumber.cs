namespace CUE4Parse.UE4.Objects.Core.Misc;

public readonly struct FFrameNumber : IUStruct
{
	public readonly int Value;

	public override string ToString()
	{
		return Value.ToString();
	}
}

namespace CUE4Parse.UE4.Objects.Engine.Curves;

public enum ERichCurveExtrapolation : byte
{
	RCCE_Cycle,
	RCCE_CycleWithOffset,
	RCCE_Oscillate,
	RCCE_Linear,
	RCCE_Constant,
	RCCE_None
}

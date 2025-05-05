using CUE4Parse.UE4.Assets.Objects;

namespace CUE4Parse.UE4.Assets.Exports.Animation;

public struct FRawCurveTracks
{
	public FFloatCurve[]? FloatCurves;

	public FRawCurveTracks(FStructFallback data)
	{
		FloatCurves = data.GetOrDefault<FFloatCurve[]>("FloatCurves");
	}
}

using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Objects.Engine.Curves;

namespace CUE4Parse.UE4.Assets.Exports.Animation;

public class FFloatCurve : FAnimCurveBase
{
	public FRichCurve FloatCurve;

	public FFloatCurve()
	{
	}

	public FFloatCurve(FStructFallback data)
		: base(data)
	{
		FloatCurve = data.GetOrDefault<FRichCurve>("FloatCurve");
	}
}

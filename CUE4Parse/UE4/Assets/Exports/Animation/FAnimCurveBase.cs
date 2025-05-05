using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Objects.Engine.Animation;

namespace CUE4Parse.UE4.Assets.Exports.Animation;

public class FAnimCurveBase
{
	public FSmartName Name;

	public int CurveTypeFlags;

	public FAnimCurveBase()
	{
	}

	public FAnimCurveBase(FStructFallback data)
	{
		Name = data.GetOrDefault<FSmartName>("Name");
		CurveTypeFlags = data.GetOrDefault("CurveTypeFlags", 0);
	}
}

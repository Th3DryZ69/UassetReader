using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Assets.Utils;
using CUE4Parse.UE4.Objects.Core.Math;

namespace CUE4Parse.UE4.Objects.Engine.Animation;

[StructFallback]
public class FPoseData
{
	public FTransform[] LocalSpacePose;

	public float[] CurveData;

	public FPoseData(FStructFallback fallback)
	{
		LocalSpacePose = fallback.GetOrDefault<FTransform[]>("LocalSpacePose");
		CurveData = fallback.GetOrDefault<float[]>("CurveData");
	}
}

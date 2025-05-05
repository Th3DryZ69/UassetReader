using CUE4Parse.UE4.Assets.Exports.Animation;
using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Assets.Utils;
using CUE4Parse.UE4.Objects.UObject;

namespace CUE4Parse.UE4.Objects.Engine.Animation;

[StructFallback]
public class FPoseDataContainer
{
	public FSmartName[] PoseNames;

	public FName[] Tracks;

	public FPoseAssetInfluences[] TrackPoseInfluenceIndices;

	public FPoseData[] Poses;

	public FAnimCurveBase[] Curves;

	public FPoseDataContainer(FStructFallback fallback)
	{
		PoseNames = fallback.GetOrDefault<FSmartName[]>("PoseNames");
		Tracks = fallback.GetOrDefault<FName[]>("Tracks");
		TrackPoseInfluenceIndices = fallback.GetOrDefault<FPoseAssetInfluences[]>("TrackPoseInfluenceIndices");
		Poses = fallback.GetOrDefault<FPoseData[]>("Poses");
		Curves = fallback.GetOrDefault<FAnimCurveBase[]>("Curves");
	}
}

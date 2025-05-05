using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Objects.UObject;

namespace CUE4Parse.UE4.Objects.Engine.Animation;

public class UPoseAsset : UAnimationAsset
{
	public FPoseDataContainer PoseContainer;

	public bool bAdditivePose;

	public int BasePoseIndex;

	public FName RetargetSource;

	public FTransform[] RetargetSourceAssetReferencePose;

	public override void Deserialize(FAssetArchive Ar, long validPos)
	{
		base.Deserialize(Ar, validPos);
		PoseContainer = GetOrDefault<FPoseDataContainer>("PoseContainer");
		bAdditivePose = GetOrDefault("bAdditivePose", defaultValue: false);
		BasePoseIndex = GetOrDefault("BasePoseIndex", 0);
		RetargetSource = GetOrDefault<FName>("RetargetSource");
		RetargetSourceAssetReferencePose = GetOrDefault<FTransform[]>("RetargetSourceAssetReferencePose");
	}
}

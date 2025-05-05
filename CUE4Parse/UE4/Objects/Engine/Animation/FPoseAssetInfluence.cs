using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Assets.Utils;

namespace CUE4Parse.UE4.Objects.Engine.Animation;

[StructFallback]
public class FPoseAssetInfluence
{
	public int BoneTransformIndex;

	public int PoseIndex;

	public FPoseAssetInfluence(FStructFallback fallback)
	{
		BoneTransformIndex = fallback.GetOrDefault("BoneTransformIndex", 0);
		PoseIndex = fallback.GetOrDefault("PoseIndex", 0);
	}
}

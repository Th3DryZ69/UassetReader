using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Assets.Utils;

namespace CUE4Parse.UE4.Objects.Engine.Animation;

[StructFallback]
public class FPoseAssetInfluences
{
	public FPoseAssetInfluence[] Influences;

	public FPoseAssetInfluences(FStructFallback fallback)
	{
		Influences = fallback.GetOrDefault<FPoseAssetInfluence[]>("Influences");
	}
}

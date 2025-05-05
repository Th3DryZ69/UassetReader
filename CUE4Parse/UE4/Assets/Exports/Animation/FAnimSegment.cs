using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Assets.Utils;
using CUE4Parse.UE4.Objects.UObject;

namespace CUE4Parse.UE4.Assets.Exports.Animation;

[StructFallback]
public class FAnimSegment
{
	public FPackageIndex AnimReference;

	public float StartPos;

	public float AnimStartTime;

	public float AnimEndTime;

	public float AnimPlayRate;

	public int LoopingCount;

	public FAnimSegment(FStructFallback fallback)
	{
		AnimReference = fallback.GetOrDefault<FPackageIndex>("AnimReference");
		StartPos = fallback.GetOrDefault("StartPos", 0f);
		AnimStartTime = fallback.GetOrDefault("AnimStartTime", 0f);
		AnimEndTime = fallback.GetOrDefault("AnimEndTime", 0f);
		AnimPlayRate = fallback.GetOrDefault("AnimPlayRate", 0f);
		LoopingCount = fallback.GetOrDefault("LoopingCount", 0);
	}
}

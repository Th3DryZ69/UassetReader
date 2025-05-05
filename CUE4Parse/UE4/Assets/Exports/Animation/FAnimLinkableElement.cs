using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Assets.Utils;
using CUE4Parse.UE4.Objects.UObject;

namespace CUE4Parse.UE4.Assets.Exports.Animation;

[StructFallback]
public class FAnimLinkableElement
{
	public FPackageIndex LinkedMontage;

	public int SlotIndex;

	public int SegmentIndex;

	public EAnimLinkMethod LinkMethod;

	public EAnimLinkMethod CachedLinkMethod;

	public float SegmentBeginTime;

	public float SegmentLength;

	public float LinkValue;

	public FPackageIndex LinkedSequence;

	public FAnimLinkableElement(FStructFallback fallback)
	{
		LinkedMontage = fallback.GetOrDefault<FPackageIndex>("LinkedMontage");
		SlotIndex = fallback.GetOrDefault("SlotIndex", 0);
		SegmentIndex = fallback.GetOrDefault("SegmentIndex", 0);
		LinkMethod = fallback.GetOrDefault("LinkMethod", EAnimLinkMethod.Absolute);
		CachedLinkMethod = fallback.GetOrDefault("CachedLinkMethod", EAnimLinkMethod.Absolute);
		SegmentBeginTime = fallback.GetOrDefault("SegmentBeginTime", 0f);
		SegmentLength = fallback.GetOrDefault("SegmentLength", 0f);
		LinkValue = fallback.GetOrDefault("LinkValue", 0f);
		LinkedSequence = fallback.GetOrDefault<FPackageIndex>("LinkedSequence");
	}
}

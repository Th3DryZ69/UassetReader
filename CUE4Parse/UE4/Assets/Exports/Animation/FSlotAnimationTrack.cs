using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Assets.Utils;
using CUE4Parse.UE4.Objects.UObject;

namespace CUE4Parse.UE4.Assets.Exports.Animation;

[StructFallback]
public class FSlotAnimationTrack
{
	public FName SlotName;

	public FAnimTrack AnimTrack;

	public FSlotAnimationTrack(FStructFallback fallback)
	{
		SlotName = fallback.GetOrDefault<FName>("SlotName");
		AnimTrack = fallback.GetOrDefault<FAnimTrack>("AnimTrack");
	}
}

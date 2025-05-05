using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Assets.Utils;

namespace CUE4Parse.UE4.Assets.Exports.Animation;

[StructFallback]
public class FAnimTrack
{
	public FAnimSegment[] AnimSegments;

	public FAnimTrack(FStructFallback fallback)
	{
		AnimSegments = fallback.GetOrDefault<FAnimSegment[]>("AnimSegments");
	}
}

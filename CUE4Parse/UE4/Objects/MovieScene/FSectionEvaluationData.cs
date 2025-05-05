using CUE4Parse.UE4.Objects.Core.Misc;

namespace CUE4Parse.UE4.Objects.MovieScene;

public readonly struct FSectionEvaluationData : IUStruct
{
	public readonly int ImplIndex;

	public readonly FFrameNumber ForcedTime;

	public readonly ESectionEvaluationFlags Flags;
}

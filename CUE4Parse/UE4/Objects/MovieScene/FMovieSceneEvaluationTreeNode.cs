using System.Runtime.InteropServices;
using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Objects.Core.Misc;

namespace CUE4Parse.UE4.Objects.MovieScene;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct FMovieSceneEvaluationTreeNode : IUStruct
{
	public readonly TRange<FFrameNumber> Range;

	public readonly FMovieSceneEvaluationTreeNodeHandle Parent;

	public readonly FEvaluationTreeEntryHandle ChildrenID;

	public readonly FEvaluationTreeEntryHandle DataID;
}

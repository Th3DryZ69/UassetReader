using CUE4Parse.UE4.Objects.Engine.Curves;

namespace CUE4Parse.UE4.Objects.MovieScene;

public readonly struct FMovieSceneTangentData : IUStruct
{
	public readonly float ArriveTangent;

	public readonly float LeaveTangent;

	public readonly float ArriveTangentWeight;

	public readonly float LeaveTangentWeight;

	public readonly ERichCurveTangentWeightMode TangentWeightMode;
}

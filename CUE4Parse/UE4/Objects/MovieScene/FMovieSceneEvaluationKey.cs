namespace CUE4Parse.UE4.Objects.MovieScene;

public readonly struct FMovieSceneEvaluationKey : IUStruct
{
	public readonly FMovieSceneSequenceID SequenceID;

	public readonly FMovieSceneTrackIdentifier TrackIdentifier;

	public readonly uint SectionIndex;
}

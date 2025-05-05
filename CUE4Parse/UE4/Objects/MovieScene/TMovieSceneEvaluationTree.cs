using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Objects.MovieScene;

public class TMovieSceneEvaluationTree<T> : FMovieSceneEvaluationTree where T : struct
{
	public readonly TEvaluationTreeEntryContainer<T> Data;

	public TMovieSceneEvaluationTree(FArchive Ar)
		: base(Ar)
	{
		Data = new TEvaluationTreeEntryContainer<T>(Ar);
	}
}

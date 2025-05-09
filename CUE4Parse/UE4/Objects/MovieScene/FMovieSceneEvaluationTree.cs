using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Objects.MovieScene;

public class FMovieSceneEvaluationTree : IUStruct
{
	public readonly FMovieSceneEvaluationTreeNode RootNode;

	public readonly TEvaluationTreeEntryContainer<FMovieSceneEvaluationTreeNode> ChildNodes;

	public FMovieSceneEvaluationTree(FArchive Ar)
	{
		RootNode = Ar.Read<FMovieSceneEvaluationTreeNode>();
		ChildNodes = new TEvaluationTreeEntryContainer<FMovieSceneEvaluationTreeNode>(Ar);
	}
}

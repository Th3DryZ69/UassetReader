using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.Utils;

namespace CUE4Parse.UE4.Objects.MovieScene.Evaluation;

public class FMovieSceneEvalTemplatePtr : IUStruct
{
	public string TypeName;

	public FStructFallback? Data;

	public FMovieSceneEvalTemplatePtr(FAssetArchive Ar)
	{
		TypeName = Ar.ReadFString();
		if (TypeName.Length > 0)
		{
			Data = new FStructFallback(Ar, TypeName.SubstringAfterLast('.'));
		}
	}
}

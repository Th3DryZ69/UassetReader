using System;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Objects.MovieScene.Evaluation;

public class FMovieSceneEvaluationFieldEntityTree : IUStruct
{
	public struct FEntityAndMetaDataIndex
	{
		public int EntityIndex;

		public int MetaDataIndex;

		public bool Equals(FEntityAndMetaDataIndex other)
		{
			if (EntityIndex == other.EntityIndex)
			{
				return MetaDataIndex == other.MetaDataIndex;
			}
			return false;
		}

		public override bool Equals(object? obj)
		{
			if (obj is FEntityAndMetaDataIndex other)
			{
				return Equals(other);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(EntityIndex, MetaDataIndex);
		}
	}

	public TMovieSceneEvaluationTree<FEntityAndMetaDataIndex> SerializedData;

	public FMovieSceneEvaluationFieldEntityTree(FArchive Ar)
	{
		SerializedData = new TMovieSceneEvaluationTree<FEntityAndMetaDataIndex>(Ar);
	}
}

using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Objects.MovieScene;

public readonly struct TEvaluationTreeEntryContainer<T> : IUStruct where T : struct
{
	public readonly FEntry[] Entries;

	public readonly T[] Items;

	public TEvaluationTreeEntryContainer(FArchive Ar)
	{
		Entries = Ar.ReadArray<FEntry>();
		Items = Ar.ReadArray<T>();
	}
}

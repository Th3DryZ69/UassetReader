using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Objects.LevelSequence;

public readonly struct FLevelSequenceLegacyObjectReference : IUStruct
{
	public readonly FGuid ObjectId;

	public readonly string ObjectPath;

	public FLevelSequenceLegacyObjectReference(FArchive Ar)
	{
		ObjectId = Ar.Read<FGuid>();
		ObjectPath = Ar.ReadFString();
	}
}

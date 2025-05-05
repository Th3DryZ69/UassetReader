using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Objects.Core.Serialization;

public struct FGuidCustomVersion_DEPRECATED
{
	public FGuid Tag;

	public int Version;

	public string FriendlyName;

	public FGuidCustomVersion_DEPRECATED(FArchive Ar)
	{
		Tag = Ar.Read<FGuid>();
		Version = Ar.Read<int>();
		FriendlyName = Ar.ReadFString();
	}

	public FCustomVersion ToCustomVersion()
	{
		return new FCustomVersion(Tag, Version);
	}
}

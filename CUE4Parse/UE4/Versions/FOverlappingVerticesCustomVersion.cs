using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Versions;

public static class FOverlappingVerticesCustomVersion
{
	public enum Type
	{
		BeforeCustomVersionWasAdded = 0,
		DetectOVerlappingVertices = 1,
		VersionPlusOne = 2,
		LatestVersion = 1
	}

	public static readonly FGuid GUID = new FGuid(1630518866u, 3662888971u, 2433568657u, 2679211388u);

	public static Type Get(FArchive Ar)
	{
		int num = Ar.CustomVer(GUID);
		if (num >= 0)
		{
			return (Type)num;
		}
		if (Ar.Game < EGame.GAME_UE4_19)
		{
			return Type.BeforeCustomVersionWasAdded;
		}
		return Type.DetectOVerlappingVertices;
	}
}

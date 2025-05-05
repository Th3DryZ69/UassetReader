using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Versions;

public static class FReflectionCaptureObjectVersion
{
	public enum Type
	{
		BeforeCustomVersionWasAdded = 0,
		MoveReflectionCaptureDataToMapBuildData = 1,
		VersionPlusOne = 2,
		LatestVersion = 1
	}

	public static readonly FGuid GUID = new FGuid(1797680364u, 516377487u, 2735465689u, 155384839u);

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
		return Type.MoveReflectionCaptureDataToMapBuildData;
	}
}

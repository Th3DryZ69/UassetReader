using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Objects.WorldCondition;

public static class FWorldConditionCustomVersion
{
	public enum Type
	{
		BeforeCustomVersionWasAdded = 0,
		StructSharedDefinition = 1,
		VersionPlusOne = 2,
		LatestVersion = 1
	}

	public static readonly FGuid GUID = new FGuid(740863010u, 365905662u, 3172593681u, 1697266693u);

	public static Type Get(FArchive Ar)
	{
		int num = Ar.CustomVer(GUID);
		if (num >= 0)
		{
			return (Type)num;
		}
		if (Ar.Game < EGame.GAME_UE5_1)
		{
			return Type.BeforeCustomVersionWasAdded;
		}
		return Type.StructSharedDefinition;
	}
}

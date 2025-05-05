using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Assets.Exports.Rig;

public class FDNAAssetCustomVersion
{
	public enum Type
	{
		BeforeCustomVersionWasAdded = 0,
		VersionPlusOne = 1,
		LatestVersion = 0
	}

	public static readonly FGuid GUID = new FGuid(2649210264u, 1741964722u, 2349768051u, 4259439463u);

	public static Type Get(FArchive Ar)
	{
		int num = Ar.CustomVer(GUID);
		if (num >= 0)
		{
			return (Type)num;
		}
		if (Ar.Game < EGame.GAME_UE4_26)
		{
			return Type.BeforeCustomVersionWasAdded;
		}
		return Type.BeforeCustomVersionWasAdded;
	}
}

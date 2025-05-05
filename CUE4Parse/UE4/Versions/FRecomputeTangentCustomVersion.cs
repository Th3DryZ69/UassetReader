using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Versions;

public static class FRecomputeTangentCustomVersion
{
	public enum Type
	{
		BeforeCustomVersionWasAdded = 0,
		RuntimeRecomputeTangent = 1,
		RecomputeTangentVertexColorMask = 2,
		VersionPlusOne = 3,
		LatestVersion = 2
	}

	public static readonly FGuid GUID = new FGuid(1434056838u, 2470071327u, 2210007163u, 1667348783u);

	public static Type Get(FArchive Ar)
	{
		int num = Ar.CustomVer(GUID);
		if (num >= 0)
		{
			return (Type)num;
		}
		EGame game = Ar.Game;
		if (game >= EGame.GAME_UE4_12)
		{
			if (game < EGame.GAME_UE4_26)
			{
				return Type.RuntimeRecomputeTangent;
			}
			return Type.RecomputeTangentVertexColorMask;
		}
		return Type.BeforeCustomVersionWasAdded;
	}
}

using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Versions;

public static class FFortniteReleaseBranchCustomObjectVersion
{
	public enum Type
	{
		BeforeCustomVersionWasAdded = 0,
		DisableLevelset_v14_10 = 1,
		ChaosClothAddTethersToCachedData = 2,
		ChaosKinematicTargetRemoveScale = 3,
		ActorComponentUCSModifiedPropertiesSparseStorage = 4,
		FixupNaniteLandscapeMeshes = 5,
		RemoveUselessLandscapeMeshesCookedCollisionData = 6,
		VersionPlusOne = 7,
		LatestVersion = 6
	}

	public static readonly FGuid GUID = new FGuid(3876086632u, 1797475416u, 2218335088u, 371613329u);

	public static Type Get(FArchive Ar)
	{
		int num = Ar.CustomVer(GUID);
		if (num >= 0)
		{
			return (Type)num;
		}
		EGame game = Ar.Game;
		if (game < EGame.GAME_UE5_1)
		{
			if (game >= EGame.GAME_UE4_25)
			{
				if (game < EGame.GAME_UE5_0)
				{
					return Type.DisableLevelset_v14_10;
				}
				return Type.ChaosKinematicTargetRemoveScale;
			}
			return Type.BeforeCustomVersionWasAdded;
		}
		if (game < EGame.GAME_UE5_2)
		{
			return Type.ActorComponentUCSModifiedPropertiesSparseStorage;
		}
		return Type.RemoveUselessLandscapeMeshesCookedCollisionData;
	}
}

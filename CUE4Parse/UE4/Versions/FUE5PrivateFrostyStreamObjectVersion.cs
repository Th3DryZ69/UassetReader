using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Versions;

public static class FUE5PrivateFrostyStreamObjectVersion
{
	public enum Type
	{
		BeforeCustomVersionWasAdded = 0,
		HLODBatchingPolicy = 1,
		SerializeSceneComponentStaticBounds = 2,
		ChaosClothAddTethersToCachedData = 3,
		SerializeActorLabelInCookedBuilds = 4,
		ConvertWorldPartitionHLODsCellsToName = 5,
		ChaosClothRemoveKinematicTethers = 6,
		SerializeSkeletalMeshMorphTargetRenderData = 7,
		StripMorphTargetSourceDataForCookedBuilds = 8,
		VersionPlusOne = 9,
		LatestVersion = 8
	}

	public static readonly FGuid GUID = new FGuid(1507482962u, 305285448u, 3094894968u, 1891166603u);

	public static Type Get(FArchive Ar)
	{
		int num = Ar.CustomVer(GUID);
		if (num >= 0)
		{
			return (Type)num;
		}
		if (Ar.Game < EGame.GAME_UE5_0)
		{
			return Type.BeforeCustomVersionWasAdded;
		}
		return Type.StripMorphTargetSourceDataForCookedBuilds;
	}
}

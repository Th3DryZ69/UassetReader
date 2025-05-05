using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Versions;

public static class FAnimObjectVersion
{
	public enum Type
	{
		BeforeCustomVersionWasAdded = 0,
		LinkTimeAnimBlueprintRootDiscovery = 1,
		StoreMarkerNamesOnSkeleton = 2,
		SerializeRigVMRegisterArrayState = 3,
		IncreaseBoneIndexLimitPerChunk = 4,
		UnlimitedBoneInfluences = 5,
		AnimSequenceCurveColors = 6,
		NotifyAndSyncMarkerGuids = 7,
		SerializeRigVMRegisterDynamicState = 8,
		SerializeGroomCards = 9,
		SerializeRigVMEntries = 10,
		SerializeHairBindingAsset = 11,
		SerializeHairClusterCullingData = 12,
		SerializeGroomCardsAndMeshes = 13,
		GroomLODStripping = 14,
		GroomBindingSerialization = 15,
		VersionPlusOne = 16,
		LatestVersion = 15
	}

	public static readonly FGuid GUID = new FGuid(2940446301u, 2144553287u, 2557689486u, 3653352197u);

	public static Type Get(FArchive Ar)
	{
		int num = Ar.CustomVer(GUID);
		if (num >= 0)
		{
			return (Type)num;
		}
		EGame game = Ar.Game;
		if (game < EGame.GAME_UE4_25)
		{
			if (game < EGame.GAME_UE4_21)
			{
				return Type.BeforeCustomVersionWasAdded;
			}
			return Type.StoreMarkerNamesOnSkeleton;
		}
		if (game < EGame.GAME_UE4_26)
		{
			return Type.NotifyAndSyncMarkerGuids;
		}
		return Type.GroomBindingSerialization;
	}
}

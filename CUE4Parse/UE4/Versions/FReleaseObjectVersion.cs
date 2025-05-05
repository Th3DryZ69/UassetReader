using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Versions;

public static class FReleaseObjectVersion
{
	public enum Type
	{
		BeforeCustomVersionWasAdded = 0,
		StaticMeshExtendedBoundsFix = 1,
		NoSyncAsyncPhysAsset = 2,
		LevelTransArrayConvertedToTArray = 3,
		AddComponentNodeTemplateUniqueNames = 4,
		UPropertryForMeshSectionSerialize = 5,
		ConvertHLODScreenSize = 6,
		SpeedTreeBillboardSectionInfoFixup = 7,
		EventSectionParameterStringAssetRef = 8,
		SkyLightRemoveMobileIrradianceMap = 9,
		RenameNoTwistToAllowTwistInTwoBoneIK = 10,
		MaterialLayersParameterSerializationRefactor = 11,
		AddSkeletalMeshSectionDisable = 12,
		RemovedMaterialSharedInputCollection = 13,
		HISMCClusterTreeMigration = 14,
		PinDefaultValuesVerified = 15,
		FixBrokenStateMachineReferencesInTransitionGetters = 16,
		MeshDescriptionNewSerialization = 17,
		UnclampRGBColorCurves = 18,
		LinkTimeAnimBlueprintRootDiscoveryBugFix = 19,
		TrailNodeBlendVariableNameChange = 20,
		PropertiesSerializeRepCondition = 21,
		FocalDistanceDisablesDOF = 22,
		Unused_SoundClass2DReverbSend = 23,
		GroomAssetVersion1 = 24,
		GroomAssetVersion2 = 25,
		SerializeAnimModifierState = 26,
		GroomAssetVersion3 = 27,
		DeprecateFilmbackSettings = 28,
		CustomImplicitCollisionType = 29,
		FFieldPathOwnerSerialization = 30,
		MeshDescriptionNewFormat = 31,
		PinTypeIncludesUObjectWrapperFlag = 32,
		WeightFMeshToMeshVertData = 33,
		AnimationGraphNodeBindingsDisplayedAsPins = 34,
		SerializeRigVMOffsetSegmentPaths = 35,
		AbcVelocitiesSupport = 36,
		MarginAddedToConvexAndBox = 37,
		StructureDataAddedToConvex = 38,
		AddedFrontRightUpAxesToLiveLinkPreProcessor = 39,
		FixupCopiedEventSections = 40,
		RemoteControlSerializeFunctionArgumentsSize = 41,
		AddedSubSequenceEntryWarpCounter = 42,
		LonglatTextureCubeDefaultMaxResolution = 43,
		GeometryCollectionCacheRemovesMassToLocal = 44,
		VersionPlusOne = 45,
		LatestVersion = 44
	}

	public static readonly FGuid GUID = new FGuid(2622805282u, 2821083070u, 2485192518u, 1639219920u);

	public static Type Get(FArchive Ar)
	{
		int num = Ar.CustomVer(GUID);
		if (num >= 0)
		{
			return (Type)num;
		}
		EGame game = Ar.Game;
		if (game < EGame.GAME_UE4_20)
		{
			if (game < EGame.GAME_UE4_15)
			{
				if (game < EGame.GAME_UE4_13)
				{
					if (game < EGame.GAME_UE4_11)
					{
						return Type.BeforeCustomVersionWasAdded;
					}
					return Type.StaticMeshExtendedBoundsFix;
				}
				if (game < EGame.GAME_UE4_14)
				{
					return Type.LevelTransArrayConvertedToTArray;
				}
				return Type.AddComponentNodeTemplateUniqueNames;
			}
			if (game < EGame.GAME_UE4_17)
			{
				if (game < EGame.GAME_UE4_16)
				{
					return Type.SpeedTreeBillboardSectionInfoFixup;
				}
				return Type.SkyLightRemoveMobileIrradianceMap;
			}
			if (game < EGame.GAME_UE4_19)
			{
				return Type.RenameNoTwistToAllowTwistInTwoBoneIK;
			}
			return Type.AddSkeletalMeshSectionDisable;
		}
		if (game < EGame.GAME_UE4_25)
		{
			if (game < EGame.GAME_UE4_23)
			{
				if (game < EGame.GAME_UE4_21)
				{
					return Type.MeshDescriptionNewSerialization;
				}
				return Type.TrailNodeBlendVariableNameChange;
			}
			if (game < EGame.GAME_UE4_24)
			{
				return Type.Unused_SoundClass2DReverbSend;
			}
			return Type.DeprecateFilmbackSettings;
		}
		if (game < EGame.GAME_UE4_27)
		{
			if (game < EGame.GAME_UE4_26)
			{
				return Type.FFieldPathOwnerSerialization;
			}
			return Type.StructureDataAddedToConvex;
		}
		if (game < EGame.GAME_UE5_0)
		{
			return Type.LonglatTextureCubeDefaultMaxResolution;
		}
		return Type.GeometryCollectionCacheRemovesMassToLocal;
	}
}

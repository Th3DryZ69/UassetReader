using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Versions;

public static class FUE5ReleaseStreamObjectVersion
{
	public enum Type
	{
		BeforeCustomVersionWasAdded = 0,
		ReflectionMethodEnum = 1,
		WorldPartitionActorDescSerializeHLODInfo = 2,
		RemovingTessellation = 3,
		LevelInstanceSerializeRuntimeBehavior = 4,
		PoseAssetRuntimeRefactor = 5,
		WorldPartitionActorDescSerializeActorFolderPath = 6,
		HairStrandsVertexFormatChange = 7,
		AddChaosMaxLinearAngularSpeed = 8,
		PackedLevelInstanceVersion = 9,
		PackedLevelInstanceBoundsFix = 10,
		CustomPropertyAnimGraphNodesUseOptionalPinManager = 11,
		TextFormatArgumentData64bitSupport = 12,
		MaterialLayerStacksAreNotParameters = 13,
		MaterialInterfaceSavedCachedData = 14,
		AddClothMappingLODBias = 15,
		AddLevelActorPackagingScheme = 16,
		WorldPartitionActorDescSerializeAttachParent = 17,
		ConvertedActorGridPlacementToSpatiallyLoadedFlag = 18,
		ActorGridPlacementDeprecateDefaultValueFixup = 19,
		PackedLevelActorUseWorldPartitionActorDesc = 20,
		AddLevelActorFolders = 21,
		RemoveSkeletalMeshLODModelBulkDatas = 22,
		ExcludeBrightnessFromEncodedHDRCubemap = 23,
		VolumetricCloudSampleCountUnification = 24,
		PoseAssetRawDataGUID = 25,
		ConvolutionBloomIntensity = 26,
		WorldPartitionHLODActorDescSerializeHLODSubActors = 27,
		LargeWorldCoordinates = 28,
		BlueprintPinsUseRealNumbers = 29,
		UpdatedDirectionalLightShadowDefaults = 30,
		GeometryCollectionConvexDefaults = 31,
		ChaosClothFasterDamping = 32,
		WorldPartitionLandscapeActorDescSerializeLandscapeActorGuid = 33,
		AddedInertiaTensorAndRotationOfMassAddedToConvex = 34,
		ChaosInertiaConvertedToVec3 = 35,
		SerializeFloatPinDefaultValuesAsSinglePrecision = 36,
		AnimLayeredBoneBlendMasks = 37,
		StoreReflectionCaptureEncodedHDRDataInRG11B10Format = 38,
		RawAnimSequenceTrackSerializer = 39,
		RemoveDuplicatedStyleInfo = 40,
		LinkedAnimGraphMemberReference = 41,
		VersionPlusOne = 42,
		LatestVersion = 41
	}

	public static readonly FGuid GUID = new FGuid(3634060866u, 616385862u, 2215816360u, 3747878777u);

	public static Type Get(FArchive Ar)
	{
		int num = Ar.CustomVer(GUID);
		if (num >= 0)
		{
			return (Type)num;
		}
		EGame game = Ar.Game;
		if (game >= EGame.GAME_UE5_0)
		{
			if (game < EGame.GAME_UE5_1)
			{
				return Type.SerializeFloatPinDefaultValuesAsSinglePrecision;
			}
			return Type.LinkedAnimGraphMemberReference;
		}
		return Type.BeforeCustomVersionWasAdded;
	}
}

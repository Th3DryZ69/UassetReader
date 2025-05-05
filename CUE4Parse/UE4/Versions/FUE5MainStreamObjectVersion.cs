using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Versions;

public static class FUE5MainStreamObjectVersion
{
	public enum Type
	{
		BeforeCustomVersionWasAdded = 0,
		GeometryCollectionNaniteData = 1,
		GeometryCollectionNaniteDDC = 2,
		RemovingSourceAnimationData = 3,
		MeshDescriptionNewFormat = 4,
		PartitionActorDescSerializeGridGuid = 5,
		ExternalActorsMapDataPackageFlag = 6,
		AnimationAddedBlendProfileModes = 7,
		WorldPartitionActorDescSerializeDataLayers = 8,
		RenamingAnimationNumFrames = 9,
		WorldPartitionHLODActorDescSerializeHLODLayer = 10,
		GeometryCollectionNaniteCooked = 11,
		AddedCookedBoolFontFaceAssets = 12,
		WorldPartitionHLODActorDescSerializeCellHash = 13,
		GeometryCollectionNaniteTransient = 14,
		AddedLandscapeSplineActorDesc = 15,
		AddCollisionConstraintFlag = 16,
		MantleDbSerialize = 17,
		AnimSyncGroupsExplicitSyncMethod = 18,
		FLandscapeActorDescFixupGridIndices = 19,
		FoliageTypeIncludeInHLOD = 20,
		IntroducingAnimationDataModel = 21,
		WorldPartitionActorDescSerializeActorLabel = 22,
		WorldPartitionActorDescSerializeArchivePersistent = 23,
		FixForceExternalActorLevelReferenceDuplicates = 24,
		SerializeMeshDescriptionBase = 25,
		ConvexUsesVerticesArray = 26,
		WorldPartitionActorDescSerializeHLODInfo = 27,
		AddDisabledFlag = 28,
		MoveCustomAttributesToDataModel = 29,
		BlendSpaceRuntimeTriangulation = 30,
		BlendSpaceSmoothingImprovements = 31,
		RemovingTessellationParameters = 32,
		SparseClassDataStructSerialization = 33,
		PackedLevelInstanceBoundsFix = 34,
		AnimNodeConstantDataRefactorPhase0 = 35,
		MaterialSavedCachedData = 36,
		RemoveDecalBlendMode = 37,
		DirLightsAreAtmosphereLightsByDefault = 38,
		WorldPartitionStreamingCellsNamingShortened = 39,
		WorldPartitionActorDescGetStreamingBounds = 40,
		MeshDescriptionVirtualization = 41,
		TextureSourceVirtualization = 42,
		RigVMCopyOpStoreNumBytes = 43,
		MaterialTranslucencyPass = 44,
		GeometryCollectionUserDefinedCollisionShapes = 45,
		RemovedAtmosphericFog = 46,
		SkyAtmosphereAffectsHeightFogWithBetterDefault = 47,
		BlendSpaceSampleOrdering = 48,
		GeometryCollectionCacheRemovesMassToLocal = 49,
		EdGraphPinSourceIndex = 50,
		VirtualizedBulkDataHaveUniqueGuids = 51,
		RigVMMemoryStorageObject = 52,
		RayTracedShadowsType = 53,
		SkelMeshSectionVisibleInRayTracingFlagAdded = 54,
		AnimGraphNodeTaggingAdded = 55,
		DynamicMeshCompactedSerialization = 56,
		ConvertReductionBaseSkeletalMeshBulkDataToInlineReductionCacheData = 57,
		SkeletalMeshLODModelMeshInfo = 58,
		TextureDoScaleMipsForAlphaCoverage = 59,
		VolumetricCloudReflectionSampleCountDefaultUpdate = 60,
		UseTriangleMeshBVH = 61,
		DynamicMeshAttributesWeightMapsAndNames = 62,
		FKControlNamingScheme = 63,
		RichCurveKeyInvalidTangentMode = 64,
		ForceUpdateAnimationAssetCurveTangents = 65,
		SoundWaveVirtualizationUpdate = 66,
		MaterialFeatureLevelNodeFixForSM6 = 67,
		GeometryCollectionPerChildDamageThreshold = 68,
		AddRigidParticleControlFlags = 69,
		LiveLinkComponentPickerPerController = 70,
		RemoveTriangleMeshBVHFaces = 71,
		LensComponentNodalOffset = 72,
		FixGpuAlwaysRunningUpdateScriptNoneInterpolated = 73,
		WorldPartitionSerializeStreamingPolicyOnCook = 74,
		WorldPartitionActorDescRemoveBoundsRelevantSerialization = 75,
		AnimationDataModelInterface_BackedOut = 76,
		LandscapeSplineActorDescDeprecation = 77,
		BackoutAnimationDataModelInterface = 78,
		MobileStationaryLocalLights = 79,
		ManagedArrayCollectionAlwaysSerializeValue = 80,
		LensComponentDistortion = 81,
		ImgMediaPathResolutionWithEngineOrProjectTokens = 82,
		AddLowResolutionHeightField = 83,
		DecreaseLowResolutionHeightField = 84,
		GeometryCollectionDamagePropagationData = 85,
		VehicleFrictionForcePositionChange = 86,
		AddSetMeshDeformerFlag = 87,
		WorldPartitionActorDescActorAndClassPaths = 88,
		ReintroduceAnimationDataModelInterface = 89,
		IncreasedSkinWeightPrecision = 90,
		MaterialHasIsUsedWithVolumetricCloudFlag = 91,
		UpdateHairDescriptionBulkData = 92,
		SpawnActorFromClassTransformScaleMethod = 93,
		RigVMLazyEvaluation = 94,
		PoseAssetRawDataGUIDUpdate = 95,
		RigVMSaveFunctionAccessInModel = 96,
		RigVMSerializeExecuteContextStruct = 97,
		VisualLoggerTimeStampAsDouble = 98,
		MaterialInstanceBasePropertyOverridesThinSurface = 99,
		MaterialRefractionModeNone = 100,
		RigVMSaveSerializedGraphInGraphFunctionData = 101,
		PerPlatformAnimSequenceTargetFrameRate = 102,
		NiagaraGrid2DDefaultUnnamedAttributesZero = 103,
		VersionPlusOne = 104,
		LatestVersion = 103
	}

	public static readonly FGuid GUID = new FGuid(1769854337u, 3863953835u, 2856997356u, 3199710760u);

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
			if (game < EGame.GAME_UE5_0)
			{
				return Type.BeforeCustomVersionWasAdded;
			}
			return Type.TextureDoScaleMipsForAlphaCoverage;
		}
		if (game < EGame.GAME_UE5_2)
		{
			return Type.WorldPartitionActorDescActorAndClassPaths;
		}
		return Type.NiagaraGrid2DDefaultUnnamedAttributesZero;
	}
}

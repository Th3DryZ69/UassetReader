using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Versions;

public static class FFortniteMainBranchObjectVersion
{
	public enum Type
	{
		BeforeCustomVersionWasAdded = 0,
		WorldCompositionTile3DOffset = 1,
		MaterialInstanceSerializeOptimization_ShaderFName = 2,
		CullDistanceRefactor_RemovedDefaultDistance = 3,
		CullDistanceRefactor_NeverCullHLODsByDefault = 4,
		CullDistanceRefactor_NeverCullALODActorsByDefault = 5,
		SaveGeneratedMorphTargetByEngine = 6,
		ConvertReductionSettingOptions = 7,
		StaticParameterTerrainLayerWeightBlendType = 8,
		FixUpNoneNameAnimationCurves = 9,
		EnsureActiveBoneIndicesToContainParents = 10,
		SerializeInstancedStaticMeshRenderData = 11,
		CachedMaterialQualityNodeUsage = 12,
		FontOutlineDropShadowFixup = 13,
		NewSkeletalMeshImporterWorkflow = 14,
		NewLandscapeMaterialPerLOD = 15,
		RemoveUnnecessaryTracksFromPose = 16,
		FoliageLazyObjPtrToSoftObjPtr = 17,
		REVERTED_StoreTimelineNamesInTemplate = 18,
		AddBakePoseOverrideForSkeletalMeshReductionSetting = 19,
		StoreTimelineNamesInTemplate = 20,
		WidgetStopDuplicatingAnimations = 21,
		AllowSkeletalMeshToReduceTheBaseLOD = 22,
		ShrinkCurveTableSize = 23,
		WidgetAnimationDefaultToSelfFail = 24,
		FortHUDElementNowRequiresTag = 25,
		FortMappedCookedAnimation = 26,
		SupportVirtualBoneInRetargeting = 27,
		FixUpWaterMetadata = 28,
		MoveWaterMetadataToActor = 29,
		ReplaceLakeCollision = 30,
		AnimLayerGuidConformation = 31,
		MakeOceanCollisionTransient = 32,
		FFieldPathOwnerSerialization = 33,
		FixUpUnderwaterPostProcessMaterial = 34,
		SupportMultipleWaterBodiesPerExclusionVolume = 35,
		RigVMByteCodeDeterminism = 36,
		LandscapePhysicalMaterialRenderData = 37,
		FixupRuntimeVirtualTextureVolume = 38,
		FixUpRiverCollisionComponents = 39,
		FixDuplicateRiverSplineMeshCollisionComponents = 40,
		ContainsStableActorGUIDs = 41,
		LevelsetSerializationSupportForBodySetup = 42,
		ChaosSolverPropertiesMoved = 43,
		GameFeatureData_MovedComponentListAndCheats = 44,
		ChaosClothAddfictitiousforces = 45,
		ChaosConvexVariableStructureDataAndVerticesArray = 46,
		RemoveLandscapeWaterInfo = 47,
		ChaosClothAddWeightedValue = 48,
		ChaosClothAddTetherStiffnessWeightMap = 49,
		ChaosClothFixLODTransitionMaps = 50,
		ChaosClothAddTetherScaleAndDragLiftWeightMaps = 51,
		ChaosClothAddMaterialWeightMaps = 52,
		SerializeFloatChannelShowCurve = 53,
		LandscapeGrassSingleArray = 54,
		AddedSubSequenceEntryWarpCounter = 55,
		WaterBodyComponentRefactor = 56,
		BPGCCookedEditorTags = 57,
		TerrainLayerWeightsAreNotParameters = 58,
		GravityOverrideDefinedInWorldSpace = 59,
		AnimDynamicsEditableChainParameters = 60,
		WaterZonesRefactor = 61,
		ChaosClothFasterDamping = 62,
		MigratedFunctionHandlersToDefaults = 63,
		ChaosInertiaConvertedToVec3 = 64,
		MigratedEventDefinitionToDefaults = 65,
		LevelInstanceActorGuidSerialize = 66,
		SingleFrameAndKeyAnimModel = 67,
		RemappedEvaluateWorldPositionOffsetInRayTracing = 68,
		WaterBodyComponentCollisionSettingsRefactor = 69,
		WidgetInheritedNamedSlots = 70,
		WaterHLODSupportAdded = 71,
		PoseWatchMigrateSkeletonDrawParametersToPoseElement = 72,
		WaterExclusionVolumeExcludeAllDefault = 73,
		WaterNontessellatedLODSupportAdded = 74,
		HierarchicalSimplificationMethodEnumAdded = 75,
		WorldPartitionStreamingCellsNamingShortened = 76,
		WorldPartitionActorDescSerializeContentBundleGuid = 77,
		WorldPartitionActorDescSerializeActorIsRuntimeOnly = 78,
		NaniteMaterialOverride = 79,
		WorldPartitionHLODActorDescSerializeStats = 80,
		WorldPartitionStreamingSourceComponentTargetDeprecation = 81,
		FixedLocalizationGatherForExternalActorPackage = 82,
		WorldPartitionHLODActorUseSourceCellGuid = 83,
		VersionPlusOne = 84,
		LatestVersion = 83
	}

	public static readonly FGuid GUID = new FGuid(1612519558u, 2892255108u, 2853622750u, 233490390u);

	public static Type Get(FArchive Ar)
	{
		int num = Ar.CustomVer(GUID);
		if (num >= 0)
		{
			return (Type)num;
		}
		EGame game = Ar.Game;
		if (game < EGame.GAME_UE4_27)
		{
			if (game < EGame.GAME_UE4_22)
			{
				if (game >= EGame.GAME_UE4_20)
				{
					if (game < EGame.GAME_UE4_21)
					{
						return Type.CachedMaterialQualityNodeUsage;
					}
					return Type.FoliageLazyObjPtrToSoftObjPtr;
				}
				return Type.BeforeCustomVersionWasAdded;
			}
			if (game < EGame.GAME_UE4_24)
			{
				if (game < EGame.GAME_UE4_23)
				{
					return Type.FortHUDElementNowRequiresTag;
				}
				return Type.SupportVirtualBoneInRetargeting;
			}
			if (game < EGame.GAME_UE4_26)
			{
				return Type.AnimLayerGuidConformation;
			}
			return Type.ChaosSolverPropertiesMoved;
		}
		if (game < EGame.GAME_UE5_1)
		{
			if (game < EGame.GAME_UE5_0)
			{
				return Type.RemoveLandscapeWaterInfo;
			}
			return Type.GravityOverrideDefinedInWorldSpace;
		}
		if (game < EGame.GAME_UE5_2)
		{
			return Type.WorldPartitionHLODActorDescSerializeStats;
		}
		return Type.WorldPartitionHLODActorUseSourceCellGuid;
	}
}

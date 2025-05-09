using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Versions;

public static class FNiagaraCustomVersion
{
	public enum Type
	{
		BeforeCustomVersionWasAdded = 0,
		VMExternalFunctionBindingRework = 1,
		PostLoadCompilationEnabled = 2,
		VMExternalFunctionBindingReworkPartDeux = 3,
		DataInterfacePerInstanceRework = 4,
		NiagaraShaderMaps = 5,
		UpdateSpawnEventGraphCombination = 6,
		DataSetLayoutRework = 7,
		AddedEmitterAndSystemScripts = 8,
		ScriptExecutionContextRework = 9,
		RemovalOfNiagaraVariableIDs = 10,
		SystemEmitterScriptSimulations = 11,
		IntegerRandom = 12,
		AddedEmitterSpawnAttributes = 13,
		NiagaraShaderMapCooking = 14,
		NiagaraShaderMapCooking2 = 15,
		AddedScriptRapidIterationVariables = 16,
		AddedTypeToDataInterfaceInfos = 17,
		EnabledAutogeneratedDefaultValuesForFunctionCallNodes = 18,
		CurveLUTNowOnByDefault = 19,
		ScriptsNowUseAGuidForIdentificationInsteadOfAnIndex = 20,
		NiagaraCombinedGPUSpawnUpdate = 21,
		DontCompileGPUWhenNotNeeded = 22,
		LifeCycleRework = 23,
		NowSerializingReadWriteDataSets = 24,
		TranslatorClearOutBetweenEmitters = 25,
		AddSamplerDataInterfaceParams = 26,
		GPUShadersForceRecompileNeeded = 27,
		PlaybackRangeStoredOnSystem = 28,
		MovedToDerivedDataCache = 29,
		DataInterfacesNotAllocated = 30,
		EmittersHaveGenericUniqueNames = 31,
		MovingTranslatorVersionToGuid = 32,
		AddingParamMapToDataSetBaseNode = 33,
		DataInterfaceComputeShaderParamRefactor = 34,
		CurveLUTRegen = 35,
		AssignmentNodeUsesBeginDefaults = 36,
		AssignmentNodeHasCorrectUsageBitmask = 37,
		EmitterLocalSpaceLiteralConstant = 38,
		TextureDataInterfaceUsesCustomSerialize = 39,
		TextureDataInterfaceSizeSerialize = 40,
		SkelMeshInterfaceAPIImprovements = 41,
		ImproveLoadTimeFixupOfOpAddPins = 42,
		MoveCommonInputMetadataToProperties = 43,
		UseHashesToIdentifyCompileStateOfTopLevelScripts = 44,
		MetaDataAndParametersUpdate = 45,
		MoveInheritanceDataFromTheEmitterHandleToTheEmitter = 46,
		AddLibraryAssetProperty = 47,
		AddAdditionalDefinesProperty = 48,
		RemoveGraphUsageCompileIds = 49,
		AddRIAndDetailLevel = 50,
		ChangeEmitterCompiledDataToSharedRefs = 51,
		DisableSortingByDefault = 52,
		MemorySaving = 53,
		AddSimulationStageUsageEnum = 54,
		AddGeneratedFunctionsToGPUParamInfo = 55,
		PlatformScalingRefactor = 56,
		PrecompileNamespaceFixup = 57,
		FixNullScriptVariables = 58,
		PrecompileNamespaceFixup2 = 59,
		SimulationStageInUsageBitmask = 60,
		StandardizeParameterNames = 61,
		ComponentsOnlyHaveUserVariables = 62,
		RibbonRendererUVRefactor = 63,
		VariablesUseTypeDefRegistry = 64,
		AddLibraryVisibilityProperty = 65,
		SignificanceHandlers = 66,
		ModuleVersioning = 67,
		MoveDefaultValueFromFNiagaraVariableMetaDataToUNiagaraScriptVariable = 68,
		ChangeSystemDeterministicDefault = 69,
		StaticSwitchFunctionPinsUsePersistentGuids = 70,
		VisibilityCullingImprovements = 71,
		AddBakerCameraBookmarks = 72,
		PopulateFunctionCallNodePinNameBindings = 73,
		ComponentRendererSpawnProperty = 74,
		RepopulateFunctionCallNodePinNameBindings = 75,
		EventSpawnsUpdateInitialAttributeValues = 76,
		VersionPlusOne = 77,
		LatestVersion = 76
	}

	public static readonly FGuid GUID = new FGuid(4243946234u, 1349927555u, 3114919512u, 4288687410u);

	public static Type Get(FArchive Ar)
	{
		int num = Ar.CustomVer(GUID);
		if (num >= 0)
		{
			return (Type)num;
		}
		EGame game = Ar.Game;
		if (game < EGame.GAME_UE4_26)
		{
			if (game < EGame.GAME_UE4_24)
			{
				if (game < EGame.GAME_UE4_21)
				{
					if (game < EGame.GAME_UE4_20)
					{
						return Type.BeforeCustomVersionWasAdded;
					}
					return Type.EmitterLocalSpaceLiteralConstant;
				}
				if (game < EGame.GAME_UE4_23)
				{
					return Type.SkelMeshInterfaceAPIImprovements;
				}
				return Type.AddLibraryAssetProperty;
			}
			if (game < EGame.GAME_UE4_25)
			{
				return Type.DisableSortingByDefault;
			}
			return Type.StandardizeParameterNames;
		}
		if (game < EGame.GAME_UE5_0)
		{
			if (game < EGame.GAME_UE4_27)
			{
				return Type.SignificanceHandlers;
			}
			return Type.MoveDefaultValueFromFNiagaraVariableMetaDataToUNiagaraScriptVariable;
		}
		if (game < EGame.GAME_UE5_1)
		{
			return Type.StaticSwitchFunctionPinsUsePersistentGuids;
		}
		return Type.EventSpawnsUpdateInitialAttributeValues;
	}
}

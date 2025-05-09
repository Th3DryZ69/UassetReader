namespace CUE4Parse.GameTypes.FN.Assets.Exports;

public enum ELevelSaveRecordVersion : short
{
	CloudSaveInfoAdded = 0,
	TimestampConversion = 1,
	SoftActorClassReferences = 2,
	SoftActorComponentClassReferences = 3,
	DuplicateNewActorRecordsRemoved = 4,
	StartOfResaveWhenNotLatestVersion = 5,
	LowerCloseThresholdForDuplicates = 6,
	DeprecatedDeleteAndNewActorRecords = 7,
	DependenciesFromSerializedWorld = 8,
	RemovingSerializedDependencies = 9,
	AddingVolumeInfoRecordsMap = 10,
	AddingVolumeGridDependency = 11,
	AddingScale = 12,
	AddingDataHash = 13,
	AddedIslandTemplateId = 14,
	AddedLevelStreamedDeleteRecord = 15,
	UsingSaveActorGUID = 16,
	UsingActorFNameForEditorSpawning = 17,
	AddedPlayerPersistenceUserWipeNumber = 18,
	Unused = 19,
	AddedVkPalette = 20,
	SwitchingToCoreSerialization = 21,
	AddedNavmeshRequired = 22,
	InitialUEFiveChange = 23,
	AddedPersistenceRequired = 24,
	AddedLevelInstance = 25,
	AddedInnerArchiverSerialization = 26,
	AddedHardReferenceTracking = 27,
	AddedDataHeaderSize = 28,
	AddedCrossReferenceSaving = 29,
	SpawningActorsWithConsistentName = 30,
	UpdatePackageNameFromIslandTemplateId = 31,
	LargeWorldCoordinateSerializationChange = 32,
	SeasionTwentyTwoRelease = 33,
	EnforceUniqueLabels = 34,
	AddedConfigForNonSaveGameProperties = 35,
	AddedLevelSaveTags = 36,
	AddedSubobjectSerialization = 37,
	RemoveInvalidEventBindings = 38,
	VersionPlusOne = 39,
	LatestVersion = 38
}

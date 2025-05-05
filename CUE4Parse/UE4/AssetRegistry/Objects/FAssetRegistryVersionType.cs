namespace CUE4Parse.UE4.AssetRegistry.Objects;

public enum FAssetRegistryVersionType
{
	PreVersioning = 0,
	HardSoftDependencies = 1,
	AddAssetRegistryState = 2,
	ChangedAssetData = 3,
	RemovedMD5Hash = 4,
	AddedHardManage = 5,
	AddedCookedMD5Hash = 6,
	AddedDependencyFlags = 7,
	FixedTags = 8,
	WorkspaceDomain = 9,
	PackageImportedClasses = 10,
	PackageFileSummaryVersionChange = 11,
	ObjectResourceOptionalVersionChange = 12,
	AddedChunkHashes = 13,
	ClassPaths = 14,
	RemoveAssetPathFNames = 15,
	AddedHeader = 16,
	VersionPlusOne = 17,
	LatestVersion = 16
}

namespace CUE4Parse.UE4.IO.Objects;

public enum EIoChunkType : byte
{
	Invalid,
	InstallManifest,
	ExportBundleData,
	BulkData,
	OptionalBulkData,
	MemoryMappedBulkData,
	LoaderGlobalMeta,
	LoaderInitialLoadMeta,
	LoaderGlobalNames,
	LoaderGlobalNameHashes,
	ContainerHeader
}

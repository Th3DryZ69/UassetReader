namespace CUE4Parse.UE4.IO.Objects;

public enum EIoChunkType5 : byte
{
	Invalid,
	ExportBundleData,
	BulkData,
	OptionalBulkData,
	MemoryMappedBulkData,
	ScriptObjects,
	ContainerHeader,
	ExternalFile,
	ShaderCodeLibrary,
	ShaderCode,
	PackageStoreEntry,
	DerivedData,
	EditorDerivedData
}

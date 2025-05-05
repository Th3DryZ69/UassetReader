namespace CUE4Parse.UE4.Pak.Objects;

public enum EPakFileVersion
{
	PakFile_Version_Initial = 1,
	PakFile_Version_NoTimestamps = 2,
	PakFile_Version_CompressionEncryption = 3,
	PakFile_Version_IndexEncryption = 4,
	PakFile_Version_RelativeChunkOffsets = 5,
	PakFile_Version_DeleteRecords = 6,
	PakFile_Version_EncryptionKeyGuid = 7,
	PakFile_Version_FNameBasedCompressionMethod = 8,
	PakFile_Version_FrozenIndex = 9,
	PakFile_Version_PathHashIndex = 10,
	PakFile_Version_Fnv64BugFix = 11,
	PakFile_Version_Last = 12,
	PakFile_Version_Invalid = 13,
	PakFile_Version_Latest = 11
}

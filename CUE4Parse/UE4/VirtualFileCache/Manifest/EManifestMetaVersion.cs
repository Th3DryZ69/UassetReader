namespace CUE4Parse.UE4.VirtualFileCache.Manifest;

public enum EManifestMetaVersion : byte
{
	Original = 0,
	SerialisesBuildId = 1,
	LatestPlusOne = 2,
	Latest = 1
}

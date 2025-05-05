namespace CUE4Parse.UE4.Objects.Core.i18N;

public enum ELocResVersion : byte
{
	Legacy = 0,
	Compact = 1,
	Optimized_CRC32 = 2,
	Optimized_CityHash64_UTF16 = 3,
	LatestPlusOne = 4,
	Latest = 3
}

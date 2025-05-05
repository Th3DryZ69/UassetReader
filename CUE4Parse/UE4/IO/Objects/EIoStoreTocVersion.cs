namespace CUE4Parse.UE4.IO.Objects;

public enum EIoStoreTocVersion : byte
{
	Invalid = 0,
	Initial = 1,
	DirectoryIndex = 2,
	PartitionSize = 3,
	PerfectHash = 4,
	PerfectHashWithOverflow = 5,
	LatestPlusOne = 6,
	Latest = 5
}

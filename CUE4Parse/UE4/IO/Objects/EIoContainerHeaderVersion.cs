namespace CUE4Parse.UE4.IO.Objects;

public enum EIoContainerHeaderVersion
{
	BeforeVersionWasAdded = -1,
	Initial = 0,
	LocalizedPackages = 1,
	OptionalSegmentPackages = 2,
	NoExportInfo = 3,
	LatestPlusOne = 4,
	Latest = 3
}

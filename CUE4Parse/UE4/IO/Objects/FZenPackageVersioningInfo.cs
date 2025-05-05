using CUE4Parse.UE4.Objects.Core.Serialization;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.IO.Objects;

public struct FZenPackageVersioningInfo
{
	public EZenPackageVersion ZenVersion;

	public FPackageFileVersion PackageVersion;

	public int LicenseeVersion;

	public FCustomVersionContainer CustomVersions;

	public FZenPackageVersioningInfo(FArchive Ar)
	{
		ZenVersion = Ar.Read<EZenPackageVersion>();
		PackageVersion = Ar.Read<FPackageFileVersion>();
		LicenseeVersion = Ar.Read<int>();
		CustomVersions = new FCustomVersionContainer(Ar);
	}
}

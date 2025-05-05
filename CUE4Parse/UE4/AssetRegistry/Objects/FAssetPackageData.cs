using CUE4Parse.UE4.AssetRegistry.Readers;
using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Objects.Core.Serialization;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.UE4.Versions;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.AssetRegistry.Objects;

[JsonConverter(typeof(FAssetPackageDataConverter))]
public class FAssetPackageData
{
	public readonly FName PackageName;

	public readonly FGuid PackageGuid;

	public readonly FMD5Hash? CookedHash;

	public readonly FName[]? ImportedClasses;

	public readonly long DiskSize;

	public readonly FPackageFileVersion FileVersionUE;

	public readonly int FileVersionLicenseeUE = -1;

	public readonly FCustomVersionContainer CustomVersions;

	public readonly uint Flags;

	public FAssetPackageData(FAssetRegistryArchive Ar)
	{
		PackageName = Ar.ReadFName();
		DiskSize = Ar.Read<long>();
		PackageGuid = Ar.Read<FGuid>();
		if (Ar.Header.Version >= FAssetRegistryVersionType.AddedCookedMD5Hash)
		{
			CookedHash = new FMD5Hash(Ar);
		}
		if (Ar.Header.Version >= FAssetRegistryVersionType.AddedChunkHashes)
		{
			Ar.Position += Ar.Read<int>() * 32;
		}
		if (Ar.Header.Version >= FAssetRegistryVersionType.WorkspaceDomain)
		{
			if (Ar.Header.Version >= FAssetRegistryVersionType.PackageFileSummaryVersionChange)
			{
				FileVersionUE = Ar.Read<FPackageFileVersion>();
			}
			else
			{
				int version = Ar.Read<int>();
				FileVersionUE = FPackageFileVersion.CreateUE4Version(version);
			}
			FileVersionLicenseeUE = Ar.Read<int>();
			Flags = Ar.Read<uint>();
			CustomVersions = new FCustomVersionContainer(Ar);
		}
		if (Ar.Header.Version >= FAssetRegistryVersionType.PackageImportedClasses)
		{
			ImportedClasses = Ar.ReadArray(Ar.ReadFName);
		}
	}
}

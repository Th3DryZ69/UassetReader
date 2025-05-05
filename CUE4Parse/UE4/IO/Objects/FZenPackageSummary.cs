using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.IO.Objects;

public struct FZenPackageSummary
{
	public uint bHasVersioningInfo;

	public uint HeaderSize;

	public FMappedName Name;

	public EPackageFlags PackageFlags;

	public uint CookedHeaderSize;

	public int ImportedPublicExportHashesOffset;

	public int ImportMapOffset;

	public int ExportMapOffset;

	public int ExportBundleEntriesOffset;

	public int GraphDataOffset;

	public int DependencyBundleHeadersOffset;

	public int DependencyBundleEntriesOffset;

	public int ImportedPackageNamesOffset;

	public FZenPackageSummary(FArchive Ar)
	{
		GraphDataOffset = 0;
		DependencyBundleHeadersOffset = 0;
		DependencyBundleEntriesOffset = 0;
		ImportedPackageNamesOffset = 0;
		bHasVersioningInfo = Ar.Read<uint>();
		HeaderSize = Ar.Read<uint>();
		Name = Ar.Read<FMappedName>();
		PackageFlags = Ar.Read<EPackageFlags>();
		CookedHeaderSize = Ar.Read<uint>();
		ImportedPublicExportHashesOffset = Ar.Read<int>();
		ImportMapOffset = Ar.Read<int>();
		ExportMapOffset = Ar.Read<int>();
		ExportBundleEntriesOffset = Ar.Read<int>();
		if (Ar.Game >= EGame.GAME_UE5_2)
		{
			DependencyBundleHeadersOffset = Ar.Read<int>();
			DependencyBundleEntriesOffset = Ar.Read<int>();
			ImportedPackageNamesOffset = Ar.Read<int>();
		}
		else
		{
			GraphDataOffset = Ar.Read<int>();
		}
	}
}

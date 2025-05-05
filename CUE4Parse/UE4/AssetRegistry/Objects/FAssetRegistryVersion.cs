using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.AssetRegistry.Objects;

public static class FAssetRegistryVersion
{
	private static readonly FGuid _GUID = new FGuid(1904189159u, 3920644410u, 2293469490u, 456687879u);

	public static bool TrySerializeVersion(FArchive Ar, out FAssetRegistryVersionType version)
	{
		if (Ar.Read<FGuid>() == _GUID)
		{
			version = Ar.Read<FAssetRegistryVersionType>();
			return true;
		}
		version = FAssetRegistryVersionType.AddedHeader;
		return false;
	}
}

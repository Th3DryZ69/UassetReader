using CUE4Parse.UE4.AssetRegistry.Readers;
using CUE4Parse.UE4.Objects.UObject;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.AssetRegistry.Objects;

public class FAssetIdentifier
{
	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
	public readonly FName PackageName;

	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
	public readonly FName PrimaryAssetType;

	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
	public readonly FName ObjectName;

	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
	public readonly FName ValueName;

	public FAssetIdentifier(FAssetRegistryArchive Ar)
	{
		int num = Ar.ReadByte();
		if ((num & 1) != 0)
		{
			PackageName = Ar.ReadFName();
		}
		if ((num & 2) != 0)
		{
			PrimaryAssetType = Ar.ReadFName();
		}
		if ((num & 4) != 0)
		{
			ObjectName = Ar.ReadFName();
		}
		if ((num & 8) != 0)
		{
			ValueName = Ar.ReadFName();
		}
	}
}

using System.Text;
using CUE4Parse.UE4.Readers;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Objects.UObject;

[JsonConverter(typeof(FTopLevelAssetPathConverter))]
public readonly struct FTopLevelAssetPath : IUStruct
{
	public readonly FName PackageName;

	public readonly FName AssetName;

	public FTopLevelAssetPath(FArchive Ar)
	{
		PackageName = Ar.ReadFName();
		AssetName = Ar.ReadFName();
	}

	public FTopLevelAssetPath(FName packageName, FName assetName)
	{
		PackageName = packageName;
		AssetName = assetName;
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (PackageName.IsNone)
		{
			return string.Empty;
		}
		stringBuilder.Append(PackageName);
		if (!AssetName.IsNone)
		{
			stringBuilder.Append('.').Append(AssetName);
		}
		return stringBuilder.ToString();
	}
}

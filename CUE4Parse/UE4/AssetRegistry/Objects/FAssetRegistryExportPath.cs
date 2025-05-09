using System.Text;
using CUE4Parse.UE4.AssetRegistry.Readers;
using CUE4Parse.UE4.Objects.UObject;

namespace CUE4Parse.UE4.AssetRegistry.Objects;

public class FAssetRegistryExportPath
{
	public readonly FName Class;

	public readonly FName Object;

	public readonly FName Package;

	public FAssetRegistryExportPath(FAssetRegistryArchive Ar)
	{
		Class = ((Ar.Header.Version >= FAssetRegistryVersionType.ClassPaths) ? new FTopLevelAssetPath(Ar).AssetName : Ar.ReadFName());
		Object = Ar.ReadFName();
		Package = Ar.ReadFName();
	}

	public FAssetRegistryExportPath(FNameEntrySerialized classs, FNameEntrySerialized objectt, FNameEntrySerialized package)
	{
		Class = new FName(classs.Name);
		Object = new FName(objectt.Name);
		Package = new FName(package.Name);
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (!Class.IsNone)
		{
			stringBuilder.Append(Class.Text + "'");
		}
		stringBuilder.Append(Package.Text);
		if (!Object.IsNone)
		{
			stringBuilder.Append("." + Object.Text);
		}
		if (!Class.IsNone)
		{
			stringBuilder.Append('\'');
		}
		return stringBuilder.ToString();
	}
}

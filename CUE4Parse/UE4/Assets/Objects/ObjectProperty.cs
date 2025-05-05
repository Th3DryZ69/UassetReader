using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.UObject;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Objects;

[JsonConverter(typeof(ObjectPropertyConverter))]
public class ObjectProperty : FPropertyTagType<FPackageIndex>
{
	public ObjectProperty(FAssetArchive Ar, ReadType type)
	{
		base.Value = ((type != ReadType.ZERO) ? new FPackageIndex(Ar) : new FPackageIndex(Ar, 0));
	}
}

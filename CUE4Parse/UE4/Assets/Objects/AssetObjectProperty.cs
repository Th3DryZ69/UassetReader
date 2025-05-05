using CUE4Parse.UE4.Readers;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Objects;

[JsonConverter(typeof(AssetObjectPropertyConverter))]
public class AssetObjectProperty : FPropertyTagType<string>
{
	public AssetObjectProperty(FArchive Ar, ReadType type)
	{
		base.Value = ((type != ReadType.ZERO) ? Ar.ReadFString() : string.Empty);
	}
}

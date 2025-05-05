using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.UObject;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Objects;

[JsonConverter(typeof(LazyObjectPropertyConverter))]
public class LazyObjectProperty : FPropertyTagType<FUniqueObjectGuid>
{
	public LazyObjectProperty(FAssetArchive Ar, ReadType type)
	{
		base.Value = ((type != ReadType.ZERO) ? Ar.Read<FUniqueObjectGuid>() : default(FUniqueObjectGuid));
	}
}

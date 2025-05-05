using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.UObject;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Objects;

[JsonConverter(typeof(NamePropertyConverter))]
public class NameProperty : FPropertyTagType<FName>
{
	public NameProperty(FAssetArchive Ar, ReadType type)
	{
		base.Value = ((type != ReadType.ZERO) ? Ar.ReadFName() : default(FName));
	}
}

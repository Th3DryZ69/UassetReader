using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.UObject;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Objects;

[JsonConverter(typeof(FieldPathPropertyConverter))]
public class FieldPathProperty : FPropertyTagType<FFieldPath>
{
	public FieldPathProperty(FAssetArchive Ar, ReadType type)
	{
		base.Value = ((type != ReadType.ZERO) ? new FFieldPath(Ar) : new FFieldPath());
	}
}

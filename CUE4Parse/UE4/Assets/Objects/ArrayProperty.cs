using CUE4Parse.UE4.Assets.Readers;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Objects;

[JsonConverter(typeof(ArrayPropertyConverter))]
public class ArrayProperty : FPropertyTagType<UScriptArray>
{
	public ArrayProperty(FAssetArchive Ar, FPropertyTagData? tagData, ReadType type)
	{
		base.Value = ((type != ReadType.ZERO) ? new UScriptArray(Ar, tagData) : new UScriptArray(tagData?.InnerType ?? "ZeroUnknown"));
	}
}

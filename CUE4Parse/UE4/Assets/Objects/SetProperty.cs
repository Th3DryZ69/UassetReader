using CUE4Parse.UE4.Assets.Readers;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Objects;

[JsonConverter(typeof(SetPropertyConverter))]
public class SetProperty : FPropertyTagType<UScriptSet>
{
	public SetProperty(FAssetArchive Ar, FPropertyTagData? tagData, ReadType type)
	{
		base.Value = ((type != ReadType.ZERO) ? new UScriptSet(Ar, tagData) : new UScriptSet());
	}
}

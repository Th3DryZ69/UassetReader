using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.Utils;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Objects;

[JsonConverter(typeof(StructPropertyConverter))]
public class StructProperty : FPropertyTagType<UScriptStruct>
{
	public StructProperty(FAssetArchive Ar, FPropertyTagData? tagData, ReadType type)
	{
		base.Value = new UScriptStruct(Ar, tagData?.StructType, tagData?.Struct, type);
	}

	public override string ToString()
	{
		return base.Value.ToString().SubstringBeforeLast(')') + ", StructProperty)";
	}
}

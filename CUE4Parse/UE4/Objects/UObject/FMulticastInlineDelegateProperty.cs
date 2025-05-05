using CUE4Parse.UE4.Assets.Readers;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Objects.UObject;

public class FMulticastInlineDelegateProperty : FProperty
{
	public FPackageIndex SignatureFunction;

	public override void Deserialize(FAssetArchive Ar)
	{
		base.Deserialize(Ar);
		SignatureFunction = new FPackageIndex(Ar);
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer)
	{
		base.WriteJson(writer, serializer);
		writer.WritePropertyName("SignatureFunction");
		serializer.Serialize(writer, SignatureFunction);
	}
}

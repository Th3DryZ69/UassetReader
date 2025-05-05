using CUE4Parse.UE4.Assets.Readers;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Objects.UObject;

public class FStructProperty : FProperty
{
	public FPackageIndex Struct;

	public override void Deserialize(FAssetArchive Ar)
	{
		base.Deserialize(Ar);
		Struct = new FPackageIndex(Ar);
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer)
	{
		base.WriteJson(writer, serializer);
		writer.WritePropertyName("Struct");
		serializer.Serialize(writer, Struct);
	}
}

using CUE4Parse.UE4.Assets.Readers;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Objects.UObject;

public class FArrayProperty : FProperty
{
	public FProperty? Inner;

	public override void Deserialize(FAssetArchive Ar)
	{
		base.Deserialize(Ar);
		Inner = (FProperty)FField.SerializeSingleField(Ar);
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer)
	{
		base.WriteJson(writer, serializer);
		writer.WritePropertyName("Inner");
		serializer.Serialize(writer, Inner);
	}
}

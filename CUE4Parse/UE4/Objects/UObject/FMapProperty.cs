using CUE4Parse.UE4.Assets.Readers;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Objects.UObject;

public class FMapProperty : FProperty
{
	public FProperty? KeyProp;

	public FProperty? ValueProp;

	public override void Deserialize(FAssetArchive Ar)
	{
		base.Deserialize(Ar);
		KeyProp = (FProperty)FField.SerializeSingleField(Ar);
		ValueProp = (FProperty)FField.SerializeSingleField(Ar);
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer)
	{
		base.WriteJson(writer, serializer);
		writer.WritePropertyName("KeyProp");
		serializer.Serialize(writer, KeyProp);
		writer.WritePropertyName("ValueProp");
		serializer.Serialize(writer, ValueProp);
	}
}

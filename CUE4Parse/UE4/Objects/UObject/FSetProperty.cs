using CUE4Parse.UE4.Assets.Readers;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Objects.UObject;

public class FSetProperty : FProperty
{
	public FProperty? ElementProp;

	public override void Deserialize(FAssetArchive Ar)
	{
		base.Deserialize(Ar);
		ElementProp = (FProperty)FField.SerializeSingleField(Ar);
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer)
	{
		base.WriteJson(writer, serializer);
		writer.WritePropertyName("ElementProp");
		serializer.Serialize(writer, ElementProp);
	}
}

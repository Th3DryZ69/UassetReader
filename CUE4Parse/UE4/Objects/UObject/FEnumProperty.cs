using CUE4Parse.UE4.Assets.Readers;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Objects.UObject;

public class FEnumProperty : FProperty
{
	public FNumericProperty? UnderlyingProp;

	public FPackageIndex Enum;

	public override void Deserialize(FAssetArchive Ar)
	{
		base.Deserialize(Ar);
		Enum = new FPackageIndex(Ar);
		UnderlyingProp = (FNumericProperty)FField.SerializeSingleField(Ar);
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer)
	{
		base.WriteJson(writer, serializer);
		writer.WritePropertyName("Enum");
		serializer.Serialize(writer, Enum);
		writer.WritePropertyName("UnderlyingProp");
		serializer.Serialize(writer, UnderlyingProp);
	}
}

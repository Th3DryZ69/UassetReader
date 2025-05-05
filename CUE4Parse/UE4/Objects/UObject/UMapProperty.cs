using CUE4Parse.UE4.Assets.Readers;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Objects.UObject;

public class UMapProperty : UProperty
{
	public FPackageIndex KeyProp;

	public FPackageIndex ValueProp;

	public override void Deserialize(FAssetArchive Ar, long validPos)
	{
		base.Deserialize(Ar, validPos);
		KeyProp = new FPackageIndex(Ar);
		ValueProp = new FPackageIndex(Ar);
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

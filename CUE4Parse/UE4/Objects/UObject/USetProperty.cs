using CUE4Parse.UE4.Assets.Readers;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Objects.UObject;

public class USetProperty : UProperty
{
	public FPackageIndex ElementProp;

	public override void Deserialize(FAssetArchive Ar, long validPos)
	{
		base.Deserialize(Ar, validPos);
		ElementProp = new FPackageIndex(Ar);
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer)
	{
		base.WriteJson(writer, serializer);
		writer.WritePropertyName("ElementProp");
		serializer.Serialize(writer, ElementProp);
	}
}

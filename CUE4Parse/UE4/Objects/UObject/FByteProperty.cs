using CUE4Parse.UE4.Assets.Readers;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Objects.UObject;

public class FByteProperty : FNumericProperty
{
	public FPackageIndex Enum;

	public override void Deserialize(FAssetArchive Ar)
	{
		base.Deserialize(Ar);
		Enum = new FPackageIndex(Ar);
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer)
	{
		base.WriteJson(writer, serializer);
		writer.WritePropertyName("Enum");
		serializer.Serialize(writer, Enum);
	}
}

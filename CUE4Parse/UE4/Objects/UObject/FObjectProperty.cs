using CUE4Parse.UE4.Assets.Readers;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Objects.UObject;

public class FObjectProperty : FProperty
{
	public FPackageIndex PropertyClass;

	public override void Deserialize(FAssetArchive Ar)
	{
		base.Deserialize(Ar);
		PropertyClass = new FPackageIndex(Ar);
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer)
	{
		base.WriteJson(writer, serializer);
		writer.WritePropertyName("PropertyClass");
		serializer.Serialize(writer, PropertyClass);
	}
}

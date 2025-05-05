using CUE4Parse.UE4.Assets.Readers;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Objects.UObject;

public class FSoftClassProperty : FObjectProperty
{
	public FPackageIndex MetaClass;

	public override void Deserialize(FAssetArchive Ar)
	{
		base.Deserialize(Ar);
		MetaClass = new FPackageIndex(Ar);
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer)
	{
		base.WriteJson(writer, serializer);
		writer.WritePropertyName("MetaClass");
		serializer.Serialize(writer, MetaClass);
	}
}

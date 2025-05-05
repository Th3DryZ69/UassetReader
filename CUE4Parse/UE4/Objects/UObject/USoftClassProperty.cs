using CUE4Parse.UE4.Assets.Readers;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Objects.UObject;

public class USoftClassProperty : UObjectPropertyBase
{
	public FPackageIndex MetaClass;

	public override void Deserialize(FAssetArchive Ar, long validPos)
	{
		base.Deserialize(Ar, validPos);
		MetaClass = new FPackageIndex(Ar);
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer)
	{
		base.WriteJson(writer, serializer);
		writer.WritePropertyName("MetaClass");
		serializer.Serialize(writer, MetaClass);
	}
}

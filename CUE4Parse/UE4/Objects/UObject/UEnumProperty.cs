using CUE4Parse.UE4.Assets.Readers;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Objects.UObject;

public class UEnumProperty : UProperty
{
	public FPackageIndex UnderlyingProp;

	public FPackageIndex Enum;

	public override void Deserialize(FAssetArchive Ar, long validPos)
	{
		base.Deserialize(Ar, validPos);
		Enum = new FPackageIndex(Ar);
		UnderlyingProp = new FPackageIndex(Ar);
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

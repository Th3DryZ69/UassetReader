using CUE4Parse.UE4.Assets.Readers;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Objects.UObject;

public class FInterfaceProperty : FProperty
{
	public FPackageIndex InterfaceClass;

	public override void Deserialize(FAssetArchive Ar)
	{
		base.Deserialize(Ar);
		InterfaceClass = new FPackageIndex(Ar);
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer)
	{
		base.WriteJson(writer, serializer);
		writer.WritePropertyName("InterfaceClass");
		serializer.Serialize(writer, InterfaceClass);
	}
}

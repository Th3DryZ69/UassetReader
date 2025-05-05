using CUE4Parse.UE4.Assets.Readers;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Objects.UObject;

public class UBoolProperty : UProperty
{
	public byte BoolSize;

	public bool bIsNativeBool;

	public override void Deserialize(FAssetArchive Ar, long validPos)
	{
		base.Deserialize(Ar, validPos);
		BoolSize = Ar.Read<byte>();
		bIsNativeBool = Ar.ReadFlag();
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer)
	{
		base.WriteJson(writer, serializer);
		writer.WritePropertyName("BoolSize");
		writer.WriteValue(BoolSize);
		writer.WritePropertyName("bIsNativeBool");
		writer.WriteValue(bIsNativeBool);
	}
}

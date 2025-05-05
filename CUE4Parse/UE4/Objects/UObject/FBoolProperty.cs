using CUE4Parse.UE4.Assets.Readers;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Objects.UObject;

public class FBoolProperty : FProperty
{
	public byte FieldSize;

	public byte ByteOffset;

	public byte ByteMask;

	public byte FieldMask;

	public byte BoolSize;

	public bool bIsNativeBool;

	public override void Deserialize(FAssetArchive Ar)
	{
		base.Deserialize(Ar);
		FieldSize = Ar.Read<byte>();
		ByteOffset = Ar.Read<byte>();
		ByteMask = Ar.Read<byte>();
		FieldMask = Ar.Read<byte>();
		BoolSize = Ar.Read<byte>();
		bIsNativeBool = Ar.ReadFlag();
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer)
	{
		base.WriteJson(writer, serializer);
		writer.WritePropertyName("FieldSize");
		writer.WriteValue(FieldSize);
		writer.WritePropertyName("ByteOffset");
		writer.WriteValue(ByteOffset);
		writer.WritePropertyName("ByteMask");
		writer.WriteValue(ByteMask);
		writer.WritePropertyName("FieldMask");
		writer.WriteValue(FieldMask);
		writer.WritePropertyName("BoolSize");
		writer.WriteValue(BoolSize);
		writer.WritePropertyName("bIsNativeBool");
		writer.WriteValue(bIsNativeBool);
	}
}

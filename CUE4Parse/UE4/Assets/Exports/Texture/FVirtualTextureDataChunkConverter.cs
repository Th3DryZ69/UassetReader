using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.Texture;

public class FVirtualTextureDataChunkConverter : JsonConverter<FVirtualTextureDataChunk>
{
	public override void WriteJson(JsonWriter writer, FVirtualTextureDataChunk value, JsonSerializer serializer)
	{
		writer.WriteStartObject();
		writer.WritePropertyName("BulkData");
		serializer.Serialize(writer, value.BulkData);
		writer.WritePropertyName("SizeInBytes");
		writer.WriteValue(value.SizeInBytes);
		writer.WritePropertyName("CodecPayloadSize");
		writer.WriteValue(value.CodecPayloadSize);
		writer.WritePropertyName("CodecPayloadOffset");
		serializer.Serialize(writer, value.CodecPayloadOffset);
		writer.WritePropertyName("CodecType");
		writer.WriteStartArray();
		EVirtualTextureCodec[] codecType = value.CodecType;
		for (int i = 0; i < codecType.Length; i++)
		{
			EVirtualTextureCodec eVirtualTextureCodec = codecType[i];
			writer.WriteValue(eVirtualTextureCodec.ToString());
		}
		writer.WriteEndArray();
		writer.WriteEndObject();
	}

	public override FVirtualTextureDataChunk ReadJson(JsonReader reader, Type objectType, FVirtualTextureDataChunk existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}

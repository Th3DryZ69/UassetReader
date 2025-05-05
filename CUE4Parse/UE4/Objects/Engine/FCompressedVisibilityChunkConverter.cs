using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Objects.Engine;

public class FCompressedVisibilityChunkConverter : JsonConverter<FCompressedVisibilityChunk>
{
	public override void WriteJson(JsonWriter writer, FCompressedVisibilityChunk value, JsonSerializer serializer)
	{
		writer.WriteStartObject();
		writer.WritePropertyName("bCompressed");
		writer.WriteValue(value.bCompressed);
		writer.WritePropertyName("UncompressedSize");
		writer.WriteValue(value.UncompressedSize);
		writer.WriteEndObject();
	}

	public override FCompressedVisibilityChunk ReadJson(JsonReader reader, Type objectType, FCompressedVisibilityChunk existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}

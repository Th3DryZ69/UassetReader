using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.Sound;

public class FStreamedAudioChunkConverter : JsonConverter<FStreamedAudioChunk>
{
	public override void WriteJson(JsonWriter writer, FStreamedAudioChunk value, JsonSerializer serializer)
	{
		writer.WriteStartObject();
		writer.WritePropertyName("DataSize");
		writer.WriteValue(value.DataSize);
		writer.WritePropertyName("AudioDataSize");
		writer.WriteValue(value.AudioDataSize);
		writer.WritePropertyName("BulkData");
		serializer.Serialize(writer, value.BulkData);
		writer.WriteEndObject();
	}

	public override FStreamedAudioChunk ReadJson(JsonReader reader, Type objectType, FStreamedAudioChunk existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}

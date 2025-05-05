using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.Sound;

public class FStreamedAudioPlatformDataConverter : JsonConverter<FStreamedAudioPlatformData>
{
	public override void WriteJson(JsonWriter writer, FStreamedAudioPlatformData value, JsonSerializer serializer)
	{
		writer.WriteStartObject();
		writer.WritePropertyName("NumChunks");
		writer.WriteValue(value.NumChunks);
		writer.WritePropertyName("AudioFormat");
		serializer.Serialize(writer, value.AudioFormat);
		writer.WritePropertyName("Chunks");
		serializer.Serialize(writer, value.Chunks);
		writer.WriteEndObject();
	}

	public override FStreamedAudioPlatformData ReadJson(JsonReader reader, Type objectType, FStreamedAudioPlatformData existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}

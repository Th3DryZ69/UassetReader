using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.Wwise;

public class FAkMediaDataChunkConverter : JsonConverter<FAkMediaDataChunk>
{
	public override void WriteJson(JsonWriter writer, FAkMediaDataChunk value, JsonSerializer serializer)
	{
		writer.WriteStartObject();
		writer.WritePropertyName("BulkData");
		serializer.Serialize(writer, value.Data);
		writer.WritePropertyName("IsPrefetch");
		writer.WriteValue(value.IsPrefetch);
		writer.WriteEndObject();
	}

	public override FAkMediaDataChunk ReadJson(JsonReader reader, Type objectType, FAkMediaDataChunk existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}

using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Wwise;

public class WwiseConverter : JsonConverter<WwiseReader>
{
	public override void WriteJson(JsonWriter writer, WwiseReader value, JsonSerializer serializer)
	{
		writer.WriteStartObject();
		writer.WritePropertyName("Header");
		serializer.Serialize(writer, value.Header);
		writer.WritePropertyName("Folders");
		serializer.Serialize(writer, value.Folders);
		writer.WritePropertyName("Initialization");
		serializer.Serialize(writer, value.Initialization);
		writer.WritePropertyName("WemIndexes");
		serializer.Serialize(writer, value.WemIndexes);
		writer.WritePropertyName("Hierarchy");
		serializer.Serialize(writer, value.Hierarchy);
		writer.WritePropertyName("IdToString");
		serializer.Serialize(writer, value.IdToString);
		writer.WritePropertyName("Platform");
		writer.WriteValue(value.Platform);
		writer.WriteEndObject();
	}

	public override WwiseReader ReadJson(JsonReader reader, Type objectType, WwiseReader existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}

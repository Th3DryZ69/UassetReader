using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Wwise.Objects;

public class AkFolderConverter : JsonConverter<AkFolder>
{
	public override void WriteJson(JsonWriter writer, AkFolder value, JsonSerializer serializer)
	{
		writer.WriteStartObject();
		writer.WritePropertyName("Offset");
		writer.WriteValue(value.Offset);
		writer.WritePropertyName("Id");
		writer.WriteValue(value.Id);
		writer.WritePropertyName("Name");
		writer.WriteValue(value.Name);
		writer.WritePropertyName("Entries");
		serializer.Serialize(writer, value.Entries);
		writer.WriteEndObject();
	}

	public override AkFolder ReadJson(JsonReader reader, Type objectType, AkFolder existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}

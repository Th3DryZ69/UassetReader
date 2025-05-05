using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CUE4Parse.GameTypes.FF7.Objects;

public class FEndTextResourceStringsConverter : JsonConverter<FEndTextResourceStrings>
{
	public override void WriteJson(JsonWriter writer, FEndTextResourceStrings value, JsonSerializer serializer)
	{
		writer.WriteStartObject();
		Dictionary<string, string>? entries = value.Entries;
		if (entries != null && entries.Count > 0)
		{
			writer.WritePropertyName("Entries");
			serializer.Serialize(writer, value.Entries);
		}
		writer.WriteEndObject();
	}

	public override FEndTextResourceStrings ReadJson(JsonReader reader, Type objectType, FEndTextResourceStrings? existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}

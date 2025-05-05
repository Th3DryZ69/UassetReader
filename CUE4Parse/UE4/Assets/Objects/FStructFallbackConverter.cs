using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Objects;

public class FStructFallbackConverter : JsonConverter<FStructFallback>
{
	public override void WriteJson(JsonWriter writer, FStructFallback value, JsonSerializer serializer)
	{
		writer.WriteStartObject();
		foreach (FPropertyTag property in value.Properties)
		{
			writer.WritePropertyName(property.Name.Text);
			serializer.Serialize(writer, property.Tag);
		}
		writer.WriteEndObject();
	}

	public override FStructFallback ReadJson(JsonReader reader, Type objectType, FStructFallback existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}

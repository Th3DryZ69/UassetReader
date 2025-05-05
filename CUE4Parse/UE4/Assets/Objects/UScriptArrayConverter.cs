using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Objects;

public class UScriptArrayConverter : JsonConverter<UScriptArray>
{
	public override void WriteJson(JsonWriter writer, UScriptArray value, JsonSerializer serializer)
	{
		writer.WriteStartArray();
		foreach (FPropertyTagType property in value.Properties)
		{
			serializer.Serialize(writer, property);
		}
		writer.WriteEndArray();
	}

	public override UScriptArray ReadJson(JsonReader reader, Type objectType, UScriptArray existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}

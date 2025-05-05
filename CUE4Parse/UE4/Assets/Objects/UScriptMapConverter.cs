using System;
using System.Collections.Generic;
using CUE4Parse.Utils;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Objects;

public class UScriptMapConverter : JsonConverter<UScriptMap>
{
	public override void WriteJson(JsonWriter writer, UScriptMap value, JsonSerializer serializer)
	{
		writer.WriteStartArray();
		foreach (KeyValuePair<FPropertyTagType, FPropertyTagType> property in value.Properties)
		{
			if (property.Key is StructProperty)
			{
				writer.WriteStartObject();
				writer.WritePropertyName("Key");
				serializer.Serialize(writer, property.Key);
				writer.WritePropertyName("Value");
				serializer.Serialize(writer, property.Value);
				writer.WriteEndObject();
			}
			else
			{
				writer.WriteStartObject();
				writer.WritePropertyName(property.Key?.ToString().SubstringBefore('(').Trim() ?? "no key name???");
				serializer.Serialize(writer, property.Value);
				writer.WriteEndObject();
			}
		}
		writer.WriteEndArray();
	}

	public override UScriptMap ReadJson(JsonReader reader, Type objectType, UScriptMap existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}

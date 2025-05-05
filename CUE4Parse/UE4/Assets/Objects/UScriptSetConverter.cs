using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Objects;

public class UScriptSetConverter : JsonConverter<UScriptSet>
{
	public override void WriteJson(JsonWriter writer, UScriptSet value, JsonSerializer serializer)
	{
		writer.WriteStartArray();
		foreach (FPropertyTagType property in value.Properties)
		{
			serializer.Serialize(writer, property);
		}
		writer.WriteEndArray();
	}

	public override UScriptSet ReadJson(JsonReader reader, Type objectType, UScriptSet existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}

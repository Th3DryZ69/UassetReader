using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Objects.UObject;

public class FFieldConverter : JsonConverter<FField>
{
	public override void WriteJson(JsonWriter writer, FField value, JsonSerializer serializer)
	{
		writer.WriteStartObject();
		value.WriteJson(writer, serializer);
		writer.WriteEndObject();
	}

	public override FField ReadJson(JsonReader reader, Type objectType, FField existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}

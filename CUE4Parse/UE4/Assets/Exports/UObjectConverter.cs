using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports;

public class UObjectConverter : JsonConverter<UObject>
{
	public override void WriteJson(JsonWriter writer, UObject value, JsonSerializer serializer)
	{
		writer.WriteStartObject();
		value.WriteJson(writer, serializer);
		writer.WriteEndObject();
	}

	public override UObject ReadJson(JsonReader reader, Type objectType, UObject existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}

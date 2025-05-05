using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Objects.UObject;

public class FNameConverter : JsonConverter<FName>
{
	public override void WriteJson(JsonWriter writer, FName value, JsonSerializer serializer)
	{
		writer.WriteValue(value.Text);
	}

	public override FName ReadJson(JsonReader reader, Type objectType, FName existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}

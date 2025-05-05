using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Objects;

public class StrPropertyConverter : JsonConverter<StrProperty>
{
	public override void WriteJson(JsonWriter writer, StrProperty value, JsonSerializer serializer)
	{
		writer.WriteValue(value.Value);
	}

	public override StrProperty ReadJson(JsonReader reader, Type objectType, StrProperty existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}

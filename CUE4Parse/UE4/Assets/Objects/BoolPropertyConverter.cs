using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Objects;

public class BoolPropertyConverter : JsonConverter<BoolProperty>
{
	public override void WriteJson(JsonWriter writer, BoolProperty value, JsonSerializer serializer)
	{
		writer.WriteValue(value.Value);
	}

	public override BoolProperty ReadJson(JsonReader reader, Type objectType, BoolProperty existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}

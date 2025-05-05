using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Objects;

public class DoublePropertyConverter : JsonConverter<DoubleProperty>
{
	public override void WriteJson(JsonWriter writer, DoubleProperty value, JsonSerializer serializer)
	{
		writer.WriteValue(value.Value);
	}

	public override DoubleProperty ReadJson(JsonReader reader, Type objectType, DoubleProperty existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}

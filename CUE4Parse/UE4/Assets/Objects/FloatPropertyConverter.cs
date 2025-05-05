using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Objects;

public class FloatPropertyConverter : JsonConverter<FloatProperty>
{
	public override void WriteJson(JsonWriter writer, FloatProperty value, JsonSerializer serializer)
	{
		writer.WriteValue(value.Value);
	}

	public override FloatProperty ReadJson(JsonReader reader, Type objectType, FloatProperty existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}

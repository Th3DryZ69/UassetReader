using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Objects;

public class IntPropertyConverter : JsonConverter<IntProperty>
{
	public override void WriteJson(JsonWriter writer, IntProperty value, JsonSerializer serializer)
	{
		writer.WriteValue(value.Value);
	}

	public override IntProperty ReadJson(JsonReader reader, Type objectType, IntProperty existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}

using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Objects;

public class Int8PropertyConverter : JsonConverter<Int8Property>
{
	public override void WriteJson(JsonWriter writer, Int8Property value, JsonSerializer serializer)
	{
		writer.WriteValue(value.Value);
	}

	public override Int8Property ReadJson(JsonReader reader, Type objectType, Int8Property existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}

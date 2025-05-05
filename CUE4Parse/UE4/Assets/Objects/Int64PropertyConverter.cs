using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Objects;

public class Int64PropertyConverter : JsonConverter<Int64Property>
{
	public override void WriteJson(JsonWriter writer, Int64Property value, JsonSerializer serializer)
	{
		writer.WriteValue(value.Value);
	}

	public override Int64Property ReadJson(JsonReader reader, Type objectType, Int64Property existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}

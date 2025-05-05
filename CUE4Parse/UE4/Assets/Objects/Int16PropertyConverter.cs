using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Objects;

public class Int16PropertyConverter : JsonConverter<Int16Property>
{
	public override void WriteJson(JsonWriter writer, Int16Property value, JsonSerializer serializer)
	{
		writer.WriteValue(value.Value);
	}

	public override Int16Property ReadJson(JsonReader reader, Type objectType, Int16Property existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}

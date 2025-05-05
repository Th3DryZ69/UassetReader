using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Objects;

public class UInt64PropertyConverter : JsonConverter<UInt64Property>
{
	public override void WriteJson(JsonWriter writer, UInt64Property value, JsonSerializer serializer)
	{
		writer.WriteValue(value.Value);
	}

	public override UInt64Property ReadJson(JsonReader reader, Type objectType, UInt64Property existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}

using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Objects;

public class UInt32PropertyConverter : JsonConverter<UInt32Property>
{
	public override void WriteJson(JsonWriter writer, UInt32Property value, JsonSerializer serializer)
	{
		writer.WriteValue(value.Value);
	}

	public override UInt32Property ReadJson(JsonReader reader, Type objectType, UInt32Property existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}

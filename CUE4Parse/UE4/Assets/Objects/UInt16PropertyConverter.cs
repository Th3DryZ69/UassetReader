using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Objects;

public class UInt16PropertyConverter : JsonConverter<UInt16Property>
{
	public override void WriteJson(JsonWriter writer, UInt16Property value, JsonSerializer serializer)
	{
		writer.WriteValue(value.Value);
	}

	public override UInt16Property ReadJson(JsonReader reader, Type objectType, UInt16Property existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}

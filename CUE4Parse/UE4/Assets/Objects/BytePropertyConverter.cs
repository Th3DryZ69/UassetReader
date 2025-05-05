using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Objects;

public class BytePropertyConverter : JsonConverter<ByteProperty>
{
	public override void WriteJson(JsonWriter writer, ByteProperty value, JsonSerializer serializer)
	{
		writer.WriteValue(value.Value);
	}

	public override ByteProperty ReadJson(JsonReader reader, Type objectType, ByteProperty existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}

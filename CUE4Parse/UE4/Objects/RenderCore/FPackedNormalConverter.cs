using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Objects.RenderCore;

public class FPackedNormalConverter : JsonConverter<FPackedNormal>
{
	public override void WriteJson(JsonWriter writer, FPackedNormal value, JsonSerializer serializer)
	{
		writer.WriteStartObject();
		writer.WritePropertyName("Data");
		writer.WriteValue(value.Data);
		writer.WriteEndObject();
	}

	public override FPackedNormal ReadJson(JsonReader reader, Type objectType, FPackedNormal existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}

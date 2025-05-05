using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Objects.RenderCore;

public class FPackedRGBA16NConverter : JsonConverter<FPackedRGBA16N>
{
	public override void WriteJson(JsonWriter writer, FPackedRGBA16N value, JsonSerializer serializer)
	{
		writer.WriteStartObject();
		writer.WritePropertyName("X");
		writer.WriteValue(value.X);
		writer.WritePropertyName("Y");
		writer.WriteValue(value.Y);
		writer.WritePropertyName("Z");
		writer.WriteValue(value.Z);
		writer.WritePropertyName("W");
		writer.WriteValue(value.X);
		writer.WriteEndObject();
	}

	public override FPackedRGBA16N ReadJson(JsonReader reader, Type objectType, FPackedRGBA16N existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}

using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Objects.Meshes;

public class FPositionVertexBufferConverter : JsonConverter<FPositionVertexBuffer>
{
	public override void WriteJson(JsonWriter writer, FPositionVertexBuffer value, JsonSerializer serializer)
	{
		writer.WriteStartObject();
		writer.WritePropertyName("Stride");
		writer.WriteValue(value.Stride);
		writer.WritePropertyName("NumVertices");
		writer.WriteValue(value.NumVertices);
		writer.WriteEndObject();
	}

	public override FPositionVertexBuffer ReadJson(JsonReader reader, Type objectType, FPositionVertexBuffer existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}

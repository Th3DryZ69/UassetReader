using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Objects.Meshes;

public class FColorVertexBufferConverter : JsonConverter<FColorVertexBuffer>
{
	public override void WriteJson(JsonWriter writer, FColorVertexBuffer value, JsonSerializer serializer)
	{
		writer.WriteStartObject();
		writer.WritePropertyName("Stride");
		writer.WriteValue(value.Stride);
		writer.WritePropertyName("NumVertices");
		writer.WriteValue(value.NumVertices);
		writer.WriteEndObject();
	}

	public override FColorVertexBuffer ReadJson(JsonReader reader, Type objectType, FColorVertexBuffer existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}

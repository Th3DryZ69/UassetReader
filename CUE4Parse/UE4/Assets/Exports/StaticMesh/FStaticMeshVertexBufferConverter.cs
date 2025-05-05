using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.StaticMesh;

public class FStaticMeshVertexBufferConverter : JsonConverter<FStaticMeshVertexBuffer>
{
	public override void WriteJson(JsonWriter writer, FStaticMeshVertexBuffer value, JsonSerializer serializer)
	{
		writer.WriteStartObject();
		writer.WritePropertyName("NumTexCoords");
		writer.WriteValue(value.NumTexCoords);
		writer.WritePropertyName("NumVertices");
		writer.WriteValue(value.NumVertices);
		writer.WritePropertyName("Strides");
		writer.WriteValue(value.Strides);
		writer.WritePropertyName("UseHighPrecisionTangentBasis");
		writer.WriteValue(value.UseHighPrecisionTangentBasis);
		writer.WritePropertyName("UseFullPrecisionUVs");
		writer.WriteValue(value.UseFullPrecisionUVs);
		writer.WriteEndObject();
	}

	public override FStaticMeshVertexBuffer ReadJson(JsonReader reader, Type objectType, FStaticMeshVertexBuffer existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}

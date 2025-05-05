using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.StaticMesh;

public class FStaticMeshSectionConverter : JsonConverter<FStaticMeshSection>
{
	public override void WriteJson(JsonWriter writer, FStaticMeshSection value, JsonSerializer serializer)
	{
		writer.WriteStartObject();
		writer.WritePropertyName("MaterialIndex");
		writer.WriteValue(value.MaterialIndex);
		writer.WritePropertyName("FirstIndex");
		writer.WriteValue(value.FirstIndex);
		writer.WritePropertyName("NumTriangles");
		writer.WriteValue(value.NumTriangles);
		writer.WritePropertyName("MinVertexIndex");
		writer.WriteValue(value.MinVertexIndex);
		writer.WritePropertyName("MaxVertexIndex");
		writer.WriteValue(value.MaxVertexIndex);
		writer.WritePropertyName("bEnableCollision");
		writer.WriteValue(value.bEnableCollision);
		writer.WritePropertyName("bCastShadow");
		writer.WriteValue(value.bCastShadow);
		writer.WritePropertyName("bForceOpaque");
		writer.WriteValue(value.bForceOpaque);
		writer.WritePropertyName("bVisibleInRayTracing");
		writer.WriteValue(value.bVisibleInRayTracing);
		writer.WriteEndObject();
	}

	public override FStaticMeshSection ReadJson(JsonReader reader, Type objectType, FStaticMeshSection existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}

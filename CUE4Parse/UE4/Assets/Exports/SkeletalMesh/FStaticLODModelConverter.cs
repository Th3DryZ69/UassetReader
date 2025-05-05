using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.SkeletalMesh;

public class FStaticLODModelConverter : JsonConverter<FStaticLODModel>
{
	public override void WriteJson(JsonWriter writer, FStaticLODModel value, JsonSerializer serializer)
	{
		writer.WriteStartObject();
		writer.WritePropertyName("Sections");
		serializer.Serialize(writer, value.Sections);
		writer.WritePropertyName("NumVertices");
		writer.WriteValue(value.NumVertices);
		writer.WritePropertyName("NumTexCoords");
		writer.WriteValue(value.NumTexCoords);
		if (value.MorphTargetVertexInfoBuffers != null)
		{
			writer.WritePropertyName("MorphTargetVertexInfoBuffers");
			serializer.Serialize(writer, value.MorphTargetVertexInfoBuffers);
		}
		writer.WritePropertyName("VertexBufferGPUSkin");
		serializer.Serialize(writer, value.VertexBufferGPUSkin);
		if (value.Chunks.Length != 0)
		{
			writer.WritePropertyName("Chunks");
			serializer.Serialize(writer, value.Chunks);
		}
		if (value.MeshToImportVertexMap.Length != 0)
		{
			writer.WritePropertyName("MaxImportVertex");
			serializer.Serialize(writer, value.MaxImportVertex);
		}
		writer.WriteEndObject();
	}

	public override FStaticLODModel ReadJson(JsonReader reader, Type objectType, FStaticLODModel existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}

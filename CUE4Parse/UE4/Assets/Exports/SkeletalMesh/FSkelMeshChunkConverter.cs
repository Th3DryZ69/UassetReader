using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.SkeletalMesh;

public class FSkelMeshChunkConverter : JsonConverter<FSkelMeshChunk>
{
	public override void WriteJson(JsonWriter writer, FSkelMeshChunk value, JsonSerializer serializer)
	{
		writer.WriteStartObject();
		writer.WritePropertyName("BaseVertexIndex");
		writer.WriteValue(value.BaseVertexIndex);
		writer.WritePropertyName("NumRigidVertices");
		writer.WriteValue(value.NumRigidVertices);
		writer.WritePropertyName("NumSoftVertices");
		writer.WriteValue(value.NumSoftVertices);
		writer.WritePropertyName("MaxBoneInfluences");
		writer.WriteValue(value.MaxBoneInfluences);
		writer.WritePropertyName("HasClothData");
		writer.WriteValue(value.HasClothData);
		writer.WriteEndObject();
	}

	public override FSkelMeshChunk ReadJson(JsonReader reader, Type objectType, FSkelMeshChunk existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}

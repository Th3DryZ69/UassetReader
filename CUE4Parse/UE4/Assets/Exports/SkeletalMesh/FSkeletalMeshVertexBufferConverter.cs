using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.SkeletalMesh;

public class FSkeletalMeshVertexBufferConverter : JsonConverter<FSkeletalMeshVertexBuffer>
{
	public override void WriteJson(JsonWriter writer, FSkeletalMeshVertexBuffer value, JsonSerializer serializer)
	{
		writer.WriteStartObject();
		writer.WritePropertyName("NumTexCoords");
		writer.WriteValue(value.NumTexCoords);
		writer.WritePropertyName("MeshExtension");
		serializer.Serialize(writer, value.MeshExtension);
		writer.WritePropertyName("MeshOrigin");
		serializer.Serialize(writer, value.MeshOrigin);
		writer.WritePropertyName("bUseFullPrecisionUVs");
		writer.WriteValue(value.bUseFullPrecisionUVs);
		writer.WritePropertyName("bExtraBoneInfluences");
		writer.WriteValue(value.bExtraBoneInfluences);
		writer.WriteEndObject();
	}

	public override FSkeletalMeshVertexBuffer ReadJson(JsonReader reader, Type objectType, FSkeletalMeshVertexBuffer existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}

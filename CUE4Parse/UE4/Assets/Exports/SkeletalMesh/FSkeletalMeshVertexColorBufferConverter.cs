using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.SkeletalMesh;

public class FSkeletalMeshVertexColorBufferConverter : JsonConverter<FSkeletalMeshVertexColorBuffer>
{
	public override void WriteJson(JsonWriter writer, FSkeletalMeshVertexColorBuffer value, JsonSerializer serializer)
	{
		serializer.Serialize(writer, value.Data);
	}

	public override FSkeletalMeshVertexColorBuffer ReadJson(JsonReader reader, Type objectType, FSkeletalMeshVertexColorBuffer existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}

using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.Animation;

public class FMeshBoneInfoConverter : JsonConverter<FMeshBoneInfo>
{
	public override void WriteJson(JsonWriter writer, FMeshBoneInfo value, JsonSerializer serializer)
	{
		writer.WriteStartObject();
		writer.WritePropertyName("Name");
		serializer.Serialize(writer, value.Name);
		writer.WritePropertyName("ParentIndex");
		writer.WriteValue(value.ParentIndex);
		writer.WriteEndObject();
	}

	public override FMeshBoneInfo ReadJson(JsonReader reader, Type objectType, FMeshBoneInfo existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}

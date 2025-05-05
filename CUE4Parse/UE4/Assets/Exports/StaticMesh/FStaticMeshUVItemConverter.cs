using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.StaticMesh;

public class FStaticMeshUVItemConverter : JsonConverter<FStaticMeshUVItem>
{
	public override void WriteJson(JsonWriter writer, FStaticMeshUVItem value, JsonSerializer serializer)
	{
		writer.WriteStartObject();
		writer.WritePropertyName("Normal");
		serializer.Serialize(writer, value.Normal);
		writer.WritePropertyName("UV");
		serializer.Serialize(writer, value.UV);
		writer.WriteEndObject();
	}

	public override FStaticMeshUVItem ReadJson(JsonReader reader, Type objectType, FStaticMeshUVItem existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}

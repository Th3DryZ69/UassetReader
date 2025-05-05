using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.Component.StaticMesh;

public class FStaticMeshComponentLODInfoConverter : JsonConverter<FStaticMeshComponentLODInfo>
{
	public override void WriteJson(JsonWriter writer, FStaticMeshComponentLODInfo value, JsonSerializer serializer)
	{
		writer.WriteStartObject();
		writer.WritePropertyName("MapBuildDataId");
		writer.WriteValue(value.MapBuildDataId.ToString());
		if (value.OverrideVertexColors != null)
		{
			writer.WritePropertyName("OverrideVertexColors");
			serializer.Serialize(writer, value.OverrideVertexColors);
		}
		writer.WriteEndObject();
	}

	public override FStaticMeshComponentLODInfo ReadJson(JsonReader reader, Type objectType, FStaticMeshComponentLODInfo existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}

using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.StaticMesh;

public class FStaticMeshRenderDataConverter : JsonConverter<FStaticMeshRenderData>
{
	public override void WriteJson(JsonWriter writer, FStaticMeshRenderData value, JsonSerializer serializer)
	{
		writer.WriteStartObject();
		writer.WritePropertyName("LODs");
		serializer.Serialize(writer, value.LODs);
		if (value.NaniteResources != null)
		{
			writer.WritePropertyName("NaniteResources");
			serializer.Serialize(writer, value.NaniteResources);
		}
		writer.WritePropertyName("Bounds");
		serializer.Serialize(writer, value.Bounds);
		writer.WritePropertyName("bLODsShareStaticLighting");
		writer.WriteValue(value.bLODsShareStaticLighting);
		writer.WritePropertyName("ScreenSize");
		serializer.Serialize(writer, value.ScreenSize);
		writer.WriteEndObject();
	}

	public override FStaticMeshRenderData ReadJson(JsonReader reader, Type objectType, FStaticMeshRenderData existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}

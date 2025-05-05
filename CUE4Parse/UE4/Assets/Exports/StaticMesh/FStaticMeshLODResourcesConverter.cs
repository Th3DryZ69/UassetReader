using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.StaticMesh;

public class FStaticMeshLODResourcesConverter : JsonConverter<FStaticMeshLODResources>
{
	public override void WriteJson(JsonWriter writer, FStaticMeshLODResources value, JsonSerializer serializer)
	{
		writer.WriteStartObject();
		writer.WritePropertyName("Sections");
		serializer.Serialize(writer, value.Sections);
		writer.WritePropertyName("MaxDeviation");
		writer.WriteValue(value.MaxDeviation);
		writer.WritePropertyName("PositionVertexBuffer");
		serializer.Serialize(writer, value.PositionVertexBuffer);
		writer.WritePropertyName("VertexBuffer");
		serializer.Serialize(writer, value.VertexBuffer);
		writer.WritePropertyName("ColorVertexBuffer");
		serializer.Serialize(writer, value.ColorVertexBuffer);
		if (value.CardRepresentationData != null)
		{
			writer.WritePropertyName("CardRepresentationData");
			serializer.Serialize(writer, value.CardRepresentationData);
		}
		writer.WriteEndObject();
	}

	public override FStaticMeshLODResources ReadJson(JsonReader reader, Type objectType, FStaticMeshLODResources existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}

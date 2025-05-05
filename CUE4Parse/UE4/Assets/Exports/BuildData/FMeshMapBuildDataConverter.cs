using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.BuildData;

public class FMeshMapBuildDataConverter : JsonConverter<FMeshMapBuildData>
{
	public override void WriteJson(JsonWriter writer, FMeshMapBuildData value, JsonSerializer serializer)
	{
		writer.WriteStartObject();
		if (value.LightMap != null)
		{
			writer.WritePropertyName("LightMap");
			serializer.Serialize(writer, value.LightMap);
		}
		if (value.ShadowMap != null)
		{
			writer.WritePropertyName("ShadowMap");
			serializer.Serialize(writer, value.ShadowMap);
		}
		writer.WritePropertyName("IrrelevantLights");
		serializer.Serialize(writer, value.IrrelevantLights);
		writer.WritePropertyName("PerInstanceLightmapData");
		serializer.Serialize(writer, value.PerInstanceLightmapData);
		writer.WriteEndObject();
	}

	public override FMeshMapBuildData ReadJson(JsonReader reader, Type objectType, FMeshMapBuildData existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}

using System;
using CUE4Parse.UE4.Objects.UObject;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.BuildData;

public class FLightMap2DConverter : JsonConverter<FLightMap2D>
{
	public override void WriteJson(JsonWriter writer, FLightMap2D value, JsonSerializer serializer)
	{
		writer.WriteStartObject();
		writer.WritePropertyName("Textures");
		serializer.Serialize(writer, value.Textures);
		FPackageIndex? skyOcclusionTexture = value.SkyOcclusionTexture;
		if (skyOcclusionTexture != null && !skyOcclusionTexture.IsNull)
		{
			writer.WritePropertyName("SkyOcclusionTexture");
			serializer.Serialize(writer, value.SkyOcclusionTexture);
		}
		FPackageIndex? aOMaterialMaskTexture = value.AOMaterialMaskTexture;
		if (aOMaterialMaskTexture != null && !aOMaterialMaskTexture.IsNull)
		{
			writer.WritePropertyName("AOMaterialMaskTexture");
			serializer.Serialize(writer, value.AOMaterialMaskTexture);
		}
		FPackageIndex? shadowMapTexture = value.ShadowMapTexture;
		if (shadowMapTexture != null && !shadowMapTexture.IsNull)
		{
			writer.WritePropertyName("ShadowMapTexture");
			serializer.Serialize(writer, value.ShadowMapTexture);
		}
		writer.WritePropertyName("VirtualTextures");
		serializer.Serialize(writer, value.VirtualTextures);
		writer.WritePropertyName("ScaleVectors");
		serializer.Serialize(writer, value.ScaleVectors);
		writer.WritePropertyName("AddVectors");
		serializer.Serialize(writer, value.AddVectors);
		writer.WritePropertyName("CoordinateScale");
		serializer.Serialize(writer, value.CoordinateScale);
		writer.WritePropertyName("CoordinateBias");
		serializer.Serialize(writer, value.CoordinateBias);
		writer.WritePropertyName("InvUniformPenumbraSize");
		serializer.Serialize(writer, value.InvUniformPenumbraSize);
		writer.WritePropertyName("bShadowChannelValid");
		serializer.Serialize(writer, value.bShadowChannelValid);
		writer.WritePropertyName("LightGuids");
		serializer.Serialize(writer, value.LightGuids);
		writer.WriteEndObject();
	}

	public override FLightMap2D ReadJson(JsonReader reader, Type objectType, FLightMap2D existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}

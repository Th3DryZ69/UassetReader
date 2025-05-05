using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.Texture;

public class FTexture2DMipMapConverter : JsonConverter<FTexture2DMipMap>
{
	public override void WriteJson(JsonWriter writer, FTexture2DMipMap value, JsonSerializer serializer)
	{
		writer.WriteStartObject();
		writer.WritePropertyName("BulkData");
		serializer.Serialize(writer, value.BulkData);
		writer.WritePropertyName("SizeX");
		writer.WriteValue(value.SizeX);
		writer.WritePropertyName("SizeY");
		writer.WriteValue(value.SizeY);
		writer.WritePropertyName("SizeZ");
		writer.WriteValue(value.SizeZ);
		writer.WriteEndObject();
	}

	public override FTexture2DMipMap ReadJson(JsonReader reader, Type objectType, FTexture2DMipMap existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}

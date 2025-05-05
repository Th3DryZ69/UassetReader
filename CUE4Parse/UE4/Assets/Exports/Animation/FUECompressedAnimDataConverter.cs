using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.Animation;

public class FUECompressedAnimDataConverter : JsonConverter<FUECompressedAnimData>
{
	public override void WriteJson(JsonWriter writer, FUECompressedAnimData value, JsonSerializer serializer)
	{
		writer.WriteStartObject();
		if (value.CompressedNumberOfFrames > 0)
		{
			writer.WritePropertyName("CompressedNumberOfFrames");
			writer.WriteValue(value.CompressedNumberOfFrames);
		}
		writer.WritePropertyName("KeyEncodingFormat");
		writer.WriteValue(value.KeyEncodingFormat.ToString());
		writer.WritePropertyName("TranslationCompressionFormat");
		writer.WriteValue(value.TranslationCompressionFormat.ToString());
		writer.WritePropertyName("RotationCompressionFormat");
		writer.WriteValue(value.RotationCompressionFormat.ToString());
		writer.WritePropertyName("ScaleCompressionFormat");
		writer.WriteValue(value.ScaleCompressionFormat.ToString());
		writer.WriteEndObject();
	}

	public override FUECompressedAnimData ReadJson(JsonReader reader, Type objectType, FUECompressedAnimData existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}

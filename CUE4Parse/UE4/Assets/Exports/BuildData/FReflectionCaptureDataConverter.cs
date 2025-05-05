using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.BuildData;

public class FReflectionCaptureDataConverter : JsonConverter<FReflectionCaptureData>
{
	public override void WriteJson(JsonWriter writer, FReflectionCaptureData value, JsonSerializer serializer)
	{
		writer.WriteStartObject();
		writer.WritePropertyName("CubemapSize");
		writer.WriteValue(value.CubemapSize);
		writer.WritePropertyName("AverageBrightness");
		writer.WriteValue(value.AverageBrightness);
		writer.WritePropertyName("Brightness");
		writer.WriteValue(value.Brightness);
		if (value.EncodedCaptureData != null)
		{
			writer.WritePropertyName("EncodedCaptureData");
			serializer.Serialize(writer, value.EncodedCaptureData);
		}
		writer.WriteEndObject();
	}

	public override FReflectionCaptureData ReadJson(JsonReader reader, Type objectType, FReflectionCaptureData existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}

using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.Engine.Font;

public class FFontDataConverter : JsonConverter<FFontData>
{
	public override void WriteJson(JsonWriter writer, FFontData value, JsonSerializer serializer)
	{
		writer.WriteStartObject();
		if (value.LocalFontFaceAsset != null)
		{
			writer.WritePropertyName("LocalFontFaceAsset");
			serializer.Serialize(writer, value.LocalFontFaceAsset);
		}
		else
		{
			if (!string.IsNullOrEmpty(value.FontFilename))
			{
				writer.WritePropertyName("FontFilename");
				writer.WriteValue(value.FontFilename);
			}
			writer.WritePropertyName("Hinting");
			writer.WriteValue(value.Hinting);
			writer.WritePropertyName("LoadingPolicy");
			writer.WriteValue(value.LoadingPolicy);
		}
		writer.WritePropertyName("SubFaceIndex");
		writer.WriteValue(value.SubFaceIndex);
		writer.WriteEndObject();
	}

	public override FFontData ReadJson(JsonReader reader, Type objectType, FFontData existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}

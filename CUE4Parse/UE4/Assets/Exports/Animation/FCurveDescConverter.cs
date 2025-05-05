using System;
using CUE4Parse.UE4.Objects.Engine.Curves;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.Animation;

public class FCurveDescConverter : JsonConverter<FCurveDesc>
{
	public override void WriteJson(JsonWriter writer, FCurveDesc value, JsonSerializer serializer)
	{
		writer.WriteStartObject();
		writer.WritePropertyName("CompressionFormat");
		writer.WriteValue(value.CompressionFormat.ToString());
		writer.WritePropertyName("KeyTimeCompressionFormat");
		writer.WriteValue(value.KeyTimeCompressionFormat.ToString());
		writer.WritePropertyName("PreInfinityExtrap");
		writer.WriteValue(value.PreInfinityExtrap.ToString());
		writer.WritePropertyName("PostInfinityExtrap");
		writer.WriteValue(value.PostInfinityExtrap.ToString());
		if (value.CompressionFormat == ERichCurveCompressionFormat.RCCF_Constant)
		{
			writer.WritePropertyName("ConstantValue");
			writer.WriteValue(value.ConstantValue);
		}
		else
		{
			writer.WritePropertyName("NumKeys");
			writer.WriteValue(value.NumKeys);
		}
		writer.WritePropertyName("KeyDataOffset");
		writer.WriteValue(value.KeyDataOffset);
		writer.WriteEndObject();
	}

	public override FCurveDesc ReadJson(JsonReader reader, Type objectType, FCurveDesc existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}

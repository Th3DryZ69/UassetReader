using System;
using CUE4Parse.UE4.Objects.UObject;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Objects.Engine.Curves;

public class FCurveMetaDataConverter : JsonConverter<FCurveMetaData>
{
	public override void WriteJson(JsonWriter writer, FCurveMetaData value, JsonSerializer serializer)
	{
		writer.WriteStartObject();
		writer.WritePropertyName("Type");
		serializer.Serialize(writer, value.Type);
		writer.WritePropertyName("LinkedBones");
		writer.WriteStartArray();
		FName[] linkedBones = value.LinkedBones;
		foreach (FName fName in linkedBones)
		{
			serializer.Serialize(writer, fName);
		}
		writer.WriteEndArray();
		writer.WritePropertyName("MaxLOD");
		writer.WriteValue(value.MaxLOD);
		writer.WriteEndObject();
	}

	public override FCurveMetaData ReadJson(JsonReader reader, Type objectType, FCurveMetaData existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}

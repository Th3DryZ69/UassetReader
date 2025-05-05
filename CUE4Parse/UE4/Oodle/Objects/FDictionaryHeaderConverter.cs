using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Oodle.Objects;

public class FDictionaryHeaderConverter : JsonConverter<FDictionaryHeader>
{
	public override void WriteJson(JsonWriter writer, FDictionaryHeader value, JsonSerializer serializer)
	{
		writer.WriteStartObject();
		writer.WritePropertyName("Magic");
		serializer.Serialize(writer, value.Magic);
		writer.WritePropertyName("DictionaryVersion");
		serializer.Serialize(writer, value.DictionaryVersion);
		writer.WritePropertyName("OodleMajorHeaderVersion");
		serializer.Serialize(writer, value.OodleMajorHeaderVersion);
		writer.WritePropertyName("HashTableSize");
		serializer.Serialize(writer, value.HashTableSize);
		writer.WritePropertyName("DictionaryData");
		serializer.Serialize(writer, value.DictionaryData);
		writer.WritePropertyName("CompressorData");
		serializer.Serialize(writer, value.CompressorData);
		writer.WriteEndObject();
	}

	public override FDictionaryHeader ReadJson(JsonReader reader, Type objectType, FDictionaryHeader existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}

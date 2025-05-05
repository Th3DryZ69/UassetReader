using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Objects.UObject;

public class FSoftObjectPathConverter : JsonConverter<FSoftObjectPath>
{
	public override void WriteJson(JsonWriter writer, FSoftObjectPath value, JsonSerializer serializer)
	{
		writer.WriteStartObject();
		writer.WritePropertyName("AssetPathName");
		serializer.Serialize(writer, value.AssetPathName);
		writer.WritePropertyName("SubPathString");
		writer.WriteValue(value.SubPathString);
		writer.WriteEndObject();
	}

	public override FSoftObjectPath ReadJson(JsonReader reader, Type objectType, FSoftObjectPath existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}

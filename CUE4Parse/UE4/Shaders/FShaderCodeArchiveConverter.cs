using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Shaders;

public class FShaderCodeArchiveConverter : JsonConverter<FShaderCodeArchive>
{
	public override void WriteJson(JsonWriter writer, FShaderCodeArchive value, JsonSerializer serializer)
	{
		writer.WriteStartObject();
		writer.WritePropertyName("SerializedShaders");
		serializer.Serialize(writer, value.SerializedShaders);
		writer.WriteEndObject();
	}

	public override FShaderCodeArchive ReadJson(JsonReader reader, Type objectType, FShaderCodeArchive existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}

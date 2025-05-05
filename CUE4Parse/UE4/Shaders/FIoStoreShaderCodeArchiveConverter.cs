using System;
using CUE4Parse.UE4.Objects.Core.Misc;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Shaders;

public class FIoStoreShaderCodeArchiveConverter : JsonConverter<FIoStoreShaderCodeArchive>
{
	public override void WriteJson(JsonWriter writer, FIoStoreShaderCodeArchive value, JsonSerializer serializer)
	{
		writer.WriteStartObject();
		writer.WritePropertyName("ShaderMapHashes");
		writer.WriteStartArray();
		FSHAHash[] shaderMapHashes = value.ShaderMapHashes;
		for (int i = 0; i < shaderMapHashes.Length; i++)
		{
			FSHAHash fSHAHash = shaderMapHashes[i];
			serializer.Serialize(writer, fSHAHash.Hash);
		}
		writer.WriteEndArray();
		writer.WritePropertyName("ShaderHashes");
		writer.WriteStartArray();
		shaderMapHashes = value.ShaderHashes;
		for (int i = 0; i < shaderMapHashes.Length; i++)
		{
			FSHAHash fSHAHash2 = shaderMapHashes[i];
			serializer.Serialize(writer, fSHAHash2.Hash);
		}
		writer.WriteEndArray();
		writer.WritePropertyName("ShaderGroupIoHashes");
		serializer.Serialize(writer, value.ShaderGroupIoHashes);
		writer.WritePropertyName("ShaderMapEntries");
		serializer.Serialize(writer, value.ShaderMapEntries);
		writer.WritePropertyName("ShaderEntries");
		serializer.Serialize(writer, value.ShaderEntries);
		writer.WritePropertyName("ShaderGroupEntries");
		serializer.Serialize(writer, value.ShaderGroupEntries);
		writer.WritePropertyName("ShaderIndices");
		serializer.Serialize(writer, value.ShaderIndices);
		writer.WriteEndObject();
	}

	public override FIoStoreShaderCodeArchive ReadJson(JsonReader reader, Type objectType, FIoStoreShaderCodeArchive existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}

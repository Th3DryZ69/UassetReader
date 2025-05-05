using System;
using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Readers;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Shaders;

[JsonConverter(typeof(FSerializedShaderArchiveConverter))]
public class FSerializedShaderArchive : FRHIShaderLibrary
{
	public class FSerializedShaderArchiveConverter : JsonConverter<FSerializedShaderArchive>
	{
		public override void WriteJson(JsonWriter writer, FSerializedShaderArchive value, JsonSerializer serializer)
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
			writer.WritePropertyName("ShaderMapEntries");
			serializer.Serialize(writer, value.ShaderMapEntries);
			writer.WritePropertyName("ShaderEntries");
			serializer.Serialize(writer, value.ShaderEntries);
			writer.WritePropertyName("PreloadEntries");
			serializer.Serialize(writer, value.PreloadEntries);
			writer.WritePropertyName("ShaderIndices");
			serializer.Serialize(writer, value.ShaderIndices);
			writer.WriteEndObject();
		}

		public override FSerializedShaderArchive ReadJson(JsonReader reader, Type objectType, FSerializedShaderArchive existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}
	}

	public readonly FSHAHash[] ShaderMapHashes;

	public readonly FSHAHash[] ShaderHashes;

	public readonly FShaderMapEntry[] ShaderMapEntries;

	public readonly FShaderCodeEntry[] ShaderEntries;

	public readonly FFileCachePreloadEntry[] PreloadEntries;

	public readonly uint[] ShaderIndices;

	public FSerializedShaderArchive(FArchive Ar)
	{
		ShaderMapHashes = Ar.ReadArray(() => new FSHAHash(Ar));
		ShaderHashes = Ar.ReadArray(() => new FSHAHash(Ar));
		ShaderMapEntries = Ar.ReadArray<FShaderMapEntry>();
		ShaderEntries = Ar.ReadArray<FShaderCodeEntry>();
		PreloadEntries = Ar.ReadArray<FFileCachePreloadEntry>();
		ShaderIndices = Ar.ReadArray<uint>();
	}
}

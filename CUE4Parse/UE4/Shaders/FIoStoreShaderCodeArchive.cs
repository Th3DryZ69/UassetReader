using CUE4Parse.UE4.IO.Objects;
using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Readers;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Shaders;

[JsonConverter(typeof(FIoStoreShaderCodeArchiveConverter))]
public class FIoStoreShaderCodeArchive : FRHIShaderLibrary
{
	public readonly FSHAHash[] ShaderMapHashes;

	public readonly FSHAHash[] ShaderHashes;

	public readonly FIoChunkId[] ShaderGroupIoHashes;

	public readonly FIoStoreShaderMapEntry[] ShaderMapEntries;

	public readonly FIoStoreShaderCodeEntry[] ShaderEntries;

	public readonly FIoStoreShaderGroupEntry[] ShaderGroupEntries;

	public readonly uint[] ShaderIndices;

	public FIoStoreShaderCodeArchive(FArchive Ar)
	{
		ShaderMapHashes = Ar.ReadArray(() => new FSHAHash(Ar));
		ShaderHashes = Ar.ReadArray(() => new FSHAHash(Ar));
		ShaderGroupIoHashes = Ar.ReadArray<FIoChunkId>();
		ShaderMapEntries = Ar.ReadArray<FIoStoreShaderMapEntry>();
		ShaderEntries = Ar.ReadArray<FIoStoreShaderCodeEntry>();
		ShaderGroupEntries = Ar.ReadArray<FIoStoreShaderGroupEntry>();
		ShaderIndices = Ar.ReadArray<uint>();
	}
}

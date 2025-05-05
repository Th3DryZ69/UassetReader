using CUE4Parse.UE4.Assets.Readers;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Objects.Engine;

[JsonConverter(typeof(FCompressedVisibilityChunkConverter))]
public readonly struct FCompressedVisibilityChunk : IUStruct
{
	public readonly bool bCompressed;

	public readonly int UncompressedSize;

	public readonly byte[] Data;

	public FCompressedVisibilityChunk(FAssetArchive Ar)
	{
		bCompressed = Ar.ReadBoolean();
		UncompressedSize = Ar.Read<int>();
		Data = Ar.ReadBytes(Ar.Read<int>());
	}
}

using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Versions;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.Texture;

[JsonConverter(typeof(FVirtualTextureDataChunkConverter))]
public class FVirtualTextureDataChunk
{
	public readonly FByteBulkData BulkData;

	public readonly uint SizeInBytes;

	public readonly uint CodecPayloadSize;

	public readonly uint[] CodecPayloadOffset;

	public readonly EVirtualTextureCodec[] CodecType;

	public FVirtualTextureDataChunk(FAssetArchive Ar, uint numLayers)
	{
		CodecType = new EVirtualTextureCodec[numLayers];
		CodecPayloadOffset = new uint[numLayers];
		if (Ar.Game >= EGame.GAME_UE5_0)
		{
			Ar.Position += 20L;
		}
		SizeInBytes = Ar.Read<uint>();
		CodecPayloadSize = Ar.Read<uint>();
		for (uint num = 0u; num < numLayers; num++)
		{
			CodecType[num] = Ar.Read<EVirtualTextureCodec>();
			CodecPayloadOffset[num] = ((Ar.Game >= EGame.GAME_UE4_27) ? Ar.Read<uint>() : Ar.Read<ushort>());
		}
		BulkData = new FByteBulkData(Ar);
	}
}

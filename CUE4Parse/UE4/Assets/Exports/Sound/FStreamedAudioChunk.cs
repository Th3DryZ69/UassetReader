using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Exceptions;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.Sound;

[JsonConverter(typeof(FStreamedAudioChunkConverter))]
public class FStreamedAudioChunk
{
	public readonly int DataSize;

	public readonly int AudioDataSize;

	public readonly FByteBulkData BulkData;

	public FStreamedAudioChunk(FAssetArchive Ar)
	{
		if (Ar.ReadBoolean())
		{
			BulkData = new FByteBulkData(Ar);
			DataSize = Ar.Read<int>();
			AudioDataSize = Ar.Read<int>();
			return;
		}
		Ar.Position -= 4L;
		throw new ParserException(Ar, "StreamedAudioChunks must be cooked");
	}
}

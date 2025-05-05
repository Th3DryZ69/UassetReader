using CUE4Parse.ACL;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.Animation.ACL;

[JsonConverter(typeof(FACLCompressedAnimDataConverter))]
public class FACLCompressedAnimData : ICompressedAnimData
{
	public byte[] CompressedByteStream;

	public int CompressedNumberOfFrames { get; set; }

	public CompressedTracks GetCompressedTracks()
	{
		return new CompressedTracks(CompressedByteStream);
	}

	public void Bind(byte[] bulkData)
	{
		CompressedByteStream = bulkData;
	}
}

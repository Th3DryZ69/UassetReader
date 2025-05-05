using System;
using CUE4Parse.UE4.Assets.Readers;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.Animation.ACL;

[JsonConverter(typeof(FACLDatabaseCompressedAnimDataConverter))]
public class FACLDatabaseCompressedAnimData : ICompressedAnimData
{
	public byte[] CompressedByteStream;

	public UAnimBoneCompressionCodec_ACLDatabase? Codec;

	public uint SequenceNameHash;

	public int CompressedNumberOfFrames { get; set; }

	public void SerializeCompressedData(FAssetArchive Ar)
	{
		((ICompressedAnimData)this).BaseSerializeCompressedData(Ar);
		SequenceNameHash = Ar.Read<uint>();
	}

	public void Bind(byte[] bulkData)
	{
		throw new NotImplementedException();
	}
}

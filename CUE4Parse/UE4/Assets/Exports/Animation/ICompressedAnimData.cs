using CUE4Parse.UE4.Assets.Readers;

namespace CUE4Parse.UE4.Assets.Exports.Animation;

public interface ICompressedAnimData
{
	int CompressedNumberOfFrames { get; set; }

	void SerializeCompressedData(FAssetArchive Ar)
	{
		BaseSerializeCompressedData(Ar);
	}

	internal void BaseSerializeCompressedData(FAssetArchive Ar)
	{
		CompressedNumberOfFrames = Ar.Read<int>();
	}

	void Bind(byte[] bulkData);
}

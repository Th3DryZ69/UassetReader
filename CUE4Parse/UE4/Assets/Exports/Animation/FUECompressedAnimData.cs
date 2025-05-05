using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.Animation;

[JsonConverter(typeof(FUECompressedAnimDataConverter))]
public class FUECompressedAnimData : FCompressedAnimDataBase, ICompressedAnimData
{
	public int CompressedNumberOfFrames { get; set; }

	public void InitViewsFromBuffer(byte[] bulkData)
	{
		using FByteArchive fByteArchive = new FByteArchive("SerializedByteStream", bulkData);
		fByteArchive.ReadArray(CompressedTrackOffsets);
		fByteArchive.ReadArray(CompressedScaleOffsets.OffsetData);
		fByteArchive.ReadArray(CompressedByteStream);
	}

	public void SerializeCompressedData(FAssetArchive Ar)
	{
		bool num = Ar.Game >= EGame.GAME_UE4_25;
		if (num)
		{
			((ICompressedAnimData)this).BaseSerializeCompressedData(Ar);
		}
		KeyEncodingFormat = Ar.Read<AnimationKeyFormat>();
		TranslationCompressionFormat = Ar.Read<AnimationCompressionFormat>();
		RotationCompressionFormat = Ar.Read<AnimationCompressionFormat>();
		ScaleCompressionFormat = Ar.Read<AnimationCompressionFormat>();
		if (!num)
		{
			((ICompressedAnimData)this).BaseSerializeCompressedData(Ar);
		}
		if (num)
		{
			CompressedByteStream = new byte[Ar.Read<int>()];
		}
		CompressedTrackOffsets = new int[Ar.Read<int>()];
		CompressedScaleOffsets.OffsetData = new int[Ar.Read<int>()];
		CompressedScaleOffsets.StripSize = Ar.Read<int>();
		if (!num)
		{
			CompressedByteStream = new byte[Ar.Read<int>()];
		}
	}

	public void Bind(byte[] bulkData)
	{
		InitViewsFromBuffer(bulkData);
	}

	public override string ToString()
	{
		return $"[{TranslationCompressionFormat}, {RotationCompressionFormat}, {ScaleCompressionFormat}]";
	}
}

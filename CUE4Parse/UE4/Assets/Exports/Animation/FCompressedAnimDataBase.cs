namespace CUE4Parse.UE4.Assets.Exports.Animation;

public class FCompressedAnimDataBase
{
	public int[] CompressedTrackOffsets;

	public FCompressedOffsetData CompressedScaleOffsets = new FCompressedOffsetData();

	public byte[] CompressedByteStream;

	public AnimationKeyFormat KeyEncodingFormat;

	public AnimationCompressionFormat TranslationCompressionFormat;

	public AnimationCompressionFormat RotationCompressionFormat;

	public AnimationCompressionFormat ScaleCompressionFormat;
}

namespace CUE4Parse.UE4.Assets.Objects;

public readonly struct FCompressedChunk
{
	public readonly int UncompressedOffset;

	public readonly int UncompressedSize;

	public readonly int CompressedOffset;

	public readonly int CompressedSize;
}

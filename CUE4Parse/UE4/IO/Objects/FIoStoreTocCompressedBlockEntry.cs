using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.IO.Objects;

public readonly struct FIoStoreTocCompressedBlockEntry
{
	private const int OffsetBits = 40;

	private const ulong OffsetMask = 1099511627775uL;

	private const int SizeBits = 24;

	private const uint SizeMask = 16777215u;

	private const int SizeShift = 8;

	public readonly long Offset;

	public readonly uint CompressedSize;

	public readonly uint UncompressedSize;

	public readonly byte CompressionMethodIndex;

	public readonly long Position;

	public readonly byte[] Buffer;

	public unsafe FIoStoreTocCompressedBlockEntry(FArchive Ar)
	{
		Position = Ar.Position;
		Buffer = Ar.ReadBytes(12);
		Ar.Position = Position;
		byte* ptr = stackalloc byte[12];
		Ar.Serialize(ptr, 12);
		Offset = *(long*)ptr & 0xFFFFFFFFFFL;
		CompressedSize = (((uint*)ptr)[1] >> 8) & 0xFFFFFF;
		UncompressedSize = ((uint*)ptr)[2] & 0xFFFFFF;
		CompressionMethodIndex = (byte)(((uint*)ptr)[2] >> 24);
	}

	public override string ToString()
	{
		return $"{"Offset"} {Offset}: From {CompressedSize} To {UncompressedSize}";
	}
}

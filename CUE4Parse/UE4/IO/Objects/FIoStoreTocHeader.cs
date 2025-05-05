using System.Linq;
using CUE4Parse.UE4.Exceptions;
using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Readers;
using CUE4Parse.Utils;

namespace CUE4Parse.UE4.IO.Objects;

public class FIoStoreTocHeader
{
	public const int SIZE = 144;

	public static byte[] TOC_MAGIC = new byte[16]
	{
		45, 61, 61, 45, 45, 61, 61, 45, 45, 61,
		61, 45, 45, 61, 61, 45
	};

	public readonly byte[] TocMagic;

	public readonly EIoStoreTocVersion Version;

	private readonly byte _reserved0;

	private readonly ushort _reserved1;

	public readonly uint TocHeaderSize;

	public readonly uint TocEntryCount;

	public readonly uint TocCompressedBlockEntryCount;

	public readonly uint TocCompressedBlockEntrySize;

	public readonly uint CompressionMethodNameCount;

	public readonly uint CompressionMethodNameLength;

	public readonly uint CompressionBlockSize;

	public readonly uint DirectoryIndexSize;

	public uint PartitionCount;

	public readonly FIoContainerId ContainerId;

	public readonly FGuid EncryptionKeyGuid;

	public readonly EIoContainerFlags ContainerFlags;

	private readonly byte _reserved3;

	private readonly ushort _reserved4;

	public readonly uint TocChunkPerfectHashSeedsCount;

	public ulong PartitionSize;

	public readonly uint TocChunksWithoutPerfectHashCount;

	private readonly uint _reserved7;

	private readonly ulong[] _reserved8;

	public FIoStoreTocHeader(FArchive Ar)
	{
		TocMagic = Ar.ReadBytes(16);
		if (!TOC_MAGIC.SequenceEqual(TocMagic))
		{
			throw new ParserException(Ar, "Invalid utoc magic");
		}
		Version = Ar.Read<EIoStoreTocVersion>();
		_reserved0 = Ar.Read<byte>();
		_reserved1 = Ar.Read<ushort>();
		TocHeaderSize = Ar.Read<uint>();
		TocEntryCount = Ar.Read<uint>();
		TocCompressedBlockEntryCount = Ar.Read<uint>();
		TocCompressedBlockEntrySize = Ar.Read<uint>();
		CompressionMethodNameCount = Ar.Read<uint>();
		CompressionMethodNameLength = Ar.Read<uint>();
		CompressionBlockSize = Ar.Read<uint>();
		DirectoryIndexSize = Ar.Read<uint>();
		PartitionCount = Ar.Read<uint>();
		ContainerId = Ar.Read<FIoContainerId>();
		EncryptionKeyGuid = Ar.Read<FGuid>();
		ContainerFlags = Ar.Read<EIoContainerFlags>();
		_reserved3 = Ar.Read<byte>();
		_reserved4 = Ar.Read<ushort>();
		TocChunkPerfectHashSeedsCount = Ar.Read<uint>();
		PartitionSize = Ar.Read<ulong>();
		TocChunksWithoutPerfectHashCount = Ar.Read<uint>();
		_reserved7 = Ar.Read<uint>();
		_reserved8 = Ar.ReadArray<ulong>(5);
		Ar.Position = Ar.Position.Align(4);
	}
}

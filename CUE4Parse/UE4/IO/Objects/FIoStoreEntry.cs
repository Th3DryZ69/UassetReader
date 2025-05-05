using System.Runtime.CompilerServices;
using CUE4Parse.Compression;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.VirtualFileSystem;

namespace CUE4Parse.UE4.IO.Objects;

public class FIoStoreEntry : VfsEntry
{
	public readonly uint TocEntryIndex;

	public override bool IsEncrypted => IoStoreReader.IsEncrypted;

	public override CompressionMethod CompressionMethod
	{
		get
		{
			FIoStoreTocResource tocResource = IoStoreReader.TocResource;
			int num = (int)(base.Offset / tocResource.Header.CompressionBlockSize);
			return tocResource.CompressionMethods[tocResource.CompressionBlocks[num].CompressionMethodIndex];
		}
	}

	public FIoChunkId ChunkId => IoStoreReader.TocResource.ChunkIds[TocEntryIndex];

	public IoStoreReader IoStoreReader
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return (IoStoreReader)Vfs;
		}
	}

	public FIoStoreEntry(IoStoreReader reader, string path, uint tocEntryIndex)
		: base(reader)
	{
		base.Path = path;
		TocEntryIndex = tocEntryIndex;
		ref FIoOffsetAndLength reference = ref reader.TocResource.ChunkOffsetLengths[tocEntryIndex];
		base.Offset = (long)reference.Offset;
		base.Size = (long)reference.Length;
	}

	public override byte[] Read()
	{
		return Vfs.Extract(this);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override FArchive CreateReader()
	{
		return new FByteArchive(base.Path, Read(), Vfs.Versions);
	}
}

using System;
using CUE4Parse.Compression;
using CUE4Parse.UE4.Readers;
using Serilog;

namespace CUE4Parse.UE4.IO.Objects;

public class FIoStoreTocResource
{
	public readonly FIoStoreTocHeader Header;

	public readonly FIoChunkId[] ChunkIds;

	public readonly FIoOffsetAndLength[] ChunkOffsetLengths;

	public readonly int[]? ChunkPerfectHashSeeds;

	public readonly int[]? ChunkIndicesWithoutPerfectHash;

	public readonly FIoStoreTocCompressedBlockEntry[] CompressionBlocks;

	public readonly CompressionMethod[] CompressionMethods;

	public readonly byte[]? DirectoryIndexBuffer;

	public readonly FIoStoreTocEntryMeta[]? ChunkMetas;

	public unsafe FIoStoreTocResource(FArchive Ar, EIoStoreTocReadOptions readOptions = EIoStoreTocReadOptions.Default)
	{
		byte[] array = new byte[Ar.Length];
		Ar.Read(array, 0, array.Length);
		using FByteArchive fByteArchive = new FByteArchive(Ar.Name, array);
		Header = new FIoStoreTocHeader(fByteArchive);
		if ((int)Header.Version < 3)
		{
			Header.PartitionCount = 1u;
			Header.PartitionSize = ulong.MaxValue;
		}
		ChunkIds = fByteArchive.ReadArray<FIoChunkId>((int)Header.TocEntryCount);
		ChunkOffsetLengths = new FIoOffsetAndLength[Header.TocEntryCount];
		for (int i = 0; i < Header.TocEntryCount; i++)
		{
			ChunkOffsetLengths[i] = new FIoOffsetAndLength(fByteArchive);
		}
		uint num = 0u;
		uint num2 = 0u;
		if ((int)Header.Version >= 5)
		{
			num = Header.TocChunkPerfectHashSeedsCount;
			num2 = Header.TocChunksWithoutPerfectHashCount;
		}
		else if ((int)Header.Version >= 4)
		{
			num = Header.TocChunkPerfectHashSeedsCount;
		}
		if (num != 0)
		{
			ChunkPerfectHashSeeds = fByteArchive.ReadArray<int>((int)num);
		}
		if (num2 != 0)
		{
			ChunkIndicesWithoutPerfectHash = fByteArchive.ReadArray<int>((int)num2);
		}
		CompressionBlocks = new FIoStoreTocCompressedBlockEntry[Header.TocCompressedBlockEntryCount];
		for (int j = 0; j < Header.TocCompressedBlockEntryCount; j++)
		{
			CompressionBlocks[j] = new FIoStoreTocCompressedBlockEntry(fByteArchive);
		}
		int num3 = (int)(Header.CompressionMethodNameLength * Header.CompressionMethodNameCount);
		byte* ptr = stackalloc byte[(int)(uint)num3];
		fByteArchive.Serialize(ptr, num3);
		CompressionMethods = new CompressionMethod[Header.CompressionMethodNameCount + 1];
		CompressionMethods[0] = CompressionMethod.None;
		for (int k = 0; k < Header.CompressionMethodNameCount; k++)
		{
			string text = new string((sbyte*)(ptr + k * Header.CompressionMethodNameLength), 0, (int)Header.CompressionMethodNameLength).TrimEnd('\0');
			if (!string.IsNullOrEmpty(text))
			{
				if (!Enum.TryParse<CompressionMethod>(text, ignoreCase: true, out var result))
				{
					Log.Warning("Unknown compression method '" + text + "' in " + Ar.Name);
					result = CompressionMethod.Unknown;
				}
				CompressionMethods[k + 1] = result;
			}
		}
		if (Header.ContainerFlags.HasFlag(EIoContainerFlags.Signed))
		{
			int num4 = fByteArchive.Read<int>();
			fByteArchive.Position += num4 + num4 + 20 * Header.TocCompressedBlockEntryCount;
		}
		if ((int)Header.Version >= 2 && Header.ContainerFlags.HasFlag(EIoContainerFlags.Indexed) && Header.DirectoryIndexSize != 0)
		{
			if (readOptions.HasFlag(EIoStoreTocReadOptions.ReadDirectoryIndex))
			{
				DirectoryIndexBuffer = fByteArchive.ReadBytes((int)Header.DirectoryIndexSize);
			}
			else
			{
				fByteArchive.Position += Header.DirectoryIndexSize;
			}
		}
		if (readOptions.HasFlag(EIoStoreTocReadOptions.ReadTocMeta))
		{
			ChunkMetas = new FIoStoreTocEntryMeta[Header.TocEntryCount];
			for (int l = 0; l < Header.TocEntryCount; l++)
			{
				ChunkMetas[l] = new FIoStoreTocEntryMeta(fByteArchive);
			}
		}
	}
}

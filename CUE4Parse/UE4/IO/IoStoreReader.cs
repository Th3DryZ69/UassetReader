using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using CUE4Parse.Compression;
using CUE4Parse.FileProvider.Objects;
using CUE4Parse.UE4.Exceptions;
using CUE4Parse.UE4.IO.Objects;
using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;
using CUE4Parse.UE4.VirtualFileSystem;
using CUE4Parse.Utils;
using Galaxy_Swapper_v2.Workspace.CUE4Parse;

namespace CUE4Parse.UE4.IO;

public class IoStoreReader : AbstractAesVfsReader
{
	public readonly IReadOnlyList<FArchive> ContainerStreams;

	public readonly FIoStoreTocResource TocResource;

	public readonly Dictionary<FIoChunkId, FIoOffsetAndLength>? TocImperfectHashMapFallback;

	public readonly FIoStoreTocHeader Info;

	public FIoContainerHeader? ContainerHeader { get; private set; }

	public override string MountPoint { get; protected set; }

	public override FGuid EncryptionKeyGuid => Info.EncryptionKeyGuid;

	public sealed override long Length { get; set; }

	public override bool IsEncrypted => Info.ContainerFlags.HasFlag(EIoContainerFlags.Encrypted);

	public override bool HasDirectoryIndex => TocResource.DirectoryIndexBuffer != null;

	public IoStoreReader(string tocPath, EIoStoreTocReadOptions readOptions = EIoStoreTocReadOptions.ReadDirectoryIndex, VersionContainer? versions = null)
		: this(new FileInfo(tocPath), readOptions, versions)
	{
	}

	public IoStoreReader(FileInfo utocFile, EIoStoreTocReadOptions readOptions = EIoStoreTocReadOptions.ReadDirectoryIndex, VersionContainer? versions = null)
		: this(new FByteArchive(utocFile.FullName, File.ReadAllBytes(utocFile.FullName), versions), (string it) => new FStreamArchive(it, File.Open(it, FileMode.Open, FileAccess.Read, FileShare.ReadWrite), versions), readOptions)
	{
	}

	public IoStoreReader(string tocPath, Stream tocStream, Stream casStream, EIoStoreTocReadOptions readOptions = EIoStoreTocReadOptions.ReadDirectoryIndex, VersionContainer? versions = null)
		: this(new FStreamArchive(tocPath, tocStream, versions), (string it) => new FStreamArchive(it, casStream, versions), readOptions)
	{
	}

	public IoStoreReader(string tocPath, Stream tocStream, Func<string, FArchive> openContainerStreamFunc, EIoStoreTocReadOptions readOptions = EIoStoreTocReadOptions.ReadDirectoryIndex, VersionContainer? versions = null)
		: this(new FStreamArchive(tocPath, tocStream, versions), openContainerStreamFunc, readOptions)
	{
	}

	public IoStoreReader(FArchive tocStream, Func<string, FArchive> openContainerStreamFunc, EIoStoreTocReadOptions readOptions = EIoStoreTocReadOptions.ReadDirectoryIndex)
		: base(tocStream.Name, tocStream.Versions)
	{
		Length = tocStream.Length;
		TocResource = new FIoStoreTocResource(tocStream, readOptions);
		List<FArchive> list;
		if (TocResource.Header.PartitionCount <= 1)
		{
			list = new List<FArchive>(1);
			try
			{
				list.Add(openContainerStreamFunc(tocStream.Name.SubstringBeforeLast('.') + ".ucas"));
			}
			catch (Exception innerException)
			{
				throw new FIoStatusException(EIoErrorCode.FileOpenFailed, "Failed to open container partition 0 for " + tocStream.Name, innerException);
			}
		}
		else
		{
			list = new List<FArchive>((int)TocResource.Header.PartitionCount);
			string text = tocStream.Name.SubstringBeforeLast('.');
			for (int i = 0; i < TocResource.Header.PartitionCount; i++)
			{
				try
				{
					string arg = ((i > 0) ? (text + "_s" + i + ".ucas") : (text + ".ucas"));
					list.Add(openContainerStreamFunc(arg));
				}
				catch (Exception innerException2)
				{
					throw new FIoStatusException(EIoErrorCode.FileOpenFailed, $"Failed to open container partition {i} for {tocStream.Name}", innerException2);
				}
			}
		}
		Length += list.Sum((FArchive x) => x.Length);
		ContainerStreams = list;
		if (TocResource.ChunkPerfectHashSeeds != null)
		{
			TocImperfectHashMapFallback = new Dictionary<FIoChunkId, FIoOffsetAndLength>();
			if (TocResource.ChunkIndicesWithoutPerfectHash != null)
			{
				int[] chunkIndicesWithoutPerfectHash = TocResource.ChunkIndicesWithoutPerfectHash;
				foreach (int num2 in chunkIndicesWithoutPerfectHash)
				{
					TocImperfectHashMapFallback[TocResource.ChunkIds[num2]] = TocResource.ChunkOffsetLengths[num2];
				}
			}
		}
		Info = TocResource.Header;
		if ((int)TocResource.Header.Version > 5)
		{
			AbstractVfsReader.log.Warning("Io Store \"{0}\" has unsupported version {1}", base.Path, (int)Info.Version);
		}
	}

	public override byte[] Extract(VfsEntry entry)
	{
		if (!(entry is FIoStoreEntry fIoStoreEntry) || entry.Vfs != this)
		{
			throw new ArgumentException("Wrong io store reader, required " + entry.Vfs.Path + ", this is " + base.Path);
		}
		byte[] result = Read(fIoStoreEntry.Offset, fIoStoreEntry.Size);
		CProvider.Export.ChunkOffsetLengths = TocResource.ChunkOffsetLengths[fIoStoreEntry.TocEntryIndex];
		return result;
	}

	public bool DoesChunkExist(FIoChunkId chunkId)
	{
		FIoOffsetAndLength outOffsetLength;
		return TryResolve(chunkId, out outOffsetLength);
	}

	public bool TryResolve(FIoChunkId chunkId, out FIoOffsetAndLength outOffsetLength)
	{
		if (TocResource.ChunkPerfectHashSeeds != null)
		{
			uint tocEntryCount = TocResource.Header.TocEntryCount;
			if (tocEntryCount == 0)
			{
				outOffsetLength = default(FIoOffsetAndLength);
				return false;
			}
			uint num = (uint)TocResource.ChunkPerfectHashSeeds.Length;
			uint num2 = (uint)(chunkId.HashWithSeed(0) % num);
			int num3 = TocResource.ChunkPerfectHashSeeds[num2];
			if (num3 == 0)
			{
				outOffsetLength = default(FIoOffsetAndLength);
				return false;
			}
			uint num5;
			if (num3 < 0)
			{
				uint num4 = (uint)(-num3 - 1);
				if (num4 >= tocEntryCount)
				{
					return TryResolveImperfect(chunkId, out outOffsetLength);
				}
				num5 = num4;
			}
			else
			{
				num5 = (uint)(chunkId.HashWithSeed(num3) % tocEntryCount);
			}
			if (TocResource.ChunkIds[num5].GetHashCode() == chunkId.GetHashCode())
			{
				outOffsetLength = TocResource.ChunkOffsetLengths[num5];
				return true;
			}
			outOffsetLength = default(FIoOffsetAndLength);
			return false;
		}
		return TryResolveImperfect(chunkId, out outOffsetLength);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private bool TryResolveImperfect(FIoChunkId chunkId, out FIoOffsetAndLength outOffsetLength)
	{
		if (TocImperfectHashMapFallback != null)
		{
			return TocImperfectHashMapFallback.TryGetValue(chunkId, out outOffsetLength);
		}
		int num = Array.IndexOf(TocResource.ChunkIds, chunkId);
		if (num == -1)
		{
			outOffsetLength = default(FIoOffsetAndLength);
			return false;
		}
		outOffsetLength = TocResource.ChunkOffsetLengths[num];
		return true;
	}

	public byte[] Read(FIoChunkId chunkId)
	{
		if (TryResolve(chunkId, out var outOffsetLength))
		{
			return Read((long)outOffsetLength.Offset, (long)outOffsetLength.Length);
		}
		throw new KeyNotFoundException($"Couldn't find chunk {chunkId} in IoStore {base.Name}");
	}

	private byte[] Read(long offset, long length)
	{
		uint compressionBlockSize = TocResource.Header.CompressionBlockSize;
		byte[] array = new byte[length];
		int num = (int)(offset / compressionBlockSize);
		int num2 = (int)(((offset + array.Length).Align((int)compressionBlockSize) - 1) / compressionBlockSize);
		long num3 = offset % compressionBlockSize;
		long num4 = length;
		int num5 = 0;
		byte[] array2 = Array.Empty<byte>();
		byte[] array3 = Array.Empty<byte>();
		FArchive[] array4 = new FArchive[ContainerStreams.Count];
		for (int i = num; i <= num2; i++)
		{
			ref FIoStoreTocCompressedBlockEntry reference = ref TocResource.CompressionBlocks[i];
			long num6 = reference.CompressedSize.Align(16);
			if (array2.Length < num6)
			{
				array2 = new byte[num6];
			}
			uint uncompressedSize = reference.UncompressedSize;
			if (array3.Length < uncompressedSize)
			{
				array3 = new byte[uncompressedSize];
			}
			int num7 = (int)((ulong)reference.Offset / TocResource.Header.PartitionSize);
			long num8 = (long)((ulong)reference.Offset % TocResource.Header.PartitionSize);
			FArchive fArchive;
			if (base.IsConcurrent)
			{
				ref FArchive reference2 = ref array4[num7];
				if (reference2 == null)
				{
					reference2 = (FArchive)ContainerStreams[num7].Clone();
				}
				fArchive = reference2;
			}
			else
			{
				fArchive = ContainerStreams[num7];
			}
			fArchive.Position = num8;
			fArchive.Read(array2, 0, (int)num6);
			array2 = DecryptIfEncrypted(array2, 0, (int)num6);
			byte[] array5;
			if (reference.CompressionMethodIndex == 0)
			{
				array5 = array2;
			}
			else
			{
				CompressionMethod method = TocResource.CompressionMethods[reference.CompressionMethodIndex];
				CUE4Parse.Compression.Compression.Decompress(array2, 0, (int)num6, array3, 0, (int)uncompressedSize, method, fArchive);
				array5 = array3;
			}
			if (CProvider.SaveExport && CProvider.Export == null)
			{
				CProvider.Export = new CProvider.ExportData
				{
					Buffer = array5,
					CompressedBuffer = array2,
					CompressionBlock = reference,
					Ucas = System.IO.Path.GetFileNameWithoutExtension(fArchive.Name),
					Utoc = System.IO.Path.GetFileNameWithoutExtension(base.Name),
					Offset = num8
				};
			}
			int num9 = (int)Math.Min(compressionBlockSize - num3, num4);
			Buffer.BlockCopy(array5, (int)num3, array, num5, num9);
			num3 = 0L;
			num4 -= num9;
			num5 += num9;
			fArchive.Position = 0L;
		}
		return array;
	}

	public override IReadOnlyDictionary<string, GameFile> Mount(bool caseInsensitive = false)
	{
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();
		ProcessIndex(caseInsensitive);
		if (base.Game >= EGame.GAME_UE5_0)
		{
			ContainerHeader = ReadContainerHeader();
		}
		if (Globals.LogVfsMounts)
		{
			TimeSpan elapsed = stopwatch.Elapsed;
			StringBuilder stringBuilder = new StringBuilder($"IoStore \"{base.Name}\": {FileCount} files");
			StringBuilder stringBuilder2;
			StringBuilder.AppendInterpolatedStringHandler handler;
			if (base.EncryptedFileCount > 0)
			{
				stringBuilder2 = stringBuilder;
				StringBuilder stringBuilder3 = stringBuilder2;
				handler = new StringBuilder.AppendInterpolatedStringHandler(13, 1, stringBuilder2);
				handler.AppendLiteral(" (");
				handler.AppendFormatted(base.EncryptedFileCount);
				handler.AppendLiteral(" encrypted)");
				stringBuilder3.Append(ref handler);
			}
			if (MountPoint.Contains("/"))
			{
				stringBuilder2 = stringBuilder;
				StringBuilder stringBuilder4 = stringBuilder2;
				handler = new StringBuilder.AppendInterpolatedStringHandler(17, 1, stringBuilder2);
				handler.AppendLiteral(", mount point: \"");
				handler.AppendFormatted(MountPoint);
				handler.AppendLiteral("\"");
				stringBuilder4.Append(ref handler);
			}
			stringBuilder2 = stringBuilder;
			StringBuilder stringBuilder5 = stringBuilder2;
			handler = new StringBuilder.AppendInterpolatedStringHandler(14, 2, stringBuilder2);
			handler.AppendLiteral(", version ");
			handler.AppendFormatted((int)Info.Version);
			handler.AppendLiteral(" in ");
			handler.AppendFormatted(elapsed);
			stringBuilder5.Append(ref handler);
			AbstractVfsReader.log.Information(stringBuilder.ToString());
		}
		return base.Files;
	}

	private void ProcessIndex(bool caseInsensitive)
	{
		if (!HasDirectoryIndex || TocResource.DirectoryIndexBuffer == null)
		{
			throw new ParserException("No directory index");
		}
		FByteArchive fByteArchive = new FByteArchive(base.Path, DecryptIfEncrypted(TocResource.DirectoryIndexBuffer));
		string mountPoint;
		try
		{
			mountPoint = fByteArchive.ReadFString();
		}
		catch (Exception innerException)
		{
			throw new InvalidAesKeyException($"Given aes key '{base.AesKey?.KeyString}'is not working with '{base.Path}'", innerException);
		}
		ValidateMountPoint(ref mountPoint);
		MountPoint = mountPoint;
		FIoDirectoryIndexEntry[] directoryEntries = fByteArchive.ReadArray<FIoDirectoryIndexEntry>();
		FIoFileIndexEntry[] fileEntries = fByteArchive.ReadArray<FIoFileIndexEntry>();
		string[] stringTable = fByteArchive.ReadArray(fByteArchive.ReadFString);
		Dictionary<string, GameFile> files = new Dictionary<string, GameFile>(fileEntries.Length);
		ReadIndex(MountPoint, 0u);
		base.Files = files;
		void ReadIndex(string directoryName, uint dir)
		{
			while (dir != uint.MaxValue)
			{
				ref FIoDirectoryIndexEntry reference = ref directoryEntries[dir];
				string text = ((reference.Name == uint.MaxValue) ? directoryName : (directoryName + stringTable[reference.Name] + "/"));
				uint num = reference.FirstFileEntry;
				while (num != uint.MaxValue)
				{
					ref FIoFileIndexEntry reference2 = ref fileEntries[num];
					string text2 = text + stringTable[reference2.Name];
					FIoStoreEntry fIoStoreEntry = new FIoStoreEntry(this, text2, reference2.UserData);
					if (fIoStoreEntry.IsEncrypted)
					{
						base.EncryptedFileCount++;
					}
					if (caseInsensitive)
					{
						files[text2.ToLowerInvariant()] = fIoStoreEntry;
					}
					else
					{
						files[text2] = fIoStoreEntry;
					}
					num = reference2.NextFileEntry;
				}
				ReadIndex(text, reference.FirstChildEntry);
				dir = reference.NextSiblingEntry;
			}
		}
	}

	private FIoContainerHeader ReadContainerHeader()
	{
		FIoChunkId chunkId = new FIoChunkId(TocResource.Header.ContainerId.Id, 0, (byte)((base.Game >= EGame.GAME_UE5_0) ? 6 : 10));
		return new FIoContainerHeader(new FByteArchive("ContainerHeader", Read(chunkId), base.Versions));
	}

	public override byte[] MountPointCheckBytes()
	{
		return TocResource.DirectoryIndexBuffer ?? new byte[128];
	}

	protected override byte[] ReadAndDecrypt(int length)
	{
		throw new InvalidOperationException("Io Store can't read bytes without context");
	}

	public override void Dispose()
	{
		foreach (FArchive containerStream in ContainerStreams)
		{
			containerStream.Dispose();
		}
	}
}

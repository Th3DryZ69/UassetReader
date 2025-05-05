using System;
using System.Runtime.CompilerServices;
using CUE4Parse.Compression;
using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;
using CUE4Parse.UE4.VirtualFileSystem;
using CUE4Parse.Utils;

namespace CUE4Parse.UE4.Pak.Objects;

public class FPakEntry : VfsEntry
{
	private const byte Flag_None = 0;

	private const byte Flag_Encrypted = 1;

	private const byte Flag_Deleted = 2;

	public readonly long CompressedSize;

	public readonly long UncompressedSize;

	public readonly FPakCompressedBlock[] CompressionBlocks = Array.Empty<FPakCompressedBlock>();

	public readonly uint Flags;

	public readonly uint CompressionBlockSize;

	public readonly int StructSize;

	public sealed override CompressionMethod CompressionMethod { get; }

	public override bool IsEncrypted => (Flags & 1) == 1;

	public bool IsDeleted => (Flags & 2) == 2;

	public bool IsCompressed
	{
		get
		{
			if (UncompressedSize != CompressedSize)
			{
				return CompressionBlockSize != 0;
			}
			return false;
		}
	}

	public PakFileReader PakFileReader
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return (PakFileReader)Vfs;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public FPakEntry(PakFileReader reader, string path, FArchive Ar)
		: base(reader)
	{
		base.Path = path;
		long position = Ar.Position;
		base.Offset = Ar.Read<long>();
		if (Ar.Game == EGame.GAME_GearsOfWar4)
		{
			CompressedSize = Ar.Read<int>();
			UncompressedSize = Ar.Read<int>();
			CompressionMethod = (CompressionMethod)Ar.Read<byte>();
			if (reader.Info.Version < EPakFileVersion.PakFile_Version_NoTimestamps)
			{
				Ar.Position += 8L;
			}
			if (reader.Info.Version >= EPakFileVersion.PakFile_Version_CompressionEncryption)
			{
				if (CompressionMethod != CompressionMethod.None)
				{
					CompressionBlocks = Ar.ReadArray<FPakCompressedBlock>();
				}
				CompressionBlockSize = Ar.Read<uint>();
				if (CompressionMethod == CompressionMethod.Oodle)
				{
					CompressionMethod = CompressionMethod.LZ4;
				}
			}
		}
		else
		{
			CompressedSize = Ar.Read<long>();
			UncompressedSize = Ar.Read<long>();
			base.Size = UncompressedSize;
			if (reader.Info.Version < EPakFileVersion.PakFile_Version_FNameBasedCompressionMethod)
			{
				ECompressionFlags eCompressionFlags = Ar.Read<ECompressionFlags>();
				int num = eCompressionFlags switch
				{
					ECompressionFlags.COMPRESS_None => 0, 
					ECompressionFlags.COMPRESS_ZLIB | ECompressionFlags.COMPRESS_GZIP | ECompressionFlags.COMPRESS_ForPackaging => 4, 
					_ => eCompressionFlags.HasFlag(ECompressionFlags.COMPRESS_ZLIB) ? 1 : ((!eCompressionFlags.HasFlag(ECompressionFlags.COMPRESS_GZIP)) ? ((!eCompressionFlags.HasFlag(ECompressionFlags.COMPRESS_Custom)) ? ((reader.Game != EGame.GAME_PlayerUnknownsBattlegrounds) ? ((reader.Game != EGame.GAME_DeadIsland2) ? (-1) : 6) : 3) : ((reader.Game != EGame.GAME_SeaOfThieves) ? 3 : 4)) : 2), 
				};
				CompressionMethod = ((num == -1) ? CompressionMethod.Unknown : reader.Info.CompressionMethods[num]);
			}
			else if (reader.Info.Version == EPakFileVersion.PakFile_Version_FNameBasedCompressionMethod && !reader.Info.IsSubVersion)
			{
				CompressionMethod = reader.Info.CompressionMethods[Ar.Read<byte>()];
			}
			else
			{
				CompressionMethod = reader.Info.CompressionMethods[Ar.Read<int>()];
			}
			if (reader.Info.Version < EPakFileVersion.PakFile_Version_NoTimestamps)
			{
				Ar.Position += 8L;
			}
			Ar.Position += 20L;
			if (reader.Info.Version >= EPakFileVersion.PakFile_Version_CompressionEncryption)
			{
				if (CompressionMethod != CompressionMethod.None)
				{
					CompressionBlocks = Ar.ReadArray<FPakCompressedBlock>();
				}
				Flags = (uint)Ar.ReadByte();
				CompressionBlockSize = Ar.Read<uint>();
			}
			if (Ar.Game == EGame.GAME_TEKKEN7)
			{
				Flags = (uint)(Flags & -2);
			}
			if (reader.Info.Version >= EPakFileVersion.PakFile_Version_RelativeChunkOffsets)
			{
				for (int i = 0; i < CompressionBlocks.Length; i++)
				{
					CompressionBlocks[i].CompressedStart += base.Offset;
					CompressionBlocks[i].CompressedEnd += base.Offset;
				}
			}
		}
		StructSize = (int)(Ar.Position - position);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public unsafe FPakEntry(PakFileReader reader, string path, byte* data)
		: base(reader)
	{
		base.Path = path;
		uint num = *(uint*)data;
		data += 4;
		uint compressionBlockSize;
		if ((num & 0x3F) == 63)
		{
			compressionBlockSize = *(uint*)data;
			data += 4;
		}
		else
		{
			compressionBlockSize = (num & 0x3F) << 11;
		}
		CompressionMethod = reader.Info.CompressionMethods[(int)((num >> 23) & 0x3F)];
		if ((num & int.MinValue) != 0)
		{
			base.Offset = (uint)(*(int*)data);
			data += 4;
		}
		else
		{
			base.Offset = *(long*)data;
			data += 8;
		}
		if (reader.Ar.Game == EGame.GAME_Snowbreak)
		{
			base.Offset ^= 522067228L;
		}
		if ((num & 0x40000000) != 0)
		{
			UncompressedSize = (uint)(*(int*)data);
			data += 4;
		}
		else
		{
			UncompressedSize = *(long*)data;
			data += 8;
		}
		base.Size = UncompressedSize;
		if (CompressionMethod != CompressionMethod.None)
		{
			if ((num & 0x20000000) != 0)
			{
				CompressedSize = (uint)(*(int*)data);
				data += 4;
			}
			else
			{
				CompressedSize = *(long*)data;
				data += 8;
			}
		}
		else
		{
			CompressedSize = UncompressedSize;
		}
		Flags |= (uint)(((num & 0x400000) != 0) ? 1 : 0);
		uint num2 = (num >> 6) & 0xFFFF;
		CompressionBlocks = new FPakCompressedBlock[num2];
		CompressionBlockSize = 0u;
		if (num2 != 0)
		{
			CompressionBlockSize = compressionBlockSize;
			if (num2 == 1)
			{
				CompressionBlockSize = (uint)UncompressedSize;
			}
		}
		StructSize = 53;
		if (CompressionMethod != CompressionMethod.None)
		{
			StructSize += (int)(4 + num2 * 2 * 8);
		}
		if (num2 == 1 && !IsEncrypted)
		{
			ref FPakCompressedBlock reference = ref CompressionBlocks[0];
			reference.CompressedStart = base.Offset + StructSize;
			reference.CompressedEnd = reference.CompressedStart + CompressedSize;
		}
		else if (num2 != 0)
		{
			uint* ptr = (uint*)data;
			int alignment = ((!IsEncrypted) ? 1 : 16);
			long num3 = base.Offset + StructSize;
			for (int i = 0; i < num2; i++)
			{
				ref FPakCompressedBlock reference2 = ref CompressionBlocks[i];
				reference2.CompressedStart = num3;
				reference2.CompressedEnd = num3 + *(ptr++);
				num3 += (reference2.CompressedEnd - reference2.CompressedStart).Align(alignment);
			}
		}
	}

	public FPakEntry(PakFileReader reader, FMemoryImageArchive Ar)
		: base(reader)
	{
		base.Offset = Ar.Read<long>();
		CompressedSize = Ar.Read<long>();
		UncompressedSize = Ar.Read<long>();
		base.Size = UncompressedSize;
		Ar.Position += 24L;
		CompressionBlocks = Ar.ReadArray<FPakCompressedBlock>();
		CompressionBlockSize = Ar.Read<uint>();
		CompressionMethod = reader.Info.CompressionMethods[Ar.Read<int>()];
		Flags = Ar.Read<byte>();
		if (reader.Info.Version >= EPakFileVersion.PakFile_Version_RelativeChunkOffsets)
		{
			for (int i = 0; i < CompressionBlocks.Length; i++)
			{
				CompressionBlocks[i].CompressedStart += base.Offset;
				CompressionBlocks[i].CompressedEnd += base.Offset;
			}
		}
		StructSize = 53;
		if (CompressionMethod != CompressionMethod.None)
		{
			StructSize += 4 + CompressionBlocks.Length * 2 * 8;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
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

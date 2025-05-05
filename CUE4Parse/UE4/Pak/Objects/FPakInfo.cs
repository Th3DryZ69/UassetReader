using System;
using System.Collections.Generic;
using System.IO;
using CUE4Parse.Compression;
using CUE4Parse.UE4.Exceptions;
using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;
using Serilog;

namespace CUE4Parse.UE4.Pak.Objects;

public class FPakInfo
{
	private enum OffsetsToTry
	{
		Size = 61,
		Size8_1 = 93,
		Size8_2 = 125,
		Size8_3 = 157,
		Size8 = 189,
		Size8a = 221,
		Size9 = 222,
		SizeHotta = 225,
		SizeLast = 226,
		SizeMax = 225
	}

	public const uint PAK_FILE_MAGIC = 1517228769u;

	public const uint PAK_FILE_MAGIC_OutlastTrials = 2777738526u;

	public const int COMPRESSION_METHOD_NAME_LEN = 32;

	public readonly uint Magic;

	public readonly EPakFileVersion Version;

	public readonly bool IsSubVersion;

	public readonly long IndexOffset;

	public readonly long IndexSize;

	public readonly FSHAHash IndexHash;

	public readonly bool EncryptedIndex;

	public readonly bool IndexIsFrozen;

	public readonly FGuid EncryptionKeyGuid;

	public readonly List<CompressionMethod> CompressionMethods;

	private static readonly OffsetsToTry[] _offsetsToTry = new OffsetsToTry[7]
	{
		OffsetsToTry.Size8a,
		OffsetsToTry.Size8,
		OffsetsToTry.Size,
		OffsetsToTry.Size9,
		OffsetsToTry.Size8_1,
		OffsetsToTry.Size8_2,
		OffsetsToTry.Size8_3
	};

	private unsafe FPakInfo(FArchive Ar, OffsetsToTry offsetToTry)
	{
		uint num = 0u;
		if (Ar.Game == EGame.GAME_TowerOfFantasy && offsetToTry == OffsetsToTry.SizeHotta)
		{
			num = Ar.Read<uint>();
			if (num > 255)
			{
				num = 0u;
			}
		}
		EncryptionKeyGuid = Ar.Read<FGuid>();
		EncryptedIndex = Ar.Read<byte>() != 0;
		Magic = Ar.Read<uint>();
		if (Magic != 1517228769 && (Ar.Game != EGame.GAME_OutlastTrials || Magic != 2777738526u))
		{
			return;
		}
		Version = ((num >= 2) ? ((EPakFileVersion)(Ar.Read<int>() ^ 2)) : Ar.Read<EPakFileVersion>());
		if (Ar.Game == EGame.GAME_StateOfDecay2)
		{
			Version &= (EPakFileVersion)65535;
		}
		IsSubVersion = Version == EPakFileVersion.PakFile_Version_FNameBasedCompressionMethod && offsetToTry == OffsetsToTry.Size8a;
		IndexOffset = Ar.Read<long>();
		if (Ar.Game == EGame.GAME_Snowbreak)
		{
			IndexOffset ^= 471670303L;
		}
		IndexSize = Ar.Read<long>();
		IndexHash = new FSHAHash(Ar);
		if (Ar.Game == EGame.GAME_MeetYourMaker && offsetToTry == OffsetsToTry.SizeHotta && Version >= EPakFileVersion.PakFile_Version_Fnv64BugFix)
		{
			Ar.Read<uint>();
		}
		if (Version == EPakFileVersion.PakFile_Version_FrozenIndex)
		{
			IndexIsFrozen = Ar.Read<byte>() != 0;
		}
		if (Version < EPakFileVersion.PakFile_Version_FNameBasedCompressionMethod)
		{
			CompressionMethods = new List<CompressionMethod>
			{
				CompressionMethod.None,
				CompressionMethod.Zlib,
				CompressionMethod.Gzip,
				CompressionMethod.Oodle,
				CompressionMethod.LZ4,
				CompressionMethod.Zstd
			};
		}
		else
		{
			int num2 = offsetToTry switch
			{
				OffsetsToTry.Size8a => 5, 
				OffsetsToTry.SizeHotta => 5, 
				OffsetsToTry.Size8 => 4, 
				OffsetsToTry.Size8_1 => 1, 
				OffsetsToTry.Size8_2 => 2, 
				OffsetsToTry.Size8_3 => 3, 
				_ => 4, 
			};
			int num3 = 32 * num2;
			byte* ptr = stackalloc byte[(int)(uint)num3];
			Ar.Serialize(ptr, num3);
			CompressionMethods = new List<CompressionMethod>(num2 + 1) { CompressionMethod.None };
			for (int i = 0; i < num2; i++)
			{
				string text = new string((sbyte*)(ptr + i * 32), 0, 32).TrimEnd('\0');
				if (!string.IsNullOrEmpty(text))
				{
					if (!Enum.TryParse<CompressionMethod>(text, ignoreCase: true, out var result))
					{
						Log.Warning("Unknown compression method '" + text + "' in " + Ar.Name);
						result = CompressionMethod.Unknown;
					}
					CompressionMethods.Add(result);
				}
			}
			if (num >= 3)
			{
				CompressionMethods.Remove(CompressionMethod.None);
			}
		}
		if (Version < EPakFileVersion.PakFile_Version_IndexEncryption)
		{
			EncryptedIndex = false;
		}
		if (Version < EPakFileVersion.PakFile_Version_EncryptionKeyGuid)
		{
			EncryptionKeyGuid = default(FGuid);
		}
	}

	public unsafe static FPakInfo ReadFPakInfo(FArchive Ar)
	{
		if (Ar.Length < 225)
		{
			throw new ParserException("File " + Ar.Name + " is too small to be a pak file");
		}
		Ar.Seek(-225L, SeekOrigin.End);
		byte* ptr = stackalloc byte[225];
		Ar.Serialize(ptr, 225);
		FPointerArchive fPointerArchive = new FPointerArchive(Ar.Name, ptr, 225L, Ar.Versions);
		EGame game = Ar.Game;
		OffsetsToTry[] array = ((game != EGame.GAME_TowerOfFantasy && game != EGame.GAME_MeetYourMaker) ? _offsetsToTry : new OffsetsToTry[1] { OffsetsToTry.SizeHotta });
		array = array;
		foreach (OffsetsToTry offsetsToTry in array)
		{
			fPointerArchive.Seek(0L - (long)offsetsToTry, SeekOrigin.End);
			FPakInfo fPakInfo = new FPakInfo(fPointerArchive, offsetsToTry);
			if (Ar.Game == EGame.GAME_OutlastTrials && fPakInfo.Magic == 2777738526u)
			{
				return fPakInfo;
			}
			if (fPakInfo.Magic == 1517228769)
			{
				return fPakInfo;
			}
		}
		throw new ParserException("File " + Ar.Name + " has an unknown format");
	}
}

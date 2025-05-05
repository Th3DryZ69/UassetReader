using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using CUE4Parse.Compression;
using CUE4Parse.FileProvider.Objects;
using CUE4Parse.UE4.Exceptions;
using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Pak.Objects;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;
using CUE4Parse.UE4.VirtualFileSystem;
using CUE4Parse.Utils;

namespace CUE4Parse.UE4.Pak;

public class PakFileReader : AbstractAesVfsReader
{
	public readonly FArchive Ar;

	public readonly FPakInfo Info;

	public override string MountPoint { get; protected set; }

	public sealed override long Length { get; set; }

	public override bool HasDirectoryIndex => true;

	public override FGuid EncryptionKeyGuid => Info.EncryptionKeyGuid;

	public override bool IsEncrypted => Info.EncryptedIndex;

	public PakFileReader(FArchive Ar)
		: base(Ar.Name, Ar.Versions)
	{
		this.Ar = Ar;
		Length = Ar.Length;
		Info = FPakInfo.ReadFPakInfo(Ar);
		if (Info.Version > EPakFileVersion.PakFile_Version_Fnv64BugFix && Ar.Game != EGame.GAME_TowerOfFantasy && Ar.Game != EGame.GAME_MeetYourMaker && Ar.Game != EGame.GAME_Snowbreak)
		{
			AbstractVfsReader.log.Warning($"Pak file \"{base.Name}\" has unsupported version {Info.Version}");
		}
	}

	public PakFileReader(string filePath, VersionContainer? versions = null)
		: this(new FileInfo(filePath), versions)
	{
	}

	public PakFileReader(FileInfo file, VersionContainer? versions = null)
		: this(file.FullName, file.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite), versions)
	{
	}

	public PakFileReader(string filePath, Stream stream, VersionContainer? versions = null)
		: this(new FStreamArchive(filePath, stream, versions))
	{
	}

	public override byte[] Extract(VfsEntry entry)
	{
		if (!(entry is FPakEntry fPakEntry) || entry.Vfs != this)
		{
			throw new ArgumentException("Wrong pak file reader, required " + entry.Vfs.Name + ", this is " + base.Name);
		}
		FArchive fArchive = (base.IsConcurrent ? ((FArchive)Ar.Clone()) : Ar);
		if (fPakEntry.IsCompressed)
		{
			byte[] array = new byte[(int)fPakEntry.UncompressedSize];
			int num = 0;
			FPakCompressedBlock[] compressionBlocks = fPakEntry.CompressionBlocks;
			for (int i = 0; i < compressionBlocks.Length; i++)
			{
				FPakCompressedBlock fPakCompressedBlock = compressionBlocks[i];
				fArchive.Position = fPakCompressedBlock.CompressedStart;
				int num2 = (int)fPakCompressedBlock.Size;
				int length = num2.Align((!fPakEntry.IsEncrypted) ? 1 : 16);
				byte[] compressed = ReadAndDecrypt(length, fArchive, fPakEntry.IsEncrypted);
				int uncompressedSize = (int)Math.Min(fPakEntry.CompressionBlockSize, fPakEntry.UncompressedSize - num);
				CUE4Parse.Compression.Compression.Decompress(compressed, 0, num2, array, num, uncompressedSize, fPakEntry.CompressionMethod);
				num += (int)fPakEntry.CompressionBlockSize;
			}
			return array;
		}
		fArchive.Position = fPakEntry.Offset + fPakEntry.StructSize;
		int num3 = (int)fPakEntry.UncompressedSize.Align((!fPakEntry.IsEncrypted) ? 1 : 16);
		byte[] array2 = ReadAndDecrypt(num3, fArchive, fPakEntry.IsEncrypted);
		if (num3 == fPakEntry.UncompressedSize)
		{
			return array2;
		}
		return array2.SubByteArray((int)fPakEntry.UncompressedSize);
	}

	public override IReadOnlyDictionary<string, GameFile> Mount(bool caseInsensitive = false)
	{
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();
		if (Info.Version >= EPakFileVersion.PakFile_Version_PathHashIndex)
		{
			ReadIndexUpdated(caseInsensitive);
		}
		else if (Info.IndexIsFrozen)
		{
			ReadFrozenIndex(caseInsensitive);
		}
		else
		{
			ReadIndexLegacy(caseInsensitive);
		}
		if (Globals.LogVfsMounts)
		{
			TimeSpan elapsed = stopwatch.Elapsed;
			StringBuilder stringBuilder = new StringBuilder($"Pak \"{base.Name}\": {FileCount} files");
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

	private void ReadIndexLegacy(bool caseInsensitive)
	{
		Ar.Position = Info.IndexOffset;
		FByteArchive fByteArchive = new FByteArchive(base.Name + " - Index", ReadAndDecrypt((int)Info.IndexSize), base.Versions);
		string mountPoint;
		try
		{
			mountPoint = fByteArchive.ReadFString();
		}
		catch (Exception innerException)
		{
			throw new InvalidAesKeyException($"Given aes key '{base.AesKey?.KeyString}'is not working with '{base.Name}'", innerException);
		}
		ValidateMountPoint(ref mountPoint);
		MountPoint = mountPoint;
		int num = fByteArchive.Read<int>();
		Dictionary<string, GameFile> dictionary = new Dictionary<string, GameFile>(num);
		for (int i = 0; i < num; i++)
		{
			string text = mountPoint + fByteArchive.ReadFString();
			FPakEntry fPakEntry = new FPakEntry(this, text, fByteArchive);
			if (fPakEntry == null || !fPakEntry.IsDeleted || fPakEntry.Size != 0L)
			{
				if (fPakEntry.IsEncrypted)
				{
					base.EncryptedFileCount++;
				}
				if (caseInsensitive)
				{
					dictionary[text.ToLowerInvariant()] = fPakEntry;
				}
				else
				{
					dictionary[text] = fPakEntry;
				}
			}
		}
		base.Files = dictionary;
	}

	private unsafe void ReadIndexUpdated(bool caseInsensitive)
	{
		Ar.Position = Info.IndexOffset;
		FArchive fArchive = new FByteArchive(base.Name + " - Primary Index", ReadAndDecrypt((int)Info.IndexSize));
		string mountPoint;
		try
		{
			mountPoint = fArchive.ReadFString();
		}
		catch (Exception innerException)
		{
			throw new InvalidAesKeyException($"Given aes key '{base.AesKey?.KeyString}'is not working with '{base.Name}'", innerException);
		}
		ValidateMountPoint(ref mountPoint);
		MountPoint = mountPoint;
		int capacity = fArchive.Read<int>();
		base.EncryptedFileCount = 0;
		fArchive.Position += 8L;
		if (!fArchive.ReadBoolean())
		{
			throw new ParserException(fArchive, "No path hash index");
		}
		fArchive.Position += 36L;
		if (!fArchive.ReadBoolean())
		{
			throw new ParserException(fArchive, "No directory index");
		}
		long position = fArchive.Read<long>();
		long num = fArchive.Read<long>();
		fArchive.Position += 20L;
		int length = fArchive.Read<int>();
		byte[] array = fArchive.ReadBytes(length);
		if (fArchive.Read<int>() < 0)
		{
			throw new ParserException("Corrupt pak PrimaryIndex detected");
		}
		Ar.Position = position;
		FByteArchive fByteArchive = new FByteArchive(base.Name + " - Directory Index", ReadAndDecrypt((int)num));
		int num2 = fByteArchive.Read<int>();
		Dictionary<string, GameFile> dictionary = new Dictionary<string, GameFile>(capacity);
		fixed (byte* ptr = array)
		{
			for (int i = 0; i < num2; i++)
			{
				string text = fByteArchive.ReadFString();
				int num3 = fByteArchive.Read<int>();
				for (int j = 0; j < num3; j++)
				{
					string text2 = fByteArchive.ReadFString();
					string text6;
					if (mountPoint.EndsWith('/') && text.StartsWith('/'))
					{
						string text5;
						if (text.Length != 1)
						{
							string text3 = mountPoint;
							string text4 = text;
							text5 = text3 + text4.Substring(1, text4.Length - 1) + text2;
						}
						else
						{
							text5 = mountPoint + text2;
						}
						text6 = text5;
					}
					else
					{
						text6 = mountPoint + text + text2;
					}
					int num4 = fByteArchive.Read<int>();
					if (num4 != int.MinValue)
					{
						FPakEntry fPakEntry = new FPakEntry(this, text6, ptr + num4);
						if (fPakEntry.IsEncrypted)
						{
							base.EncryptedFileCount++;
						}
						if (caseInsensitive)
						{
							dictionary[text6.ToLowerInvariant()] = fPakEntry;
						}
						else
						{
							dictionary[text6] = fPakEntry;
						}
					}
				}
			}
		}
		base.Files = dictionary;
	}

	private void ReadFrozenIndex(bool caseInsensitive)
	{
		this.Ar.Position = Info.IndexOffset;
		FMemoryImageArchive Ar = new FMemoryImageArchive(new FByteArchive("FPakFileData", this.Ar.ReadBytes((int)Info.IndexSize)));
		string mountPoint = Ar.ReadFString();
		ValidateMountPoint(ref mountPoint);
		MountPoint = mountPoint;
		FPakEntry[] array = Ar.ReadArray(() => new FPakEntry(this, Ar));
		Dictionary<string, GameFile> dictionary = new Dictionary<string, GameFile>(array.Length);
		foreach (var item in Ar.ReadTMap(() => Ar.ReadFString(), () => Ar.ReadTMap(() => Ar.ReadFString(), () => Ar.Read<int>(), 16, 4), 16, 56))
		{
			var (text, _) = item;
			foreach (var (text2, num) in item.Item2)
			{
				string text6;
				if (mountPoint.EndsWith('/') && text.StartsWith('/'))
				{
					string text5;
					if (text.Length != 1)
					{
						string text3 = mountPoint;
						string text4 = text;
						text5 = text3 + text4.Substring(1, text4.Length - 1) + text2;
					}
					else
					{
						text5 = mountPoint + text2;
					}
					text6 = text5;
				}
				else
				{
					text6 = mountPoint + text + text2;
				}
				FPakEntry fPakEntry = array[num];
				fPakEntry.Path = text6;
				if (!fPakEntry.IsDeleted || fPakEntry.Size != 0L)
				{
					if (fPakEntry.IsEncrypted)
					{
						base.EncryptedFileCount++;
					}
					if (caseInsensitive)
					{
						dictionary[text6.ToLowerInvariant()] = fPakEntry;
					}
					else
					{
						dictionary[text6] = fPakEntry;
					}
				}
			}
		}
		base.Files = dictionary;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected override byte[] ReadAndDecrypt(int length)
	{
		return ReadAndDecrypt(length, Ar, IsEncrypted);
	}

	public override byte[] MountPointCheckBytes()
	{
		FArchive obj = (base.IsConcurrent ? ((FArchive)Ar.Clone()) : Ar);
		obj.Position = Info.IndexOffset;
		return obj.ReadBytes(260.Align(16));
	}

	public override void Dispose()
	{
		Ar.Dispose();
	}
}

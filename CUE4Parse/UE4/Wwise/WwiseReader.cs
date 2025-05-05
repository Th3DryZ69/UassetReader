using System.Collections.Generic;
using CUE4Parse.UE4.Exceptions;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Wwise.Enums;
using CUE4Parse.UE4.Wwise.Objects;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Wwise;

[JsonConverter(typeof(WwiseConverter))]
public class WwiseReader
{
	public BankHeader Header { get; }

	public AkFolder[]? Folders { get; }

	public string[]? Initialization { get; }

	public DataIndex[]? WemIndexes { get; }

	public byte[][]? WemSounds { get; }

	public Hierarchy[]? Hierarchy { get; }

	public Dictionary<uint, string>? IdToString { get; }

	public string? Platform { get; }

	public Dictionary<string, byte[]> WwiseEncodedMedias { get; }

	public WwiseReader(FArchive Ar)
	{
		IdToString = new Dictionary<uint, string>();
		WwiseEncodedMedias = new Dictionary<string, byte[]>();
		AkFolder[] folders;
		while (Ar.Position < Ar.Length)
		{
			ESectionIdentifier eSectionIdentifier = Ar.Read<ESectionIdentifier>();
			int num = Ar.Read<int>();
			long position = Ar.Position;
			switch (eSectionIdentifier)
			{
			case ESectionIdentifier.AKPK:
			{
				if (!Ar.ReadBoolean())
				{
					throw new ParserException(Ar, "'" + Ar.Name + "' has unsupported endianness.");
				}
				Ar.Position += 16L;
				Folders = Ar.ReadArray(() => new AkFolder(Ar));
				folders = Folders;
				for (int num4 = 0; num4 < folders.Length; num4++)
				{
					folders[num4].PopulateName(Ar);
				}
				folders = Folders;
				foreach (AkFolder akFolder in folders)
				{
					akFolder.Entries = new AkEntry[Ar.Read<uint>()];
					for (int num5 = 0; num5 < akFolder.Entries.Length; num5++)
					{
						AkEntry akEntry = new AkEntry(Ar);
						akEntry.Path = Folders[akEntry.FolderId].Name;
						long position2 = Ar.Position;
						Ar.Position = akEntry.Offset;
						akEntry.IsSoundBank = Ar.Read<ESectionIdentifier>() == ESectionIdentifier.BKHD;
						Ar.Position -= 4L;
						akEntry.Data = Ar.ReadBytes(akEntry.Size);
						Ar.Position = position2;
						akFolder.Entries[num5] = akEntry;
					}
				}
				break;
			}
			case ESectionIdentifier.BKHD:
				Header = Ar.Read<BankHeader>();
				break;
			case ESectionIdentifier.INIT:
				Initialization = Ar.ReadArray(delegate
				{
					Ar.Position += 4L;
					return Ar.ReadFString();
				});
				break;
			case ESectionIdentifier.DIDX:
				WemIndexes = Ar.ReadArray(num / 12, Ar.Read<DataIndex>);
				break;
			case ESectionIdentifier.DATA:
				if (WemIndexes != null)
				{
					WemSounds = new byte[WemIndexes.Length][];
					for (int num3 = 0; num3 < WemSounds.Length; num3++)
					{
						Ar.Position = position + WemIndexes[num3].Offset;
						WemSounds[num3] = Ar.ReadBytes(WemIndexes[num3].Length);
						WwiseEncodedMedias[WemIndexes[num3].Id.ToString()] = WemSounds[num3];
					}
				}
				break;
			case ESectionIdentifier.HIRC:
				Hierarchy = Ar.ReadArray(() => new Hierarchy(Ar));
				break;
			case ESectionIdentifier.STID:
			{
				Ar.Position += 4L;
				int num2 = Ar.Read<int>();
				for (int i = 0; i < num2; i++)
				{
					IdToString[Ar.Read<uint>()] = Ar.ReadString();
				}
				break;
			}
			case ESectionIdentifier.PLAT:
				Platform = Ar.ReadFString();
				break;
			}
			if (Ar.Position != position + num)
			{
				long position3 = position + num;
				Ar.Position = position3;
			}
		}
		if (Folders == null)
		{
			return;
		}
		folders = Folders;
		for (int num4 = 0; num4 < folders.Length; num4++)
		{
			AkEntry[] entries = folders[num4].Entries;
			foreach (AkEntry akEntry2 in entries)
			{
				if (!akEntry2.IsSoundBank && akEntry2.Data != null)
				{
					WwiseEncodedMedias[IdToString.TryGetValue(akEntry2.NameHash, out string value) ? value : $"{akEntry2.Path.ToUpper()}_{akEntry2.NameHash}"] = akEntry2.Data;
				}
			}
		}
	}
}

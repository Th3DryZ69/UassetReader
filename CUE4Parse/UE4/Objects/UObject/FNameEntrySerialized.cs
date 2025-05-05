using System;
using System.Text;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Objects.UObject;

public struct FNameEntrySerialized
{
	public string? Name;

	public ulong hashVersion;

	public FNameEntrySerialized(FArchive Ar)
	{
		hashVersion = 0uL;
		bool num = Ar.Ver >= EUnrealEngineObjectUE4Version.NAME_HASHES_SERIALIZED || Ar.Game == EGame.GAME_GearsOfWar4;
		Name = Ar.ReadFString().Trim();
		if (num)
		{
			Ar.Position += 4L;
		}
	}

	public FNameEntrySerialized(string name, ulong HashVersion = 0uL)
	{
		Name = name;
		hashVersion = HashVersion;
	}

	public override string ToString()
	{
		return Name ?? "None";
	}

	public static FNameEntrySerialized[] LoadNameBatch(FArchive nameAr, int nameCount)
	{
		FNameEntrySerialized[] array = new FNameEntrySerialized[nameCount];
		for (int i = 0; i < nameCount; i++)
		{
			array[i] = LoadNameHeader(nameAr);
		}
		return array;
	}

	public static FNameEntrySerialized[] LoadNameBatch(FArchive Ar)
	{
		int num = Ar.Read<int>();
		if (num == 0)
		{
			return Array.Empty<FNameEntrySerialized>();
		}
		Ar.Position += 4L;
		ulong num2 = Ar.Read<ulong>();
		Ar.Position += num * 8;
		FSerializedNameHeader[] array = Ar.ReadArray<FSerializedNameHeader>(num);
		FNameEntrySerialized[] array2 = new FNameEntrySerialized[num];
		for (int i = 0; i < num; i++)
		{
			FSerializedNameHeader fSerializedNameHeader = array[i];
			int length = (int)fSerializedNameHeader.Length;
			string name = (fSerializedNameHeader.IsUtf16 ? new string(Ar.ReadArray<char>(length)) : Encoding.UTF8.GetString(Ar.ReadBytes(length)));
			array2[i] = new FNameEntrySerialized(name, 0uL)
			{
				hashVersion = num2
			};
		}
		return array2;
	}

	private unsafe static FNameEntrySerialized LoadNameHeader(FArchive Ar)
	{
		FSerializedNameHeader fSerializedNameHeader = Ar.Read<FSerializedNameHeader>();
		int length = (int)fSerializedNameHeader.Length;
		if (fSerializedNameHeader.IsUtf16)
		{
			if (Ar.Position % 2 == 1)
			{
				Ar.Position++;
			}
			int num = length * 2;
			byte* ptr = stackalloc byte[(int)(uint)num];
			Ar.Serialize(ptr, num);
			return new FNameEntrySerialized(new string((char*)ptr, 0, length), 0uL);
		}
		byte* ptr2 = stackalloc byte[(int)(uint)length];
		Ar.Serialize(ptr2, length);
		return new FNameEntrySerialized(new string((sbyte*)ptr2, 0, length), 0uL);
	}
}

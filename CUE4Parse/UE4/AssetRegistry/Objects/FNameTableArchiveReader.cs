using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CUE4Parse.UE4.AssetRegistry.Readers;
using CUE4Parse.UE4.Exceptions;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.AssetRegistry.Objects;

public class FNameTableArchiveReader : FAssetRegistryArchive
{
	public FNameTableArchiveReader(FArchive Ar, FAssetRegistryHeader header)
		: base(Ar, header)
	{
		long num = Ar.Read<long>();
		if (num > Ar.Length)
		{
			throw new ArgumentOutOfRangeException("nameOffset", "Archive is corrupted");
		}
		if (num > 0)
		{
			long position = Ar.Position;
			Ar.Position = num;
			int num2 = Ar.Read<int>();
			if (num2 < 0)
			{
				throw new ArgumentOutOfRangeException("nameCount", "Archive is corrupted");
			}
			long val = (Ar.Length - Ar.Position) / 4;
			NameMap = new FNameEntrySerialized[Math.Min(num2, val)];
			Ar.ReadArray(NameMap, () => new FNameEntrySerialized(Ar));
			Ar.Position = position;
		}
		else
		{
			NameMap = Array.Empty<FNameEntrySerialized>();
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override FName ReadFName()
	{
		int num = baseArchive.Read<int>();
		int number = baseArchive.Read<int>();
		if (num < 0 || num >= NameMap.Length)
		{
			throw new ParserException(baseArchive, $"FName could not be read, requested index {num}, name map size {NameMap.Length}");
		}
		return new FName(NameMap[num], num, number);
	}

	public override void SerializeTagsAndBundles(FAssetData assetData)
	{
		int num = baseArchive.Read<int>();
		Dictionary<FName, string> dictionary = new Dictionary<FName, string>();
		for (int i = 0; i < num; i++)
		{
			dictionary[ReadFName()] = baseArchive.ReadFString();
		}
		assetData.TagsAndValues = dictionary;
		assetData.TaggedAssetBundles = new FAssetBundleData();
	}

	public override object Clone()
	{
		return new FNameTableArchiveReader((FArchive)baseArchive.Clone(), Header);
	}
}

using System.Collections.Generic;
using CUE4Parse.UE4.AssetRegistry.Readers;
using CUE4Parse.UE4.Exceptions;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.AssetRegistry.Objects;

public class FAssetRegistryReader : FAssetRegistryArchive
{
	private const uint AssetRegistryNumberedNameBit = 2147483648u;

	public readonly FStore Tags;

	public FAssetRegistryReader(FArchive Ar, FAssetRegistryHeader header)
		: base(Ar, header)
	{
		NameMap = FNameEntrySerialized.LoadNameBatch(Ar);
		Tags = new FStore(this);
	}

	public override FName ReadFName()
	{
		uint num = baseArchive.Read<uint>();
		uint number = 0u;
		if ((num & 0x80000000u) != 0)
		{
			num -= 2147483648u;
			number = baseArchive.Read<uint>();
		}
		if (num >= NameMap.Length)
		{
			throw new ParserException(baseArchive, $"FName could not be read, requested index {num}, name map size {NameMap.Length}");
		}
		return new FName(NameMap, (int)num, (int)number);
	}

	public override void SerializeTagsAndBundles(FAssetData assetData)
	{
		ulong mapSize = baseArchive.Read<ulong>();
		Dictionary<FName, string> dictionary = new Dictionary<FName, string>();
		foreach (FNumberedPair item in FPartialMapHandle.MakeFullHandle(Tags, mapSize).GetEnumerable())
		{
			dictionary[item.Key] = FValueHandle.GetString(Tags, item.Value);
		}
		assetData.TagsAndValues = dictionary;
		assetData.TaggedAssetBundles = new FAssetBundleData(this);
	}

	public override object Clone()
	{
		return new FAssetRegistryReader((FArchive)baseArchive.Clone(), Header);
	}
}

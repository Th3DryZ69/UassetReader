using System;
using System.Collections.Generic;
using CUE4Parse.UE4.Objects.UObject;

namespace CUE4Parse.UE4.AssetRegistry.Objects;

public class FMapHandle
{
	public readonly bool bHasNumberlessKeys;

	public readonly FStore Store;

	public readonly ushort Num;

	public readonly uint PairBegin;

	public FMapHandle(bool hasNumberlessKeys, FStore store, ushort num, uint pairBegin)
	{
		bHasNumberlessKeys = hasNumberlessKeys;
		Store = store;
		Num = num;
		PairBegin = pairBegin;
	}

	public IEnumerable<FNumberedPair> GetEnumerable()
	{
		if (bHasNumberlessKeys)
		{
			foreach (FNumberlessPair item in GetNumberlessView())
			{
				yield return new FNumberedPair(new FName(Store.NameMap[item.Key].Name), item.Value);
			}
			yield break;
		}
		foreach (FNumberedPair item2 in GetNumberView())
		{
			yield return item2;
		}
	}

	private IEnumerable<FNumberedPair> GetNumberView()
	{
		return new ArraySegment<FNumberedPair>(Store.Pairs, (int)PairBegin, Num);
	}

	private IEnumerable<FNumberlessPair> GetNumberlessView()
	{
		return new ArraySegment<FNumberlessPair>(Store.NumberlessPairs, (int)PairBegin, Num);
	}
}

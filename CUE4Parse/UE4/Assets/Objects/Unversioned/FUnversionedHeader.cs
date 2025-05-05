using System.Collections;
using System.Collections.Generic;
using CUE4Parse.UE4.Readers;
using CUE4Parse.Utils;

namespace CUE4Parse.UE4.Assets.Objects.Unversioned;

public class FUnversionedHeader
{
	public IReadOnlyList<FFragment> Fragments;

	public BitArray ZeroMask;

	public readonly bool HasNonZeroValues;

	public bool HasValues => HasNonZeroValues | (ZeroMask.Length > 0);

	public FUnversionedHeader(FArchive Ar)
	{
		List<FFragment> list = new List<FFragment>();
		int num = 0;
		uint num2 = 0u;
		FFragment item;
		do
		{
			item = new FFragment(Ar.Read<ushort>());
			list.Add(item);
			if (item.HasAnyZeroes)
			{
				num += item.ValueNum;
			}
			else
			{
				num2 += item.ValueNum;
			}
		}
		while (!item.IsLast);
		if (num > 0)
		{
			LoadZeroMaskData(Ar, num, out ZeroMask);
			HasNonZeroValues = num2 != 0 || ZeroMask.Contains(search: false);
		}
		else
		{
			ZeroMask = new BitArray(0);
			HasNonZeroValues = num2 != 0;
		}
		Fragments = list;
	}

	private static void LoadZeroMaskData(FArchive reader, int numBits, out BitArray data)
	{
		if (numBits <= 8)
		{
			data = new BitArray(new byte[1] { reader.Read<byte>() });
		}
		else if (numBits <= 16)
		{
			data = new BitArray(new int[1] { reader.Read<ushort>() });
		}
		else
		{
			int num = numBits.DivideAndRoundUp(32);
			int[] array = new int[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = reader.Read<int>();
			}
			data = new BitArray(array);
		}
		data.Length = numBits;
	}
}

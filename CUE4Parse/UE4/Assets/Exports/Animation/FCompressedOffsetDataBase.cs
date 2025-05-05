using System;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Assets.Exports.Animation;

public class FCompressedOffsetDataBase<T> where T : struct
{
	public T[] OffsetData = Array.Empty<T>();

	public int StripSize;

	public FCompressedOffsetDataBase(int stripSize = 2)
	{
		StripSize = stripSize;
	}

	public FCompressedOffsetDataBase(FArchive Ar)
	{
		OffsetData = Ar.ReadArray<T>();
		StripSize = Ar.Read<int>();
	}

	public T GetOffsetData(int stripIndex, int offset)
	{
		return OffsetData[stripIndex * StripSize + offset];
	}

	public bool IsValid()
	{
		if (StripSize > 0)
		{
			return OffsetData.Length != 0;
		}
		return false;
	}
}

using System;
using System.Collections.Generic;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.GameTypes.FN.Assets.Exports;

public class FFortCreativeVkPalette
{
	public Dictionary<string, FFortCreativeVkPalette_ProjectInfo> LinkCodeMap;

	public FFortCreativeVkPalette(FArchive Ar)
	{
		Ar.Read<int>();
		int num = Ar.Read<int>();
		if (num > 0)
		{
			throw new NotImplementedException();
		}
		LinkCodeMap = new Dictionary<string, FFortCreativeVkPalette_ProjectInfo>(num);
		for (int i = 0; i < num; i++)
		{
			LinkCodeMap[Ar.ReadFString()] = new FFortCreativeVkPalette_ProjectInfo(Ar);
		}
	}
}

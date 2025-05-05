using System.Collections.Generic;
using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Assets.Readers;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Objects.UObject;

[JsonConverter(typeof(FFormatContainerConverter))]
public class FFormatContainer
{
	public readonly SortedDictionary<FName, FByteBulkData> Formats;

	public FFormatContainer(FAssetArchive Ar)
	{
		int num = Ar.Read<int>();
		Formats = new SortedDictionary<FName, FByteBulkData>();
		for (int i = 0; i < num; i++)
		{
			Formats[Ar.ReadFName()] = new FByteBulkData(Ar);
		}
	}
}

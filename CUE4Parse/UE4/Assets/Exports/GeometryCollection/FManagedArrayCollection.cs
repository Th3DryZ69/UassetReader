using System.Collections.Generic;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.UObject;

namespace CUE4Parse.UE4.Assets.Exports.GeometryCollection;

public class FManagedArrayCollection
{
	public readonly int Version;

	public readonly Dictionary<FName, int> GroupInfo;

	public readonly Dictionary<FKeyType, FValueType> Map;

	public FManagedArrayCollection(FAssetArchive Ar)
	{
		Version = Ar.Read<int>();
		int num = Ar.Read<int>();
		GroupInfo = new Dictionary<FName, int>(num);
		for (int i = 0; i < num; i++)
		{
			GroupInfo[Ar.ReadFName()] = Ar.Read<int>();
		}
		num = Ar.Read<int>();
		Map = new Dictionary<FKeyType, FValueType>(num);
		for (int j = 0; j < num; j++)
		{
			Map[new FKeyType(Ar)] = new FValueType(Ar, Version);
		}
	}
}

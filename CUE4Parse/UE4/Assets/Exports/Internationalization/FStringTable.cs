using System.Collections.Generic;
using CUE4Parse.UE4.Assets.Readers;

namespace CUE4Parse.UE4.Assets.Exports.Internationalization;

public class FStringTable
{
	public string TableNamespace;

	public Dictionary<string, string> KeysToMetaData;

	public FStringTable(FAssetArchive Ar)
	{
		TableNamespace = Ar.ReadFString();
		int num = Ar.Read<int>();
		KeysToMetaData = new Dictionary<string, string>(num);
		for (int i = 0; i < num; i++)
		{
			KeysToMetaData[Ar.ReadFString()] = Ar.ReadFString();
		}
	}
}

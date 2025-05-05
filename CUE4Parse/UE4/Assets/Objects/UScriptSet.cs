using System.Collections.Generic;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Exceptions;
using Newtonsoft.Json;
using Serilog;

namespace CUE4Parse.UE4.Assets.Objects;

[JsonConverter(typeof(UScriptSetConverter))]
public class UScriptSet
{
	public readonly List<FPropertyTagType> Properties;

	public UScriptSet()
	{
		Properties = new List<FPropertyTagType>();
	}

	public UScriptSet(FAssetArchive Ar, FPropertyTagData? tagData)
	{
		string propertyType = tagData?.InnerType ?? throw new ParserException(Ar, "UScriptSet needs inner type");
		int num = Ar.Read<int>();
		for (int i = 0; i < num; i++)
		{
			FPropertyTagType.ReadPropertyTagType(Ar, propertyType, tagData.InnerTypeData, ReadType.ARRAY);
		}
		int num2 = Ar.Read<int>();
		Properties = new List<FPropertyTagType>(num2);
		for (int j = 0; j < num2; j++)
		{
			FPropertyTagType fPropertyTagType = FPropertyTagType.ReadPropertyTagType(Ar, propertyType, tagData.InnerTypeData, ReadType.ARRAY);
			if (fPropertyTagType != null)
			{
				Properties.Add(fPropertyTagType);
				continue;
			}
			Log.Debug($"Failed to read element for index {j} in set");
		}
	}
}

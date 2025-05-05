using System.Collections.Generic;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Exceptions;
using CUE4Parse.UE4.Versions;
using Newtonsoft.Json;
using Serilog;

namespace CUE4Parse.UE4.Assets.Objects;

[JsonConverter(typeof(UScriptArrayConverter))]
public class UScriptArray
{
	public readonly string InnerType;

	public readonly FPropertyTagData? InnerTagData;

	public readonly List<FPropertyTagType> Properties;

	public UScriptArray(string innerType)
	{
		InnerType = innerType;
		InnerTagData = null;
		Properties = new List<FPropertyTagType>();
	}

	public UScriptArray(FAssetArchive Ar, FPropertyTagData? tagData)
	{
		InnerType = tagData?.InnerType ?? throw new ParserException(Ar, "UScriptArray needs inner type");
		int num = Ar.Read<int>();
		if (Ar.HasUnversionedProperties)
		{
			InnerTagData = tagData.InnerTypeData;
		}
		else if (Ar.Ver >= EUnrealEngineObjectUE4Version.INNER_ARRAY_TAG_INFO && InnerType == "StructProperty")
		{
			InnerTagData = new FPropertyTag(Ar, readData: false).TagData;
			if (InnerTagData == null)
			{
				throw new ParserException(Ar, "Couldn't read ArrayProperty with inner type " + InnerType);
			}
		}
		Properties = new List<FPropertyTagType>(num);
		for (int i = 0; i < num; i++)
		{
			FPropertyTagType fPropertyTagType = FPropertyTagType.ReadPropertyTagType(Ar, InnerType, InnerTagData, ReadType.ARRAY);
			if (fPropertyTagType != null)
			{
				Properties.Add(fPropertyTagType);
				continue;
			}
			Log.Debug($"Failed to read array property of type {InnerType} at ${Ar.Position}, index {i}");
		}
	}

	public override string ToString()
	{
		return $"{InnerType}[{Properties.Count}]";
	}
}

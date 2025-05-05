using System.Collections.Generic;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Exceptions;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Objects;

[JsonConverter(typeof(UScriptMapConverter))]
public class UScriptMap
{
	public Dictionary<FPropertyTagType?, FPropertyTagType?> Properties;

	public UScriptMap()
	{
		Properties = new Dictionary<FPropertyTagType, FPropertyTagType>();
	}

	public UScriptMap(FAssetArchive Ar, FPropertyTagData tagData)
	{
		if (tagData.InnerType == null || tagData.ValueType == null)
		{
			throw new ParserException(Ar, "Can't serialize UScriptMap without key or value type");
		}
		if (!Ar.HasUnversionedProperties && Ar.Versions.MapStructTypes.TryGetValue(tagData.Name, out KeyValuePair<string, string> value))
		{
			if (!string.IsNullOrEmpty(value.Key))
			{
				tagData.InnerTypeData = new FPropertyTagData(value.Key);
			}
			if (!string.IsNullOrEmpty(value.Value))
			{
				tagData.ValueTypeData = new FPropertyTagData(value.Value);
			}
		}
		int num = Ar.Read<int>();
		for (int i = 0; i < num; i++)
		{
			FPropertyTagType.ReadPropertyTagType(Ar, tagData.InnerType, tagData.InnerTypeData, ReadType.MAP);
		}
		int num2 = Ar.Read<int>();
		Properties = new Dictionary<FPropertyTagType, FPropertyTagType>(num2);
		for (int j = 0; j < num2; j++)
		{
			bool flag = false;
			try
			{
				FPropertyTagType key = FPropertyTagType.ReadPropertyTagType(Ar, tagData.InnerType, tagData.InnerTypeData, ReadType.MAP);
				flag = true;
				FPropertyTagType value2 = FPropertyTagType.ReadPropertyTagType(Ar, tagData.ValueType, tagData.ValueTypeData, ReadType.MAP);
				Properties[key] = value2;
			}
			catch (ParserException innerException)
			{
				throw new ParserException(Ar, $"Failed to read {(flag ? "value" : "key")} for index {j} in map", innerException);
			}
		}
	}
}

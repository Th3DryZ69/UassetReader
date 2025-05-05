using System;
using System.Collections.Generic;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.Utils;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Objects;

[JsonConverter(typeof(EnumPropertyConverter))]
public class EnumProperty : FPropertyTagType<FName>
{
	public EnumProperty(FAssetArchive Ar, FPropertyTagData? tagData, ReadType type)
	{
		if (type == ReadType.ZERO)
		{
			base.Value = new FName(IndexToEnum(Ar, tagData, 0));
		}
		else if (Ar.HasUnversionedProperties && type == ReadType.NORMAL)
		{
			int index = 0;
			if (tagData != null && tagData.InnerType != null)
			{
				object obj = FPropertyTagType.ReadPropertyTagType(Ar, tagData.InnerType, tagData.InnerTypeData, ReadType.NORMAL)?.GenericValue;
				if (obj != null && obj.IsNumericType())
				{
					index = Convert.ToInt32(obj);
				}
			}
			else
			{
				index = Ar.Read<byte>();
			}
			base.Value = new FName(IndexToEnum(Ar, tagData, index));
		}
		else
		{
			base.Value = Ar.ReadFName();
		}
	}

	private static string IndexToEnum(FAssetArchive Ar, FPropertyTagData? tagData, int index)
	{
		string text = tagData?.EnumName;
		if (text == null)
		{
			return index.ToString();
		}
		if (tagData.Enum != null)
		{
			(FName, long)[] names = tagData.Enum.Names;
			for (int i = 0; i < names.Length; i++)
			{
				(FName, long) tuple = names[i];
				var (fName, _) = tuple;
				if (tuple.Item2 == index)
				{
					return fName.Text;
				}
			}
			return text + "::" + index;
		}
		if (Ar.Owner.Mappings != null && Ar.Owner.Mappings.Enums.TryGetValue(text, out Dictionary<int, string> value) && value.TryGetValue(index, out var value2))
		{
			return text + "::" + value2;
		}
		return text + "::" + index;
	}
}

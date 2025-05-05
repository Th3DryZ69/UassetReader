using System;
using CUE4Parse.UE4.Assets.Readers;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Objects;

[JsonConverter(typeof(BoolPropertyConverter))]
public class BoolProperty : FPropertyTagType<bool>
{
	public BoolProperty(FAssetArchive Ar, FPropertyTagData? tagData, ReadType type)
	{
		switch (type)
		{
		case ReadType.NORMAL:
			if (!Ar.HasUnversionedProperties)
			{
				base.Value = tagData != null && tagData.Bool == true;
				break;
			}
			goto case ReadType.MAP;
		case ReadType.MAP:
		case ReadType.ARRAY:
			base.Value = Ar.ReadFlag();
			break;
		case ReadType.ZERO:
			base.Value = tagData != null && tagData.Bool == true;
			break;
		default:
			throw new ArgumentOutOfRangeException("type", type, null);
		}
	}
}

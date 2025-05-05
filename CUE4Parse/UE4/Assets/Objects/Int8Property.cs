using CUE4Parse.UE4.Readers;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Objects;

[JsonConverter(typeof(Int8PropertyConverter))]
public class Int8Property : FPropertyTagType<sbyte>
{
	public Int8Property(FArchive Ar, ReadType type)
	{
		base.Value = (sbyte)((type != ReadType.ZERO) ? Ar.Read<sbyte>() : 0);
	}
}

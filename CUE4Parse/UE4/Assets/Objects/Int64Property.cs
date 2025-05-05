using CUE4Parse.UE4.Readers;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Objects;

[JsonConverter(typeof(Int64PropertyConverter))]
public class Int64Property : FPropertyTagType<long>
{
	public Int64Property(FArchive Ar, ReadType type)
	{
		base.Value = ((type != ReadType.ZERO) ? Ar.Read<long>() : 0);
	}
}

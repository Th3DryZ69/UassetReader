using CUE4Parse.UE4.Readers;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Objects;

[JsonConverter(typeof(Int16PropertyConverter))]
public class Int16Property : FPropertyTagType<short>
{
	public Int16Property(FArchive Ar, ReadType type)
	{
		base.Value = (short)((type != ReadType.ZERO) ? Ar.Read<short>() : 0);
	}
}

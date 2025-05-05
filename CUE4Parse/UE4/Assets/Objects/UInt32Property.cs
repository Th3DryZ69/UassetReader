using CUE4Parse.UE4.Readers;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Objects;

[JsonConverter(typeof(UInt32PropertyConverter))]
public class UInt32Property : FPropertyTagType<uint>
{
	public UInt32Property(FArchive Ar, ReadType type)
	{
		base.Value = ((type != ReadType.ZERO) ? Ar.Read<uint>() : 0u);
	}
}

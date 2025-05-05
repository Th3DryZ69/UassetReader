using CUE4Parse.UE4.Readers;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Objects;

[JsonConverter(typeof(UInt16PropertyConverter))]
public class UInt16Property : FPropertyTagType<ushort>
{
	public UInt16Property(FArchive Ar, ReadType type)
	{
		base.Value = (ushort)((type != ReadType.ZERO) ? Ar.Read<ushort>() : 0);
	}
}

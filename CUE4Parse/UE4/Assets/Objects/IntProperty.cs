using CUE4Parse.UE4.Readers;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Objects;

[JsonConverter(typeof(IntPropertyConverter))]
public class IntProperty : FPropertyTagType<int>
{
	public IntProperty(FArchive Ar, ReadType type)
	{
		base.Value = ((type != ReadType.ZERO) ? Ar.Read<int>() : 0);
	}
}

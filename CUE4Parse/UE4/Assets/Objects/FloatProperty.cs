using CUE4Parse.UE4.Readers;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Objects;

[JsonConverter(typeof(FloatPropertyConverter))]
public class FloatProperty : FPropertyTagType<float>
{
	public FloatProperty(FArchive Ar, ReadType type)
	{
		base.Value = ((type != ReadType.ZERO) ? Ar.Read<float>() : 0f);
	}
}

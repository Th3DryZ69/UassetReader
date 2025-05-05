using CUE4Parse.UE4.Readers;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Objects;

[JsonConverter(typeof(DoublePropertyConverter))]
public class DoubleProperty : FPropertyTagType<double>
{
	public DoubleProperty(FArchive Ar, ReadType type)
	{
		base.Value = ((type != ReadType.ZERO) ? Ar.Read<double>() : 0.0);
	}
}

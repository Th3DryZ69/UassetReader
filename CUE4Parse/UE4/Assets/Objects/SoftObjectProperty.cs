using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.UObject;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Objects;

[JsonConverter(typeof(SoftObjectPropertyConverter))]
public class SoftObjectProperty : FPropertyTagType<FSoftObjectPath>
{
	public SoftObjectProperty(FAssetArchive Ar, ReadType type)
	{
		if (type == ReadType.ZERO)
		{
			base.Value = default(FSoftObjectPath);
		}
		else
		{
			base.Value = new FSoftObjectPath(Ar);
		}
	}
}

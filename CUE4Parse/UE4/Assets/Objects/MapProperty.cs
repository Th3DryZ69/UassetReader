using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Exceptions;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Objects;

[JsonConverter(typeof(MapPropertyConverter))]
public class MapProperty : FPropertyTagType<UScriptMap>
{
	public MapProperty(FAssetArchive Ar, FPropertyTagData? tagData, ReadType type)
	{
		if (type == ReadType.ZERO)
		{
			base.Value = new UScriptMap();
			return;
		}
		if (tagData == null)
		{
			throw new ParserException(Ar, "Can't load MapProperty without tag data");
		}
		base.Value = new UScriptMap(Ar, tagData);
	}
}

using CUE4Parse.MappingsProvider;
using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Assets.Readers;

namespace CUE4Parse.GameTypes.SWJS.Objects;

public class FRsBitfield : FStructFallback
{
	public FRsBitfield(FAssetArchive Ar, string propertyName)
	{
		base.Properties.Add(new FPropertyTag(Ar, new PropertyInfo(-1, propertyName, new PropertyType("StrProperty")), ReadType.NORMAL));
		UObject.DeserializePropertiesTagged(base.Properties, Ar);
	}
}

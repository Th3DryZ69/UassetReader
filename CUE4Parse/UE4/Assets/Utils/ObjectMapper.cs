using CUE4Parse.UE4.Assets.Exports;

namespace CUE4Parse.UE4.Assets.Utils;

public abstract class ObjectMapper
{
	public abstract void Map(IPropertyHolder src, object dst);
}

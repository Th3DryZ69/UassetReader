using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.UObject;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Objects;

[JsonConverter(typeof(InterfacePropertyConverter))]
public class InterfaceProperty : FPropertyTagType<FScriptInterface>
{
	public InterfaceProperty(FAssetArchive Ar, ReadType type)
	{
		base.Value = ((type != ReadType.ZERO) ? new FScriptInterface(Ar) : new FScriptInterface());
	}
}

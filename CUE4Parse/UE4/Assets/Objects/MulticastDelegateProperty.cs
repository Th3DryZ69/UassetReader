using System;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.UObject;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Objects;

[JsonConverter(typeof(MulticastDelegatePropertyConverter))]
public class MulticastDelegateProperty : FPropertyTagType<FMulticastScriptDelegate>
{
	public MulticastDelegateProperty(FAssetArchive Ar, ReadType type)
	{
		base.Value = ((type != ReadType.ZERO) ? new FMulticastScriptDelegate(Ar) : new FMulticastScriptDelegate(Array.Empty<FScriptDelegate>()));
	}
}

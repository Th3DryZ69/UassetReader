using CUE4Parse.UE4.Assets.Readers;

namespace CUE4Parse.UE4.Assets.Objects;

public class MulticastInlineDelegateProperty : MulticastDelegateProperty
{
	public MulticastInlineDelegateProperty(FAssetArchive Ar, ReadType type)
		: base(Ar, type)
	{
	}
}

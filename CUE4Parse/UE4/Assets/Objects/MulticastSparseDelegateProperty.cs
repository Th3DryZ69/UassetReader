using CUE4Parse.UE4.Assets.Readers;

namespace CUE4Parse.UE4.Assets.Objects;

public class MulticastSparseDelegateProperty : MulticastDelegateProperty
{
	public MulticastSparseDelegateProperty(FAssetArchive Ar, ReadType type)
		: base(Ar, type)
	{
	}
}

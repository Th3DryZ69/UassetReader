using CUE4Parse.UE4.Assets.Readers;

namespace CUE4Parse.UE4.Objects.UObject;

public class FFieldPathProperty : FProperty
{
	public FName PropertyClass;

	public override void Deserialize(FAssetArchive Ar)
	{
		base.Deserialize(Ar);
		PropertyClass = Ar.ReadFName();
	}
}

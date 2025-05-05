using CUE4Parse.UE4.Assets.Exports.Nanite;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Assets.Exports.GeometryCollection;

public class UGeometryCollection : UObject
{
	public FGeometryCollection? GeometryCollection { get; private set; }

	public FGeometryCollectionNaniteData? NaniteData { get; private set; }

	public FNaniteResources? OldNaniteData { get; private set; }

	public override void Deserialize(FAssetArchive Ar, long validPos)
	{
		base.Deserialize(Ar, validPos);
		if (FDestructionObjectVersion.Get(Ar) >= FDestructionObjectVersion.Type.GeometryCollectionInDDC)
		{
			Ar.ReadBoolean();
		}
		else
			_ = 0;
		if (FDestructionObjectVersion.Get(Ar) >= FDestructionObjectVersion.Type.GeometryCollectionInDDCAndAsset)
		{
			GeometryCollection = new FGeometryCollection(Ar);
		}
		if (FUE5MainStreamObjectVersion.Get(Ar) == FUE5MainStreamObjectVersion.Type.GeometryCollectionNaniteData || (FUE5MainStreamObjectVersion.Get(Ar) >= FUE5MainStreamObjectVersion.Type.GeometryCollectionNaniteCooked && FUE5MainStreamObjectVersion.Get(Ar) < FUE5MainStreamObjectVersion.Type.GeometryCollectionNaniteTransient))
		{
			OldNaniteData = new FNaniteResources(Ar);
		}
		if (FUE5MainStreamObjectVersion.Get(Ar) >= FUE5MainStreamObjectVersion.Type.GeometryCollectionNaniteTransient && Ar.ReadBoolean())
		{
			NaniteData = new FGeometryCollectionNaniteData(Ar);
		}
	}
}

using CUE4Parse.UE4.Assets.Readers;

namespace CUE4Parse.UE4.Assets.Exports.SkeletalMesh;

public class FRigidVertex : FSoftVertex
{
	public FRigidVertex(FAssetArchive Ar)
		: base(Ar, isRigid: true)
	{
	}
}

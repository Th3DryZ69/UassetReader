using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Objects.Engine;

public class FSkeletalMeshAreaWeightedTriangleSampler : FWeightedRandomSampler
{
	public FSkeletalMeshAreaWeightedTriangleSampler(FArchive Ar)
		: base(Ar)
	{
	}
}

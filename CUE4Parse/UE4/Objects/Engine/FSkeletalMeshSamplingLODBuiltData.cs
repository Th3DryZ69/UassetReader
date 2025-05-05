using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Objects.Engine;

public readonly struct FSkeletalMeshSamplingLODBuiltData : IUStruct
{
	public readonly FSkeletalMeshAreaWeightedTriangleSampler AreaWeightedTriangleSampler;

	public FSkeletalMeshSamplingLODBuiltData(FArchive Ar)
	{
		AreaWeightedTriangleSampler = new FSkeletalMeshAreaWeightedTriangleSampler(Ar);
	}
}

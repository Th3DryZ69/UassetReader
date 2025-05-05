using System;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Objects.Engine;

public readonly struct FSkeletalMeshSamplingRegionBuiltData : IUStruct
{
	public readonly int[] TriangleIndices;

	public readonly int[] Vertices;

	public readonly int[] BoneIndices;

	public readonly FSkeletalMeshAreaWeightedTriangleSampler AreaWeightedSampler;

	public FSkeletalMeshSamplingRegionBuiltData(FArchive Ar)
	{
		TriangleIndices = Ar.ReadArray<int>();
		BoneIndices = Ar.ReadArray<int>();
		AreaWeightedSampler = new FSkeletalMeshAreaWeightedTriangleSampler(Ar);
		Vertices = ((FNiagaraObjectVersion.Get(Ar) >= FNiagaraObjectVersion.Type.SkeletalMeshVertexSampling) ? Ar.ReadArray<int>() : Array.Empty<int>());
	}
}

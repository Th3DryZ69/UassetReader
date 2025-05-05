using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Objects.Engine;

public class FWeightedRandomSampler : IUStruct
{
	public readonly float[] Prob;

	public readonly int[] Alias;

	public readonly float TotalWeight;

	public FWeightedRandomSampler(FArchive Ar)
	{
		Prob = Ar.ReadArray<float>();
		Alias = Ar.ReadArray<int>();
		TotalWeight = Ar.Read<float>();
	}
}

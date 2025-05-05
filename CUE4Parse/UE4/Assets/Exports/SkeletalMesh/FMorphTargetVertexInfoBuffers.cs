using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Readers;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.SkeletalMesh;

public class FMorphTargetVertexInfoBuffers
{
	[JsonIgnore]
	public readonly FMorphTargetVertexInfo[] MorphData;

	public readonly FVector4[] MinimumValuePerMorph;

	public readonly FVector4[] MaximumValuePerMorph;

	public readonly uint[] BatchStartOffsetPerMorph;

	public readonly uint[] BatchesPerMorph;

	public readonly int NumTotalBatches;

	public readonly float PositionPrecision;

	public readonly float TangentZPrecision;

	public FMorphTargetVertexInfoBuffers(FArchive Ar)
	{
		FByteArchive packed = new FByteArchive("PackedMorphData", Ar.ReadArray<byte>(Ar.Read<int>() * 4), Ar.Versions);
		MorphData = packed.ReadArray(NumTotalBatches, () => new FMorphTargetVertexInfo(packed));
		MinimumValuePerMorph = Ar.ReadArray<FVector4>();
		MaximumValuePerMorph = Ar.ReadArray<FVector4>();
		BatchStartOffsetPerMorph = Ar.ReadArray<uint>();
		BatchesPerMorph = Ar.ReadArray<uint>();
		NumTotalBatches = Ar.Read<int>();
		PositionPrecision = Ar.Read<float>();
		TangentZPrecision = Ar.Read<float>();
	}
}

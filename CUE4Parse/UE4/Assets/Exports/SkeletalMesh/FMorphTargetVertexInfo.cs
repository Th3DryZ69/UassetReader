using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Assets.Exports.SkeletalMesh;

public class FMorphTargetVertexInfo
{
	public readonly uint DataOffset;

	public readonly uint IndexBits;

	public readonly uint IndexMin;

	public readonly FVector PositionMin;

	public readonly uint TangentZBits;

	public readonly FVector TangentZMin;

	public FMorphTargetVertexInfo(FArchive Ar)
	{
		DataOffset = Ar.Read<uint>();
		IndexBits = Ar.Read<uint>();
		IndexMin = Ar.Read<uint>();
		Ar.Read<uint>();
		Ar.Read<uint>();
		Ar.Read<uint>();
		TangentZBits = Ar.Read<uint>();
		Ar.Read<uint>();
		Ar.Read<uint>();
		Ar.Read<uint>();
	}
}

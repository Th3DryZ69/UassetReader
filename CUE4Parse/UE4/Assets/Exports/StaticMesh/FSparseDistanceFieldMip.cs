using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Assets.Exports.StaticMesh;

public class FSparseDistanceFieldMip
{
	public FIntVector IndirectionDimensions;

	public int NumDistanceFieldBricks;

	public FVector VolumeToVirtualUVScale;

	public FVector VolumeToVirtualUVAdd;

	public FVector2D DistanceFieldToVolumeScaleBias;

	public uint BulkOffset;

	public uint BulkSize;

	public FSparseDistanceFieldMip(FArchive Ar)
	{
		IndirectionDimensions = Ar.Read<FIntVector>();
		NumDistanceFieldBricks = Ar.Read<int>();
		VolumeToVirtualUVScale = new FVector(Ar);
		VolumeToVirtualUVAdd = new FVector(Ar);
		DistanceFieldToVolumeScaleBias = new FVector2D(Ar);
		BulkOffset = Ar.Read<uint>();
		BulkSize = Ar.Read<uint>();
	}
}

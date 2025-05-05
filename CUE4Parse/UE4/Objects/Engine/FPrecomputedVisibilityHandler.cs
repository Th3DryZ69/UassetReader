using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Core.Math;

namespace CUE4Parse.UE4.Objects.Engine;

public readonly struct FPrecomputedVisibilityHandler : IUStruct
{
	public readonly FVector2D PrecomputedVisibilityCellBucketOriginXY;

	public readonly float PrecomputedVisibilityCellSizeXY;

	public readonly float PrecomputedVisibilityCellSizeZ;

	public readonly int PrecomputedVisibilityCellBucketSizeXY;

	public readonly int PrecomputedVisibilityNumCellBuckets;

	public readonly FPrecomputedVisibilityBucket[] PrecomputedVisibilityCellBuckets;

	public FPrecomputedVisibilityHandler(FAssetArchive Ar)
	{
		PrecomputedVisibilityCellBucketOriginXY = new FVector2D(Ar);
		PrecomputedVisibilityCellSizeXY = Ar.Read<float>();
		PrecomputedVisibilityCellSizeZ = Ar.Read<float>();
		PrecomputedVisibilityCellBucketSizeXY = Ar.Read<int>();
		PrecomputedVisibilityNumCellBuckets = Ar.Read<int>();
		PrecomputedVisibilityCellBuckets = Ar.ReadArray(() => new FPrecomputedVisibilityBucket(Ar));
	}
}

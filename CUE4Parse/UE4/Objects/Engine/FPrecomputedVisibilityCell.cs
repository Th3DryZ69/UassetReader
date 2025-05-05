using CUE4Parse.UE4.Objects.Core.Math;

namespace CUE4Parse.UE4.Objects.Engine;

public readonly struct FPrecomputedVisibilityCell : IUStruct
{
	public readonly FVector Min;

	public readonly ushort ChunkIndex;

	public readonly ushort DataOffset;
}

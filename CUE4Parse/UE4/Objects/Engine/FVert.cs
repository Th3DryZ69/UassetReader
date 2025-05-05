using CUE4Parse.UE4.Objects.Core.Math;

namespace CUE4Parse.UE4.Objects.Engine;

public readonly struct FVert : IUStruct
{
	public readonly int pVertex;

	public readonly int iSide;

	public readonly FVector2D ShadowTexCoord;

	public readonly FVector2D BackfaceShadowTexCoord;
}

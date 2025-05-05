using CUE4Parse.UE4.Objects.Core.Math;

namespace CUE4Parse.UE4.Objects.Engine;

public readonly struct FBspNode : IUStruct
{
	public const int MAX_NODE_VERTICES = 255;

	public const int MAX_ZONES = 64;

	public readonly FPlane Plane;

	public readonly int iVertPool;

	public readonly int iSurf;

	public readonly int iVertexIndex;

	public readonly ushort ComponentIndex;

	public readonly ushort ComponentNodeIndex;

	public readonly int ComponentElementIndex;

	public readonly int iBack;

	public readonly int iFront;

	public readonly int iPlane;

	public readonly int iCollisionBound;

	public readonly byte iZone0;

	public readonly byte iZone1;

	public readonly byte NumVertices;

	public readonly EBspNodeFlags NodeFlags;

	public readonly int iLeaf0;

	public readonly int iLeaf1;
}

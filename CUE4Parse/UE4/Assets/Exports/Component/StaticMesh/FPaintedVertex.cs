using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Objects.RenderCore;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Assets.Exports.Component.StaticMesh;

public struct FPaintedVertex
{
	public FVector Position;

	public FVector4 Normal;

	public FColor Color;

	public FPaintedVertex(FArchive Ar)
	{
		Position = new FVector(Ar);
		if (FRenderingObjectVersion.Get(Ar) < FRenderingObjectVersion.Type.IncreaseNormalPrecision)
		{
			FPackedNormal fPackedNormal = new FPackedNormal(Ar);
			Normal = fPackedNormal;
		}
		else
		{
			Normal = new FVector4(Ar);
		}
		Color = Ar.Read<FColor>();
	}
}

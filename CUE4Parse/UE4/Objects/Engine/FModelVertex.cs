using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Objects.RenderCore;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Objects.Engine;

public struct FModelVertex : IUStruct
{
	public FVector Position;

	public FVector TangentX;

	public FVector4 TangentZ;

	public FVector2D TexCoord;

	public FVector2D ShadowTexCoord;

	public FModelVertex(FArchive Ar)
	{
		Position = Ar.Read<FVector>();
		if (FRenderingObjectVersion.Get(Ar) < FRenderingObjectVersion.Type.IncreaseNormalPrecision)
		{
			TangentX = (FVector)Ar.Read<FDeprecatedSerializedPackedNormal>();
			TangentZ = (FVector4)Ar.Read<FDeprecatedSerializedPackedNormal>();
		}
		else
		{
			TangentX = Ar.Read<FVector>();
			TangentZ = Ar.Read<FVector4>();
		}
		TexCoord = Ar.Read<FVector2D>();
		ShadowTexCoord = Ar.Read<FVector2D>();
	}

	public FVector GetTangentY()
	{
		return ((FVector)TangentZ ^ TangentX) * TangentZ.W;
	}
}

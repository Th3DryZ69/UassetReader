using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Objects.RenderCore;

namespace CUE4Parse.UE4.Objects.Engine;

public struct FDeprecatedModelVertex : IUStruct
{
	public FVector Position;

	public FDeprecatedSerializedPackedNormal TangentX;

	public FDeprecatedSerializedPackedNormal TangentZ;

	public FVector2D TexCoord;

	public FVector2D ShadowTexCoord;

	public static implicit operator FModelVertex(FDeprecatedModelVertex v)
	{
		return new FModelVertex
		{
			Position = v.Position,
			TangentX = (FVector)v.TangentX,
			TangentZ = (FVector4)v.TangentZ,
			TexCoord = v.TexCoord,
			ShadowTexCoord = v.ShadowTexCoord
		};
	}
}

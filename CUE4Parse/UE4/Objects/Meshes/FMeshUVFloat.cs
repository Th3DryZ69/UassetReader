using System.Numerics;
using CUE4Parse.UE4.Writers;
using CUE4Parse.Utils;

namespace CUE4Parse.UE4.Objects.Meshes;

public struct FMeshUVFloat : IUStruct
{
	public float U;

	public float V;

	public FMeshUVFloat(float u, float v)
	{
		U = u;
		V = v;
	}

	public void Serialize(FArchiveWriter Ar)
	{
		Ar.Write(U);
		Ar.Write(V);
	}

	public static implicit operator Vector2(FMeshUVFloat uv)
	{
		return new Vector2(uv.U, uv.V);
	}

	public static explicit operator FMeshUVFloat(FMeshUVHalf uvHalf)
	{
		return new FMeshUVFloat(TypeConversionUtils.HalfToFloat(uvHalf.U), TypeConversionUtils.HalfToFloat(uvHalf.V));
	}
}

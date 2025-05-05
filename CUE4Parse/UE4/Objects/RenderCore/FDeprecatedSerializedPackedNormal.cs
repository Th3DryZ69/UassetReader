using CUE4Parse.UE4.Objects.Core.Math;

namespace CUE4Parse.UE4.Objects.RenderCore;

public struct FDeprecatedSerializedPackedNormal
{
	public uint Data;

	public static FVector4 VectorMultiplyAdd(FVector4 vec1, FVector4 vec2, FVector4 vec3)
	{
		return new FVector4(vec1.X * vec2.X + vec3.X, vec1.Y * vec2.Y + vec3.Y, vec1.Z * vec2.Z + vec3.Z, vec1.W * vec2.W + vec3.W);
	}

	public static explicit operator FVector4(FDeprecatedSerializedPackedNormal packed)
	{
		return VectorMultiplyAdd(new FVector4(packed.Data & 0xFF, (packed.Data >> 8) & 0xFF, (packed.Data >> 16) & 0xFF, (packed.Data >> 24) & 0xFF), new FVector4(0.007843138f), new FVector4(-1f));
	}

	public static explicit operator FVector(FDeprecatedSerializedPackedNormal packed)
	{
		return (FVector)(FVector4)packed;
	}
}

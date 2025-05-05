using System.Numerics;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace CUE4Parse.UE4.Objects.Core.Math;

public static class UnrealMathSSE
{
	public static readonly Vector128<float> QMULTI_SIGN_MASK0 = Vector128.Create(1f, -1f, 1f, -1f);

	public static readonly Vector128<float> QMULTI_SIGN_MASK1 = Vector128.Create(1f, 1f, -1f, -1f);

	public static readonly Vector128<float> QMULTI_SIGN_MASK2 = Vector128.Create(-1f, 1f, 1f, -1f);

	public static byte ShuffleMask(byte A0, byte A1, byte B2, byte B3)
	{
		return (byte)(A0 | (A1 << 2) | (B2 << 4) | (B3 << 6));
	}

	public static Vector128<float> VectorReplicate(Vector128<float> vec, byte elementIndex)
	{
		return Sse.Shuffle(vec, vec, ShuffleMask(elementIndex, elementIndex, elementIndex, elementIndex));
	}

	public static Vector128<float> VectorMultiply(Vector128<float> vec1, Vector128<float> vec2)
	{
		return Sse.Multiply(vec1, vec2);
	}

	public static Vector128<float> VectorSwizzle(Vector128<float> vec, byte x, byte y, byte z, byte w)
	{
		return Sse.Shuffle(vec, vec, ShuffleMask(x, y, z, w));
	}

	public static Vector128<float> VectorMultiplyAdd(Vector128<float> vec1, Vector128<float> vec2, Vector128<float> vec3)
	{
		return Sse.Add(Sse.Multiply(vec1, vec2), vec3);
	}

	public static FQuat VectorQuaternionMultiply2(FQuat quat1, FQuat quat2)
	{
		Vector128<float> vec = FQuat.AsVector128(quat1);
		Vector128<float> vector = FQuat.AsVector128(quat2);
		Vector128<float> value = VectorMultiplyAdd(vec3: VectorMultiplyAdd(vec3: VectorMultiplyAdd(vec3: VectorMultiply(VectorReplicate(vec, 3), vector), vec1: VectorMultiply(VectorReplicate(vec, 0), VectorSwizzle(vector, 3, 2, 1, 0)), vec2: QMULTI_SIGN_MASK0), vec1: VectorMultiply(VectorReplicate(vec, 1), VectorSwizzle(vector, 2, 3, 0, 1)), vec2: QMULTI_SIGN_MASK1), vec1: VectorMultiply(VectorReplicate(vec, 2), VectorSwizzle(vector, 1, 0, 3, 2)), vec2: QMULTI_SIGN_MASK2);
		Vector4 vector2 = value.AsVector4();
		return new FQuat(vector2.X, vector2.Y, vector2.Z, vector2.W);
	}
}

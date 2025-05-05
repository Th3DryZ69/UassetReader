using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;
using CUE4Parse.UE4.Writers;
using CUE4Parse.Utils;

namespace CUE4Parse.UE4.Objects.Core.Math;

public struct FQuat : IUStruct
{
	public const float THRESH_QUAT_NORMALIZED = 0.01f;

	public static readonly FQuat Identity = new FQuat(0f, 0f, 0f, 1f);

	public float X;

	public float Y;

	public float Z;

	public float W;

	private static int[] matrixNxt = new int[3] { 1, 2, 0 };

	public bool IsNormalized => MathF.Abs(1f - SizeSquared) < 0.01f;

	public float Size => MathF.Sqrt(SizeSquared);

	public float SizeSquared => X * X + Y * Y + Z * Z + W * W;

	public FQuat(EForceInit zeroOrNot)
	{
		X = 0f;
		Y = 0f;
		Z = 0f;
		W = ((zeroOrNot != EForceInit.ForceInitToZero) ? 1 : 0);
	}

	public FQuat(float x, float y, float z, float w)
	{
		X = x;
		Y = y;
		Z = z;
		W = w;
	}

	public FQuat(FArchive Ar)
	{
		if (Ar.Ver >= EUnrealEngineObjectUE5Version.LARGE_WORLD_COORDINATES)
		{
			X = (float)Ar.Read<double>();
			Y = (float)Ar.Read<double>();
			Z = (float)Ar.Read<double>();
			W = (float)Ar.Read<double>();
		}
		else
		{
			X = Ar.Read<float>();
			Y = Ar.Read<float>();
			Z = Ar.Read<float>();
			W = Ar.Read<float>();
		}
	}

	public FQuat(FMatrix m)
	{
		if (m.GetScaledAxis(EAxis.X).IsNearlyZero() || m.GetScaledAxis(EAxis.Y).IsNearlyZero() || m.GetScaledAxis(EAxis.Z).IsNearlyZero())
		{
			FQuat identity = Identity;
			X = identity.X;
			Y = identity.Y;
			Z = identity.Z;
			W = identity.W;
			return;
		}
		float num = m.M00 + m.M11 + m.M22;
		float num3;
		if (num > 0f)
		{
			float num2 = 1f / MathF.Sqrt(num + 1f);
			W = 0.5f * (1f / num2);
			num3 = 0.5f * num2;
			X = (m.M12 - m.M21) * num3;
			Y = (m.M20 - m.M02) * num3;
			Z = (m.M01 - m.M10) * num3;
			return;
		}
		int num4 = 0;
		if (m.M11 > m.M00)
		{
			num4 = 1;
		}
		if (m.M22 > m[4 * num4 + num4])
		{
			num4 = 2;
		}
		int num5 = matrixNxt[num4];
		int num6 = matrixNxt[num5];
		num3 = m[4 * num4 + num4] - m[4 * num5 + num5] - m[4 * num6 + num6] + 1f;
		float num7 = 1f / MathF.Sqrt(num3);
		Span<float> span = stackalloc float[4];
		span[num4] = 0.5f * (1f / num7);
		num3 = 0.5f * num7;
		span[3] = (m[4 * num5 + num6] - m[4 * num6 + num5]) * num3;
		span[num5] = (m[4 * num4 + num5] + m[4 * num5 + num4]) * num3;
		span[num6] = (m[4 * num4 + num6] + m[4 * num6 + num4]) * num3;
		X = span[0];
		Y = span[1];
		Z = span[2];
		W = span[3];
	}

	public FQuat(FRotator rotator)
	{
		FQuat fQuat = rotator.Quaternion();
		X = fQuat.X;
		Y = fQuat.Y;
		Z = fQuat.Z;
		W = fQuat.W;
	}

	public FQuat(FVector axis, float angleRad)
	{
		float x = 0.5f * angleRad;
		float num = MathF.Sin(x);
		float w = MathF.Cos(x);
		X = num * axis.X;
		Y = num * axis.Y;
		Z = num * axis.Z;
		W = w;
	}

	public bool Equals(FQuat q, float tolerance)
	{
		if (!(MathF.Abs(X - q.X) <= tolerance) || !(MathF.Abs(Y - q.Y) <= tolerance) || !(MathF.Abs(Z - q.Z) <= tolerance) || !(MathF.Abs(W - q.W) <= tolerance))
		{
			if (MathF.Abs(X + q.X) <= tolerance && MathF.Abs(Y + q.Y) <= tolerance && MathF.Abs(Z + q.Z) <= tolerance)
			{
				return MathF.Abs(W + q.W) <= tolerance;
			}
			return false;
		}
		return true;
	}

	public bool IsIdentity(float tolerance = 1E-08f)
	{
		return Equals(Identity, tolerance);
	}

	public static Vector128<float> AsVector128(FQuat value)
	{
		return Unsafe.As<FQuat, Vector128<float>>(ref value);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FQuat operator *(FQuat a, FQuat b)
	{
		if (Sse.IsSupported)
		{
			return UnrealMathSSE.VectorQuaternionMultiply2(a, b);
		}
		FQuat result = default(FQuat);
		float num = (a.Z - a.Y) * (b.Y - b.Z);
		float num2 = (a.W + a.X) * (b.W + b.X);
		float num3 = (a.W - a.X) * (b.Y + b.Z);
		float num4 = (a.Y + a.Z) * (b.W - b.X);
		float num5 = (a.Z - a.X) * (b.X - b.Y);
		float num6 = (a.Z + a.X) * (b.X + b.Y);
		float num7 = (a.W + a.Y) * (b.W - b.Z);
		float num8 = (a.W - a.Y) * (b.W + b.Z);
		float num9 = num6 + num7 + num8;
		float num10 = 0.5f * (num5 + num9);
		result.X = num2 + num10 - num9;
		result.Y = num3 + num10 - num8;
		result.Z = num4 + num10 - num7;
		result.W = num + num10 - num6;
		return result;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FVector operator *(FQuat a, FVector b)
	{
		return a.RotateVector(b);
	}

	public static bool operator ==(FQuat a, FQuat b)
	{
		if (a.X == b.X && a.Y == b.Y && a.Z == b.Z)
		{
			return a.W == b.W;
		}
		return false;
	}

	public static bool operator !=(FQuat a, FQuat b)
	{
		return !(a == b);
	}

	public void Normalize(float tolerance = 1E-08f)
	{
		float num = X * X + Y * Y + Z * Z + W * W;
		if (num >= tolerance)
		{
			float num2 = num.InvSqrt();
			X *= num2;
			Y *= num2;
			Z *= num2;
			W *= num2;
		}
		else
		{
			FQuat identity = Identity;
			X = identity.X;
			Y = identity.Y;
			Z = identity.Z;
			W = identity.W;
		}
	}

	public FQuat GetNormalized(float tolerance = 1E-08f)
	{
		FQuat result = this;
		result.Normalize(tolerance);
		return result;
	}

	public FVector RotateVector(FVector v)
	{
		FVector a = new FVector(X, Y, Z);
		FVector fVector = 2f * FVector.CrossProduct(a, v);
		return v + W * fVector + FVector.CrossProduct(a, fVector);
	}

	public FVector UnrotateVector(FVector v)
	{
		FVector a = new FVector(0f - X, 0f - Y, 0f - Z);
		FVector fVector = 2f * FVector.CrossProduct(a, v);
		return v + W * fVector + FVector.CrossProduct(a, fVector);
	}

	public FQuat Inverse()
	{
		if (!IsNormalized)
		{
			throw new ArgumentException("Quat must be normalized in order to be inversed");
		}
		return new FQuat(0f - X, 0f - Y, 0f - Z, W);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Conjugate()
	{
		X = 0f - X;
		Y = 0f - Y;
		Z = 0f - Z;
	}

	public static FQuat Conjugate(FQuat quat)
	{
		return new FQuat(0f - quat.X, 0f - quat.Y, 0f - quat.Z, quat.W);
	}

	public static FQuat FindBetweenNormals(FVector a, FVector b, float normAb = 1f)
	{
		float num = normAb + FVector.DotProduct(a, b);
		FQuat result;
		if (num >= 1E-06f * normAb)
		{
			result = new FQuat(a.Y * b.Z - a.Z * b.Y, a.Z * b.X - a.X * b.Z, a.X * b.Y - a.Y * b.X, num);
		}
		else
		{
			num = 0f;
			result = ((MathF.Abs(a.X) > MathF.Abs(a.Y)) ? new FQuat(0f - a.Z, 0f, a.X, num) : new FQuat(0f, 0f - a.Z, a.Y, num));
		}
		result.Normalize();
		return result;
	}

	public FRotator Rotator()
	{
		float num = Z * X - W * Y;
		float y = 2f * (W * Z + X * Y);
		float x = 1f - 2f * (Y * Y + Z * Z);
		FRotator fRotator = new FRotator();
		if (num < -0.4999995f)
		{
			fRotator.Pitch = -90f;
			fRotator.Yaw = MathF.Atan2(y, x) * (180f / (float)System.Math.PI);
			fRotator.Roll = FRotator.NormalizeAxis(0f - fRotator.Yaw - 2f * MathF.Atan2(X, W) * (180f / (float)System.Math.PI));
		}
		else if (num > 0.4999995f)
		{
			fRotator.Pitch = 90f;
			fRotator.Yaw = MathF.Atan2(y, x) * (180f / (float)System.Math.PI);
			fRotator.Roll = FRotator.NormalizeAxis(fRotator.Yaw - 2f * MathF.Atan2(X, W) * (180f / (float)System.Math.PI));
		}
		else
		{
			fRotator.Pitch = MathF.Asin(2f * num) * (180f / (float)System.Math.PI);
			fRotator.Yaw = MathF.Atan2(y, x) * (180f / (float)System.Math.PI);
			fRotator.Roll = MathF.Atan2(-2f * (W * X + Y * Z), 1f - 2f * (X * X + Y * Y)) * (180f / (float)System.Math.PI);
		}
		return fRotator;
	}

	public bool ContainsNaN()
	{
		if (float.IsFinite(X) && float.IsFinite(Y) && float.IsFinite(Z))
		{
			return !float.IsFinite(W);
		}
		return true;
	}

	public override string ToString()
	{
		return $"X={X:F9} Y={Y:F9} Z={Z:F9} W={W:F9}";
	}

	public void Serialize(FArchiveWriter Ar)
	{
		Ar.Write(X);
		Ar.Write(Y);
		Ar.Write(Z);
		Ar.Write(W);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FQuat FastLerp(FQuat q1, FQuat q2, float alpha)
	{
		float num = MathUtils.FloatSelect(q1 | q2, 1f, -1f);
		return q2 * alpha + q1 * (num * (1f - alpha));
	}

	public static FQuat Slerp_NotNormalized(FQuat quat1, FQuat quat2, float Slerp)
	{
		float num = quat1.X * quat2.X + quat1.Y * quat2.Y + quat1.Z * quat2.Z + quat1.W * quat2.W;
		float num2 = MathUtils.FloatSelect(num, num, 0f - num);
		float num5;
		float num6;
		if (num2 < 0.9999f)
		{
			float num3 = MathF.Acos(num2);
			float num4 = 1f / MathF.Sin(num3);
			num5 = MathF.Sin((1f - Slerp) * num3) * num4;
			num6 = MathF.Sin(Slerp * num3) * num4;
		}
		else
		{
			num5 = 1f - Slerp;
			num6 = Slerp;
		}
		num6 = MathUtils.FloatSelect(num, num6, 0f - num6);
		return new FQuat
		{
			X = num5 * quat1.X + num6 * quat2.X,
			Y = num5 * quat1.Y + num6 * quat2.Y,
			Z = num5 * quat1.Z + num6 * quat2.Z,
			W = num5 * quat1.W + num6 * quat2.W
		};
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FQuat Slerp(FQuat quat1, FQuat quat2, float slerp)
	{
		return Slerp_NotNormalized(quat1, quat2, slerp).GetNormalized();
	}

	public static float operator |(FQuat a, FQuat b)
	{
		return a.X * b.X + a.Y * b.Y + a.Z * b.Z + a.W * b.W;
	}

	public static FQuat operator *(FQuat a, float scale)
	{
		return new FQuat(scale * a.X, scale * a.Y, scale * a.Z, scale * a.W);
	}

	public static FQuat operator +(FQuat a, FQuat b)
	{
		return new FQuat(a.X + b.X, a.Y + b.Y, a.Z + b.Z, a.W + b.W);
	}

	public static implicit operator Quaternion(FQuat v)
	{
		return new Quaternion(v.X, v.Y, v.Z, v.W);
	}
}

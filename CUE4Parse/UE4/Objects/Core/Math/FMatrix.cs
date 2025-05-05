using System;
using System.Runtime.CompilerServices;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;
using CUE4Parse.Utils;

namespace CUE4Parse.UE4.Objects.Core.Math;

public class FMatrix : IUStruct
{
	public float M00;

	public float M01;

	public float M02;

	public float M03;

	public float M10;

	public float M11;

	public float M12;

	public float M13;

	public float M20;

	public float M21;

	public float M22;

	public float M23;

	public float M30;

	public float M31;

	public float M32;

	public float M33;

	public static FMatrix Identity => new FMatrix(1f, 0f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 0f, 1f);

	public float this[int i]
	{
		get
		{
			return i switch
			{
				0 => M00, 
				1 => M01, 
				2 => M02, 
				3 => M03, 
				4 => M10, 
				5 => M11, 
				6 => M12, 
				7 => M13, 
				8 => M20, 
				9 => M21, 
				10 => M22, 
				11 => M23, 
				12 => M30, 
				13 => M31, 
				14 => M32, 
				15 => M33, 
				_ => throw new IndexOutOfRangeException(), 
			};
		}
		set
		{
			switch (i)
			{
			case 0:
				M00 = value;
				break;
			case 1:
				M01 = value;
				break;
			case 2:
				M02 = value;
				break;
			case 3:
				M03 = value;
				break;
			case 4:
				M10 = value;
				break;
			case 5:
				M11 = value;
				break;
			case 6:
				M12 = value;
				break;
			case 7:
				M13 = value;
				break;
			case 8:
				M20 = value;
				break;
			case 9:
				M21 = value;
				break;
			case 10:
				M22 = value;
				break;
			case 11:
				M23 = value;
				break;
			case 12:
				M30 = value;
				break;
			case 13:
				M31 = value;
				break;
			case 14:
				M32 = value;
				break;
			case 15:
				M33 = value;
				break;
			default:
				throw new IndexOutOfRangeException();
			}
		}
	}

	public FMatrix()
	{
	}

	public FMatrix(FMatrix m)
	{
		M00 = m.M00;
		M01 = m.M01;
		M02 = m.M02;
		M03 = m.M03;
		M10 = m.M10;
		M11 = m.M11;
		M12 = m.M12;
		M13 = m.M13;
		M20 = m.M20;
		M21 = m.M21;
		M22 = m.M22;
		M23 = m.M23;
		M30 = m.M30;
		M31 = m.M31;
		M32 = m.M32;
		M33 = m.M33;
	}

	public FMatrix(float m00, float m01, float m02, float m03, float m10, float m11, float m12, float m13, float m20, float m21, float m22, float m23, float m30, float m31, float m32, float m33)
	{
		M00 = m00;
		M01 = m01;
		M02 = m02;
		M03 = m03;
		M10 = m10;
		M11 = m11;
		M12 = m12;
		M13 = m13;
		M20 = m20;
		M21 = m21;
		M22 = m22;
		M23 = m23;
		M30 = m30;
		M31 = m31;
		M32 = m32;
		M33 = m33;
	}

	public FMatrix(FArchive Ar)
	{
		if (Ar.Ver >= EUnrealEngineObjectUE5Version.LARGE_WORLD_COORDINATES)
		{
			M00 = (float)Ar.Read<double>();
			M01 = (float)Ar.Read<double>();
			M02 = (float)Ar.Read<double>();
			M03 = (float)Ar.Read<double>();
			M10 = (float)Ar.Read<double>();
			M11 = (float)Ar.Read<double>();
			M12 = (float)Ar.Read<double>();
			M13 = (float)Ar.Read<double>();
			M20 = (float)Ar.Read<double>();
			M21 = (float)Ar.Read<double>();
			M22 = (float)Ar.Read<double>();
			M23 = (float)Ar.Read<double>();
			M30 = (float)Ar.Read<double>();
			M31 = (float)Ar.Read<double>();
			M32 = (float)Ar.Read<double>();
			M33 = (float)Ar.Read<double>();
		}
		else
		{
			M00 = Ar.Read<float>();
			M01 = Ar.Read<float>();
			M02 = Ar.Read<float>();
			M03 = Ar.Read<float>();
			M10 = Ar.Read<float>();
			M11 = Ar.Read<float>();
			M12 = Ar.Read<float>();
			M13 = Ar.Read<float>();
			M20 = Ar.Read<float>();
			M21 = Ar.Read<float>();
			M22 = Ar.Read<float>();
			M23 = Ar.Read<float>();
			M30 = Ar.Read<float>();
			M31 = Ar.Read<float>();
			M32 = Ar.Read<float>();
			M33 = Ar.Read<float>();
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FMatrix operator *(FMatrix a, FMatrix b)
	{
		return new FMatrix(a.M00 * b.M00 + a.M01 * b.M10 + a.M02 * b.M20 + a.M03 * b.M30, a.M00 * b.M01 + a.M01 * b.M11 + a.M02 * b.M21 + a.M03 * b.M31, a.M00 * b.M02 + a.M01 * b.M12 + a.M02 * b.M22 + a.M03 * b.M32, a.M00 * b.M03 + a.M01 * b.M13 + a.M02 * b.M23 + a.M03 * b.M33, a.M10 * b.M00 + a.M11 * b.M10 + a.M12 * b.M20 + a.M13 * b.M30, a.M10 * b.M01 + a.M11 * b.M11 + a.M12 * b.M21 + a.M13 * b.M31, a.M10 * b.M02 + a.M11 * b.M12 + a.M12 * b.M22 + a.M13 * b.M32, a.M10 * b.M03 + a.M11 * b.M13 + a.M12 * b.M23 + a.M13 * b.M33, a.M20 * b.M00 + a.M21 * b.M10 + a.M22 * b.M20 + a.M23 * b.M30, a.M20 * b.M01 + a.M21 * b.M11 + a.M22 * b.M21 + a.M23 * b.M31, a.M20 * b.M02 + a.M21 * b.M12 + a.M22 * b.M22 + a.M23 * b.M32, a.M20 * b.M03 + a.M21 * b.M13 + a.M22 * b.M23 + a.M23 * b.M33, a.M30 * b.M00 + a.M31 * b.M10 + a.M32 * b.M20 + a.M33 * b.M30, a.M30 * b.M01 + a.M31 * b.M11 + a.M32 * b.M21 + a.M33 * b.M31, a.M30 * b.M02 + a.M31 * b.M12 + a.M32 * b.M22 + a.M33 * b.M32, a.M30 * b.M03 + a.M31 * b.M13 + a.M32 * b.M23 + a.M33 * b.M33);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public FVector4 TransformFVector4(FVector4 p)
	{
		return new FVector4(p.X * M00 + p.Y * M10 + p.Z * M20 + p.W * M30, p.X * M01 + p.Y * M11 + p.Z * M21 + p.W * M31, p.X * M02 + p.Y * M12 + p.Z * M22 + p.W * M32, p.X * M03 + p.Y * M13 + p.Z * M23 + p.W * M33);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public FVector4 TransformPosition(FVector v)
	{
		return TransformFVector4(new FVector4(v.X, v.Y, v.Z, 1f));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public FVector InverseTransformPosition(FVector v)
	{
		return (FVector)InverseFast().TransformPosition(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public FVector4 TransformVector(FVector v)
	{
		return TransformFVector4(new FVector4(v.X, v.Y, v.Z, 0f));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public FMatrix GetTransposed()
	{
		return new FMatrix(M00, M10, M20, M30, M01, M11, M21, M31, M02, M12, M22, M32, M03, M13, M23, M33);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float Determinant()
	{
		return M00 * (M11 * (M22 * M33 - M23 * M32) - M21 * (M12 * M33 - M13 * M32) + M31 * (M12 * M23 - M13 * M22)) - M10 * (M01 * (M22 * M33 - M23 * M32) - M21 * (M02 * M33 - M03 * M32) + M31 * (M02 * M23 - M03 * M22)) + M20 * (M01 * (M12 * M33 - M13 * M32) - M11 * (M02 * M33 - M03 * M32) + M31 * (M02 * M13 - M03 * M12)) - M30 * (M01 * (M12 * M23 - M13 * M22) - M11 * (M02 * M23 - M03 * M22) + M21 * (M02 * M13 - M03 * M12));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float RotDeterminant()
	{
		return M00 * (M11 * M22 - M12 * M21) - M10 * (M01 * M22 - M02 * M21) + M20 * (M01 * M12 - M02 * M11);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public FMatrix InverseFast()
	{
		FMatrix fMatrix = new FMatrix();
		float[] array = new float[4];
		FMatrix fMatrix2 = new FMatrix
		{
			M00 = M22 * M33 - M23 * M32,
			M01 = M12 * M33 - M13 * M32,
			M02 = M12 * M23 - M13 * M22,
			M10 = M22 * M33 - M23 * M32,
			M11 = M02 * M33 - M03 * M32,
			M12 = M02 * M23 - M03 * M22,
			M20 = M12 * M33 - M13 * M32,
			M21 = M02 * M33 - M03 * M32,
			M22 = M02 * M13 - M03 * M12,
			M30 = M12 * M23 - M13 * M22,
			M31 = M02 * M23 - M03 * M22,
			M32 = M02 * M13 - M03 * M12
		};
		array[0] = M11 * fMatrix2.M00 - M21 * fMatrix2.M01 + M31 * fMatrix2.M02;
		array[1] = M01 * fMatrix2.M10 - M21 * fMatrix2.M11 + M31 * fMatrix2.M12;
		array[2] = M01 * fMatrix2.M20 - M11 * fMatrix2.M21 + M31 * fMatrix2.M22;
		array[3] = M01 * fMatrix2.M30 - M11 * fMatrix2.M31 + M21 * fMatrix2.M32;
		float num = M00 * array[0] - M10 * array[1] + M20 * array[2] - M30 * array[3];
		float num2 = 1f / num;
		fMatrix.M00 = num2 * array[0];
		fMatrix.M01 = (0f - num2) * array[1];
		fMatrix.M02 = num2 * array[2];
		fMatrix.M03 = (0f - num2) * array[3];
		fMatrix.M10 = (0f - num2) * (M10 * fMatrix2.M00 - M20 * fMatrix2.M01 + M30 * fMatrix2.M02);
		fMatrix.M11 = num2 * (M00 * fMatrix2.M10 - M20 * fMatrix2.M11 + M30 * fMatrix2.M12);
		fMatrix.M12 = (0f - num2) * (M00 * fMatrix2.M20 - M10 * fMatrix2.M21 + M30 * fMatrix2.M22);
		fMatrix.M13 = num2 * (M00 * fMatrix2.M30 - M10 * fMatrix2.M31 + M20 * fMatrix2.M32);
		fMatrix.M20 = num2 * (M10 * (M21 * M33 - M23 * M31) - M20 * (M11 * M33 - M13 * M31) + M30 * (M11 * M23 - M13 * M21));
		fMatrix.M21 = (0f - num2) * (M00 * (M21 * M33 - M23 * M31) - M20 * (M01 * M33 - M03 * M31) + M30 * (M01 * M23 - M03 * M21));
		fMatrix.M22 = num2 * (M00 * (M11 * M33 - M13 * M31) - M10 * (M01 * M33 - M03 * M31) + M30 * (M01 * M13 - M03 * M11));
		fMatrix.M23 = (0f - num2) * (M00 * (M11 * M23 - M13 * M21) - M10 * (M01 * M23 - M03 * M21) + M20 * (M01 * M13 - M03 * M11));
		fMatrix.M30 = (0f - num2) * (M10 * (M21 * M32 - M22 * M31) - M20 * (M11 * M32 - M12 * M31) + M30 * (M11 * M22 - M12 * M21));
		fMatrix.M31 = num2 * (M00 * (M21 * M32 - M22 * M31) - M20 * (M01 * M32 - M02 * M31) + M30 * (M01 * M22 - M02 * M21));
		fMatrix.M32 = (0f - num2) * (M00 * (M11 * M32 - M12 * M31) - M10 * (M01 * M32 - M02 * M31) + M30 * (M01 * M12 - M02 * M11));
		fMatrix.M33 = num2 * (M00 * (M11 * M22 - M12 * M21) - M10 * (M01 * M22 - M02 * M21) + M20 * (M01 * M12 - M02 * M11));
		return fMatrix;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public FMatrix Inverse()
	{
		if (GetScaledAxis(EAxis.X).IsNearlyZero(1E-08f) && GetScaledAxis(EAxis.Y).IsNearlyZero(1E-08f) && GetScaledAxis(EAxis.Z).IsNearlyZero(1E-08f))
		{
			return Identity;
		}
		if (Determinant() != 0f)
		{
			return InverseFast();
		}
		return Identity;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void RemoveScaling(float tolerance = 1E-08f)
	{
		float num = M00 * M00 + M01 * M01 + M02 * M02;
		float num2 = M10 * M10 + M11 * M11 + M12 * M12;
		float num3 = M20 * M20 + M21 * M21 + M22 * M22;
		float num4 = ((num - tolerance >= 0f) ? num.InvSqrt() : 1f);
		float num5 = ((num2 - tolerance >= 0f) ? num2.InvSqrt() : 1f);
		float num6 = ((num3 - tolerance >= 0f) ? num3.InvSqrt() : 1f);
		M00 *= num4;
		M01 *= num4;
		M02 *= num4;
		M10 *= num5;
		M11 *= num5;
		M12 *= num5;
		M20 *= num6;
		M21 *= num6;
		M22 *= num6;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public FVector ExtractScaling(float tolerance = 1E-08f)
	{
		float num = M00 * M00 + M01 * M01 + M02 * M02;
		float num2 = M10 * M10 + M11 * M11 + M12 * M12;
		float num3 = M20 * M20 + M21 * M21 + M22 * M22;
		FVector result = default(FVector);
		if (num > tolerance)
		{
			float num4 = 1f / (result.X = MathF.Sqrt(num));
			M00 *= num4;
			M01 *= num4;
			M02 *= num4;
		}
		else
		{
			result.X = 0f;
		}
		if (num2 > tolerance)
		{
			float num5 = 1f / (result.Y = MathF.Sqrt(num2));
			M10 *= num5;
			M11 *= num5;
			M12 *= num5;
		}
		else
		{
			result.Y = 0f;
		}
		if (num3 > tolerance)
		{
			float num6 = 1f / (result.Z = MathF.Sqrt(num3));
			M20 *= num6;
			M21 *= num6;
			M22 *= num6;
		}
		else
		{
			result.Z = 0f;
		}
		return result;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float GetMaximumAxisScale()
	{
		return MathF.Sqrt(MathF.Max(GetScaledAxis(EAxis.X).SizeSquared(), MathF.Max(GetScaledAxis(EAxis.Y).SizeSquared(), GetScaledAxis(EAxis.Z).SizeSquared())));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public FVector GetOrigin()
	{
		return new FVector(M30, M31, M32);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public FVector GetScaledAxis(EAxis axis)
	{
		return axis switch
		{
			EAxis.X => new FVector(M00, M01, M02), 
			EAxis.Y => new FVector(M10, M11, M12), 
			EAxis.Z => new FVector(M20, M21, M22), 
			_ => FVector.ZeroVector, 
		};
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void SetAxis(int i, FVector axis)
	{
		switch (i)
		{
		case 0:
			M00 = axis.X;
			M01 = axis.Y;
			M02 = axis.Z;
			break;
		case 1:
			M10 = axis.X;
			M11 = axis.Y;
			M12 = axis.Z;
			break;
		case 2:
			M20 = axis.X;
			M21 = axis.Y;
			M22 = axis.Z;
			break;
		case 3:
			M30 = axis.X;
			M31 = axis.Y;
			M32 = axis.Z;
			break;
		}
	}

	public FRotator Rotator()
	{
		FVector scaledAxis = GetScaledAxis(EAxis.X);
		FVector scaledAxis2 = GetScaledAxis(EAxis.Y);
		FVector scaledAxis3 = GetScaledAxis(EAxis.Z);
		FRotator fRotator = new FRotator(MathF.Atan2(scaledAxis.Z, MathF.Sqrt(scaledAxis.X * scaledAxis.X + scaledAxis.Y * scaledAxis.Y)) * 180f / (float)System.Math.PI, MathF.Atan2(scaledAxis.Y, scaledAxis.X) * 180f / (float)System.Math.PI, 0f);
		FVector scaledAxis4 = new FRotationMatrix(fRotator).GetScaledAxis(EAxis.Y);
		fRotator.Roll = MathF.Atan2(scaledAxis3 | scaledAxis4, scaledAxis2 | scaledAxis4) * 180f / (float)System.Math.PI;
		return fRotator;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public FVector GetScaleVector(float tolerance = 1E-08f)
	{
		FVector result = new FVector(1f, 1f, 1f);
		float num = M00 * M00 + M01 * M01 + M02 * M02;
		result[0] = ((num > tolerance) ? MathF.Sqrt(num) : 0f);
		float num2 = M10 * M10 + M11 * M11 + M12 * M12;
		result[1] = ((num2 > tolerance) ? MathF.Sqrt(num2) : 0f);
		float num3 = M20 * M20 + M21 * M21 + M22 * M22;
		result[2] = ((num3 > tolerance) ? MathF.Sqrt(num3) : 0f);
		return result;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public FQuat ToQuat()
	{
		return new FQuat(this);
	}

	public override string ToString()
	{
		return $"[{M00:F1} {M01:F1} {M02:F1} {M03:F1}] [{M10:F1} {M11:F1} {M12:F1} {M13:F1}] [{M20:F1} {M21:F1} {M22:F1} {M23:F1}] [{M30:F1} {M31:F1} {M32:F1} {M33:F1}]";
	}
}

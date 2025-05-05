using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;
using CUE4Parse.UE4.Writers;
using CUE4Parse.Utils;

namespace CUE4Parse.UE4.Objects.Core.Math;

public struct FVector : IUStruct
{
	public const float ThreshVectorNormalized = 0.01f;

	public static readonly FVector ZeroVector = new FVector(0f, 0f, 0f);

	public static readonly FVector OneVector = new FVector(1f, 1f, 1f);

	public static readonly FVector UpVector = new FVector(0f, 0f, 1f);

	public static readonly FVector ForwardVector = new FVector(1f, 0f, 0f);

	public static readonly FVector RightVector = new FVector(0f, 1f, 0f);

	public float X;

	public float Y;

	public float Z;

	public float this[int i]
	{
		get
		{
			return i switch
			{
				0 => X, 
				1 => Y, 
				2 => Z, 
				_ => throw new IndexOutOfRangeException(), 
			};
		}
		set
		{
			switch (i)
			{
			case 0:
				X = value;
				break;
			case 1:
				Y = value;
				break;
			case 2:
				Z = value;
				break;
			default:
				throw new IndexOutOfRangeException();
			}
		}
	}

	public FVector(float x, float y, float z)
	{
		X = x;
		Y = y;
		Z = z;
	}

	public FVector(FArchive Ar)
	{
		if (Ar.Ver >= EUnrealEngineObjectUE5Version.LARGE_WORLD_COORDINATES)
		{
			X = (float)Ar.Read<double>();
			Y = (float)Ar.Read<double>();
			Z = (float)Ar.Read<double>();
		}
		else
		{
			X = Ar.Read<float>();
			Y = Ar.Read<float>();
			Z = Ar.Read<float>();
		}
	}

	public FVector(float f)
		: this(f, f, f)
	{
	}

	public FVector(FVector2D v, float z)
		: this(v.X, v.Y, z)
	{
	}

	public FVector(FVector4 v)
		: this(v.X, v.Y, v.Z)
	{
	}

	public FVector(FLinearColor color)
		: this(color.R, color.G, color.B)
	{
	}

	public FVector(FIntVector v)
		: this(v.X, v.Y, v.Z)
	{
	}

	public FVector(FIntPoint p)
		: this(p.X, p.Y, 0f)
	{
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public FVector Set(FVector other)
	{
		X = other.X;
		Y = other.Y;
		Z = other.Z;
		return this;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public FVector Set(float x, float y, float z)
	{
		X = x;
		Y = y;
		Z = z;
		return this;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public FVector GetSignVector()
	{
		return new FVector
		{
			X = ((X >= 0f) ? 1 : (-1)),
			Y = ((Y >= 0f) ? 1 : (-1)),
			Z = ((Z >= 0f) ? 1 : (-1))
		};
	}

	public void Scale(float scale)
	{
		X *= scale;
		Y *= scale;
		Z *= scale;
	}

	public void Scale(FVector scale)
	{
		X *= scale.X;
		Y *= scale.Y;
		Z *= scale.Z;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FVector operator +(FVector a)
	{
		return a;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FVector operator -(FVector a)
	{
		return new FVector(0f - a.X, 0f - a.Y, 0f - a.Z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FVector operator ^(FVector a, FVector b)
	{
		return new FVector(a.Y * b.Z - a.Z * b.Y, a.Z * b.X - a.X * b.Z, a.X * b.Y - a.Y * b.X);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float operator |(FVector a, FVector b)
	{
		return a.X * b.X + a.Y * b.Y + a.Z * b.Z;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FVector operator +(FVector a, FVector b)
	{
		return new FVector(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FVector operator +(FVector a, float bias)
	{
		return new FVector(a.X + bias, a.Y + bias, a.Z + bias);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FVector operator -(FVector a, FVector b)
	{
		return new FVector(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FVector operator -(FVector a, float bias)
	{
		return new FVector(a.X - bias, a.Y - bias, a.Z - bias);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FVector operator *(FVector a, FVector b)
	{
		return new FVector(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FVector operator *(FVector a, float scale)
	{
		return new FVector(a.X * scale, a.Y * scale, a.Z * scale);
	}

	public static FVector operator *(FVector v, FQuat q)
	{
		FVector fVector = new FVector(q.X, q.Y, q.Z);
		float w = q.W;
		return 2f * DotProduct(fVector, v) * fVector + (w * w - DotProduct(fVector, fVector)) * v + 2f * w * CrossProduct(fVector, v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FVector operator *(float scale, FVector a)
	{
		return a * scale;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FVector operator /(FVector a, FVector b)
	{
		return new FVector(a.X / b.X, a.Y / b.Y, a.Z / b.Z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FVector operator /(FVector a, float scale)
	{
		float num = 1f / scale;
		return new FVector(a.X * num, a.Y * num, a.Z * num);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FVector operator /(float scale, FVector a)
	{
		return a / scale;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator ==(FVector a, FVector b)
	{
		if (a.X == b.X && a.Y == b.Y)
		{
			return a.Z == b.Z;
		}
		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator !=(FVector a, FVector b)
	{
		if (a.X == b.X && a.Y == b.Y)
		{
			return a.Z != b.Z;
		}
		return true;
	}

	public override bool Equals(object? obj)
	{
		if (obj is FVector v)
		{
			return Equals(v, 0f);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return (((X.GetHashCode() * 397) ^ Y.GetHashCode()) * 397) ^ Z.GetHashCode();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(FVector v, float tolerance = 0.0001f)
	{
		if (MathF.Abs(X - v.X) <= tolerance && MathF.Abs(Y - v.Y) <= tolerance)
		{
			return MathF.Abs(Z - v.Z) <= tolerance;
		}
		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool AllComponentsEqual(float tolerance = 0.0001f)
	{
		if (MathF.Abs(X - Y) <= tolerance && MathF.Abs(X - Z) <= tolerance)
		{
			return MathF.Abs(Y - Z) <= tolerance;
		}
		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float Max()
	{
		return MathF.Max(MathF.Max(X, Y), Z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float AbsMax()
	{
		return MathF.Max(MathF.Max(MathF.Abs(X), MathF.Abs(Y)), MathF.Abs(Z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float Min()
	{
		return MathF.Min(MathF.Min(X, Y), Z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float AbsMin()
	{
		return MathF.Min(MathF.Min(MathF.Abs(X), MathF.Abs(Y)), MathF.Abs(Z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public FVector ComponentMax(FVector other)
	{
		return new FVector(MathF.Max(X, other.X), MathF.Max(Y, other.Y), MathF.Max(Z, other.Z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public FVector ComponentMin(FVector other)
	{
		return new FVector(MathF.Min(X, other.X), MathF.Min(Y, other.Y), MathF.Min(Z, other.Z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public FVector Abs()
	{
		return new FVector(MathF.Abs(X), MathF.Abs(Y), MathF.Abs(Z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float Size()
	{
		return MathF.Sqrt(X * X + Y * Y + Z * Z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float SizeSquared()
	{
		return X * X + Y * Y + Z * Z;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float Size2D()
	{
		return MathF.Sqrt(X * X + Y * Y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float SizeSquared2D()
	{
		return X * X + Y * Y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool ContainsNaN()
	{
		if (float.IsFinite(X) && float.IsFinite(Y))
		{
			return !float.IsFinite(Z);
		}
		return true;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool IsNearlyZero(float tolerance = 0.0001f)
	{
		if (MathF.Abs(X) <= tolerance && MathF.Abs(Y) <= tolerance)
		{
			return MathF.Abs(Z) <= tolerance;
		}
		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool IsZero()
	{
		if (X == 0f && Y == 0f)
		{
			return Z == 0f;
		}
		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool IsUnit(float lengthSquaredTolerance = 0.0001f)
	{
		return MathF.Abs(1f - SizeSquared()) < lengthSquaredTolerance;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool IsNormalized()
	{
		return MathF.Abs(1f - SizeSquared()) < 0.01f;
	}

	public bool Normalize(float tolerance = 1E-08f)
	{
		float num = X * X + Y * Y + Z * Z;
		if (num > tolerance)
		{
			float num2 = num.InvSqrt();
			X *= num2;
			Y *= num2;
			Z *= num2;
			return true;
		}
		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public FVector GetClampedToMaxSize(float maxSize)
	{
		if (maxSize < 0.0001f)
		{
			return new FVector(0f, 0f, 0f);
		}
		float num = SizeSquared();
		if (num > maxSize * maxSize)
		{
			float num2 = maxSize * num.InvSqrt();
			return new FVector(X * num2, Y * num2, Z * num2);
		}
		return new FVector(X, Y, Z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public FVector GetSafeNormal(float tolerance = 1E-08f)
	{
		float num = X * X + Y * Y + Z * Z;
		if (num == 1f)
		{
			return this;
		}
		if (num < tolerance)
		{
			return ZeroVector;
		}
		float num2 = num.InvSqrt();
		return new FVector(X * num2, Y * num2, Z * num2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public FVector GetSafeNormal2D(float tolerance = 1E-08f)
	{
		float num = X * X + Y * Y;
		if (num == 1f)
		{
			if (Z != 0f)
			{
				return new FVector(X, Y, 0f);
			}
			return this;
		}
		if (num < tolerance)
		{
			return ZeroVector;
		}
		float num2 = num.InvSqrt();
		return new FVector(X * num2, Y * num2, 0f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float CosineAngle2D(FVector b)
	{
		FVector fVector = this;
		fVector.Z = 0f;
		b.Z = 0f;
		fVector.Normalize();
		b.Normalize();
		return fVector | b;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public FVector ProjectOnTo(FVector a)
	{
		return a * ((this | a) / (a | a));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public FVector ProjectOnToNormal(FVector normal)
	{
		return normal * (this | normal);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public FRotator ToOrientationRotator()
	{
		return new FRotator
		{
			Yaw = MathF.Atan2(Y, X) * (180f / (float)System.Math.PI),
			Pitch = MathF.Atan2(Z, MathF.Sqrt(X * X + Y * Y)) * (180f / (float)System.Math.PI),
			Roll = 0f
		};
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public FQuat ToOrientationQuat()
	{
		float num = MathF.Atan2(Y, X);
		float num2 = MathF.Atan2(Z, MathF.Sqrt(X * X + Y * Y));
		float num3 = MathF.Sin(num2 * 0.5f);
		float num4 = MathF.Sin(num * 0.5f);
		float num5 = MathF.Cos(num2 * 0.5f);
		float num6 = MathF.Cos(num * 0.5f);
		return new FQuat
		{
			X = num3 * num4,
			Y = (0f - num3) * num6,
			Z = num5 * num4,
			W = num5 * num6
		};
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public FRotator Rotation()
	{
		return ToOrientationRotator();
	}

	public override string ToString()
	{
		return $"X={X,3:F3} Y={Y,3:F3} Z={Z,3:F3}";
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FVector CrossProduct(FVector a, FVector b)
	{
		return a ^ b;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float DotProduct(FVector a, FVector b)
	{
		return a | b;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float ComputeSquaredDistanceFromBoxToPoint(FVector mins, FVector maxs, FVector point)
	{
		float num = 0f;
		if (point.X < mins.X)
		{
			num += (point.X - mins.X) * (point.X - mins.X);
		}
		else if (point.X > maxs.X)
		{
			num += (point.X - maxs.X) * (point.X - maxs.X);
		}
		if (point.Y < mins.Y)
		{
			num += (point.Y - mins.Y) * (point.Y - mins.Y);
		}
		else if (point.Y > maxs.Y)
		{
			num += (point.Y - maxs.Y) * (point.Y - maxs.Y);
		}
		if (point.Z < mins.Z)
		{
			num += (point.Z - mins.Z) * (point.Z - mins.Z);
		}
		else if (point.Z > maxs.Z)
		{
			num += (point.Z - maxs.Z) * (point.Z - maxs.Z);
		}
		return num;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float PointPlaneDist(FVector point, FVector planeBase, FVector planeNormal)
	{
		return (point - planeBase) | planeNormal;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FVector PointPlaneProject(FVector point, FVector planeBase, FVector planeNormal)
	{
		return point - PointPlaneDist(point, planeBase, planeNormal) * planeNormal;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FVector VectorPlaneProject(FVector delta, FVector normal)
	{
		return delta - delta.ProjectOnToNormal(normal);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float DistSquared(FVector v1, FVector v2)
	{
		return (v2.X - v1.X).Square() + (v2.Y - v1.Y).Square() + (v2.Z - v1.Z).Square();
	}

	public void Serialize(FArchiveWriter Ar)
	{
		Ar.Write(X);
		Ar.Write(Y);
		Ar.Write(Z);
	}

	public static implicit operator Vector3(FVector v)
	{
		return new Vector3(v.X, v.Y, v.Z);
	}
}

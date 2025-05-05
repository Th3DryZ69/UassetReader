using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;
using CUE4Parse.UE4.Writers;
using CUE4Parse.Utils;

namespace CUE4Parse.UE4.Objects.Core.Math;

public class FRotator : IUStruct
{
	private const float KindaSmallNumber = 0.0001f;

	public static readonly FRotator ZeroRotator = new FRotator(0f, 0f, 0f);

	public float Pitch;

	public float Yaw;

	public float Roll;

	public FRotator()
	{
	}

	public FRotator(EForceInit forceInit)
		: this(0f, 0f, 0f)
	{
	}

	public FRotator(float f)
		: this(f, f, f)
	{
	}

	public FRotator(float pitch, float yaw, float roll)
	{
		Pitch = pitch;
		Yaw = yaw;
		Roll = roll;
	}

	public FRotator(FArchive Ar)
	{
		if (Ar.Ver >= EUnrealEngineObjectUE5Version.LARGE_WORLD_COORDINATES)
		{
			Pitch = (float)Ar.Read<double>();
			Yaw = (float)Ar.Read<double>();
			Roll = (float)Ar.Read<double>();
		}
		else
		{
			Pitch = Ar.Read<float>();
			Yaw = Ar.Read<float>();
			Roll = Ar.Read<float>();
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FRotator operator +(FRotator a, FRotator b)
	{
		return new FRotator(a.Pitch + b.Pitch, a.Yaw + b.Yaw, a.Roll + b.Roll);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FRotator operator -(FRotator a, FRotator b)
	{
		return new FRotator(a.Pitch - b.Pitch, a.Yaw - b.Yaw, a.Roll - b.Roll);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FRotator operator *(FRotator r, float scale)
	{
		return new FRotator(r.Pitch * scale, r.Yaw * scale, r.Roll * scale);
	}

	public FVector RotateVector(FVector v)
	{
		return new FVector(new FRotationMatrix(this).TransformVector(v));
	}

	public FVector UnrotateVector(FVector v)
	{
		return new FVector(new FRotationMatrix(this).GetTransposed().TransformVector(v));
	}

	public FVector Vector()
	{
		float num = Pitch.ToRadians();
		float z = (float)System.Math.Sin(num);
		float num2 = (float)System.Math.Cos(num);
		float num3 = Yaw.ToRadians();
		float num4 = (float)System.Math.Sin(num3);
		float num5 = (float)System.Math.Cos(num3);
		return new FVector(num2 * num5, num2 * num4, z);
	}

	public FQuat Quaternion()
	{
		float num = (float)System.Math.Sin(Pitch * ((float)System.Math.PI / 360f));
		float num2 = (float)System.Math.Cos(Pitch * ((float)System.Math.PI / 360f));
		float num3 = (float)System.Math.Sin(Yaw * ((float)System.Math.PI / 360f));
		float num4 = (float)System.Math.Cos(Yaw * ((float)System.Math.PI / 360f));
		float num5 = (float)System.Math.Sin(Roll * ((float)System.Math.PI / 360f));
		float num6 = (float)System.Math.Cos(Roll * ((float)System.Math.PI / 360f));
		return new FQuat
		{
			X = num6 * num * num3 - num5 * num2 * num4,
			Y = (0f - num6) * num * num4 - num5 * num2 * num3,
			Z = num6 * num2 * num3 - num5 * num * num4,
			W = num6 * num2 * num4 + num5 * num * num3
		};
	}

	public void Normalize()
	{
		Pitch = NormalizeAxis(Pitch);
		Yaw = NormalizeAxis(Yaw);
		Roll = NormalizeAxis(Roll);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public FRotator GetNormalized()
	{
		Normalize();
		return this;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float ClampAxis(float angle)
	{
		angle %= 360f;
		if (angle < 0f)
		{
			angle += 360f;
		}
		return angle;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float NormalizeAxis(float angle)
	{
		angle = ClampAxis(angle);
		if (angle > 180f)
		{
			angle -= 360f;
		}
		return angle;
	}

	public static byte CompressAxisToByte(float angle)
	{
		return (byte)((angle * 256f / 360f).RoundToInt() & 0xFF);
	}

	public static float DecompressAxisFromByte(byte angle)
	{
		return (float)(int)angle * 360f / 256f;
	}

	public static ushort CompressAxisToShort(float angle)
	{
		return (ushort)((angle * 65536f / 360f).RoundToInt() & 0xFFF);
	}

	public static float DecompressAxisFromShort(ushort angle)
	{
		return (float)(int)angle * 360f / 65536f;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator ==(FRotator? a, FRotator b)
	{
		if ((object)a != null && a.Pitch == b.Pitch && a.Yaw == b.Yaw)
		{
			return a.Roll == b.Roll;
		}
		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator !=(FRotator? a, FRotator b)
	{
		return !(a == b);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(FRotator r, float tolerance = 0.0001f)
	{
		if (System.Math.Abs(NormalizeAxis(Pitch - r.Pitch)) <= tolerance && System.Math.Abs(NormalizeAxis(Yaw - r.Yaw)) <= tolerance)
		{
			return System.Math.Abs(NormalizeAxis(Roll - r.Roll)) <= tolerance;
		}
		return false;
	}

	public void Serialize(FArchiveWriter Ar)
	{
		Ar.Write(Pitch);
		Ar.Write(Yaw);
		Ar.Write(Roll);
	}

	public override bool Equals(object? obj)
	{
		if (obj is FRotator r)
		{
			return Equals(r, 0f);
		}
		return false;
	}

	public override string ToString()
	{
		return $"P={Pitch} Y={Yaw} R={Roll}";
	}

	public static implicit operator Vector3(FRotator r)
	{
		return new Vector3(r.Pitch, r.Yaw, r.Roll);
	}
}

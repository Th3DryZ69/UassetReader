using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Objects.Core.Math;

[StructLayout(LayoutKind.Sequential, Size = 16)]
public struct FPlane : IUStruct
{
	public FVector Vector;

	public float W;

	public float X
	{
		get
		{
			return Vector.X;
		}
		set
		{
			Vector.X = value;
		}
	}

	public float Y
	{
		get
		{
			return Vector.Y;
		}
		set
		{
			Vector.Y = value;
		}
	}

	public float Z
	{
		get
		{
			return Vector.Z;
		}
		set
		{
			Vector.Z = value;
		}
	}

	public FPlane(FVector @base, FVector normal)
	{
		Vector = @base;
		W = @base | normal;
	}

	public FPlane(float x, float y, float z, float w)
	{
		this = default(FPlane);
		X = x;
		Y = y;
		Z = z;
		W = w;
	}

	public FPlane(FArchive Ar)
	{
		Vector = new FVector(Ar);
		if (Ar.Ver >= EUnrealEngineObjectUE5Version.LARGE_WORLD_COORDINATES)
		{
			W = (float)Ar.Read<double>();
		}
		else
		{
			W = Ar.Read<float>();
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float PlaneDot(FVector p)
	{
		return X * p.X + Y * p.Y + Z * p.Z - W;
	}

	public bool Equals(FPlane v, float tolerance = 0.0001f)
	{
		if (MathF.Abs(X - v.X) <= tolerance && MathF.Abs(Y - v.Y) <= tolerance && MathF.Abs(Z - v.Z) <= tolerance)
		{
			return MathF.Abs(W - v.W) <= tolerance;
		}
		return false;
	}

	public override bool Equals(object? obj)
	{
		if (obj is FPlane v)
		{
			return Equals(v, 0f);
		}
		return false;
	}
}

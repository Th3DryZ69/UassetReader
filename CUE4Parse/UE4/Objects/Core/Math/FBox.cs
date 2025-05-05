using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Objects.Core.Math;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct FBox : IUStruct
{
	public FVector Min;

	public FVector Max;

	public byte IsValid;

	public FVector this[int i] => i switch
	{
		0 => Min, 
		1 => Max, 
		_ => throw new IndexOutOfRangeException(), 
	};

	public FBox(FVector min, FVector max, byte isValid = 1)
	{
		Min = min;
		Max = max;
		IsValid = isValid;
	}

	public FBox(FArchive Ar)
	{
		Min = new FVector(Ar);
		Max = new FVector(Ar);
		IsValid = Ar.Read<byte>();
	}

	public FBox(FVector[] points)
	{
		Min = new FVector(0f, 0f, 0f);
		Max = new FVector(0f, 0f, 0f);
		IsValid = 0;
		foreach (FVector fVector in points)
		{
			Min += fVector;
			Max += fVector;
		}
	}

	public FBox(FBox box)
	{
		Min = box.Min;
		Max = box.Max;
		IsValid = box.IsValid;
	}

	public bool Equals(FBox other)
	{
		if (Min.Equals(other.Min))
		{
			return Max.Equals(other.Max);
		}
		return false;
	}

	public override bool Equals(object? obj)
	{
		if (obj is FBox other)
		{
			return Equals(other);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return (Min.GetHashCode() * 397) ^ Max.GetHashCode();
	}

	public static FBox operator +(FBox a, FVector other)
	{
		if (a.IsValid != 0)
		{
			return new FBox(new FVector(System.Math.Min(a.Min.X, other.X), System.Math.Min(a.Min.Y, other.Y), System.Math.Min(a.Min.Z, other.Z)), new FVector(System.Math.Max(a.Max.X, other.X), System.Math.Max(a.Max.Y, other.Y), System.Math.Max(a.Max.Z, other.Z)), 1);
		}
		return new FBox(other, other, 1);
	}

	public static FBox operator +(FBox a, FBox other)
	{
		if (a.IsValid != 0)
		{
			return new FBox(new FVector(System.Math.Min(a.Min.X, other.Min.X), System.Math.Min(a.Min.Y, other.Min.Y), System.Math.Min(a.Min.Z, other.Min.Z)), new FVector(System.Math.Max(a.Max.X, other.Max.X), System.Math.Max(a.Max.Y, other.Max.Y), System.Math.Max(a.Max.Z, other.Max.Z)), 1);
		}
		return new FBox(other.Min, other.Max, other.IsValid);
	}

	public static FBox operator *(FBox a, float scale)
	{
		return new FBox(new FVector(a.Min.X * scale, a.Min.Y * scale, a.Min.Z * scale), new FVector(a.Max.X * scale, a.Max.Y * scale, a.Max.Z * scale), 1);
	}

	public float ComputeSquaredDistanceToPoint(FVector point)
	{
		return FVector.ComputeSquaredDistanceFromBoxToPoint(Min, Max, point);
	}

	public FBox ExpandBy(float w)
	{
		return new FBox(Min - new FVector(w, w, w), Max + new FVector(w, w, w), 1);
	}

	public FBox ExpandBy(FVector v)
	{
		return new FBox(Min - v, Max + v, 1);
	}

	public FBox ExpandBy(FVector neg, FVector pos)
	{
		return new FBox(Min - neg, Max + pos, 1);
	}

	public FBox ShiftBy(FVector offset)
	{
		return new FBox(Min + offset, Max + offset, 1);
	}

	public FBox MoveTo(FVector destination)
	{
		FVector fVector = destination - GetCenter();
		return new FBox(Min + fVector, Max + fVector, 1);
	}

	public FVector GetCenter()
	{
		return (Min + Max) * 0.5f;
	}

	public void GetCenterAndExtents(out FVector center, out FVector extents)
	{
		extents = GetExtent();
		center = Min + extents;
	}

	public FVector GetClosestPointTo(FVector point)
	{
		FVector result = point;
		if (point.X < Min.X)
		{
			result.X = Min.X;
		}
		else if (point.X > Max.X)
		{
			result.X = Max.X;
		}
		if (point.Y < Min.Y)
		{
			result.Y = Min.Y;
		}
		else if (point.Y > Max.Y)
		{
			result.Y = Max.Y;
		}
		if (point.Z < Min.Z)
		{
			result.Z = Min.Z;
		}
		else if (point.Z > Max.Z)
		{
			result.Z = Max.Z;
		}
		return result;
	}

	public FVector GetExtent()
	{
		return (Max - Min) * 0.5f;
	}

	public FVector GetSize()
	{
		return Max - Min;
	}

	public float GetVolume()
	{
		return (Max.X - Min.X) * (Max.Y - Min.Y) * (Max.Z - Min.Z);
	}

	public bool Intersects(FBox other)
	{
		if (Min.X > other.Max.X || other.Min.X > Max.X)
		{
			return false;
		}
		if (Min.Y > other.Max.Y || other.Min.Y > Max.Y)
		{
			return false;
		}
		if (Min.Z > other.Max.Z || other.Min.Z > Max.Z)
		{
			return false;
		}
		return true;
	}

	public bool IntersectsXY(FBox other)
	{
		if (Min.X > other.Max.X || other.Min.X > Max.X)
		{
			return false;
		}
		if (Min.Y > other.Max.Y || other.Min.Y > Max.Y)
		{
			return false;
		}
		return true;
	}

	public FBox Overlap(FBox other)
	{
		if (!Intersects(other))
		{
			return new FBox(new FVector(0f, 0f, 0f), new FVector(0f, 0f, 0f), 1);
		}
		FVector min = default(FVector);
		FVector max = default(FVector);
		min.X = System.Math.Max(Min.X, other.Min.X);
		max.X = System.Math.Min(Max.X, other.Max.X);
		min.Y = System.Math.Max(Min.Y, other.Min.Y);
		max.Y = System.Math.Min(Max.Y, other.Max.Y);
		min.Z = System.Math.Max(Min.Z, other.Min.Z);
		max.Z = System.Math.Min(Max.Z, other.Max.Z);
		return new FBox(min, max, 1);
	}

	public bool IsInside(FVector @in)
	{
		if (@in.X > Min.X && @in.X < Max.X && @in.Y > Min.Y && @in.Y < Max.Y && @in.Z > Min.Z)
		{
			return @in.Z < Max.Z;
		}
		return false;
	}

	public bool IsInsideOrOn(FVector @in)
	{
		if (@in.X >= Min.X && @in.X <= Max.X && @in.Y >= Min.Y && @in.Y <= Max.Y && @in.Z >= Min.Z)
		{
			return @in.Z <= Max.Z;
		}
		return false;
	}

	public bool IsInside(FBox other)
	{
		if (IsInside(other.Min))
		{
			return IsInside(other.Max);
		}
		return false;
	}

	public bool IsInsideXY(FVector @in)
	{
		if (@in.X > Min.X && @in.X < Max.X && @in.Y > Min.Y)
		{
			return @in.Y < Max.Y;
		}
		return false;
	}

	public bool IsInsideXY(FBox other)
	{
		if (IsInsideXY(other.Min))
		{
			return IsInsideXY(other.Max);
		}
		return false;
	}

	public FBox TransformBy(FMatrix m)
	{
		if (IsValid == 0)
		{
			return default(FBox);
		}
		FVector min = Min;
		FVector max = Max;
		FVector fVector = new FVector(m.M00, m.M01, m.M02);
		FVector fVector2 = new FVector(m.M10, m.M11, m.M12);
		FVector fVector3 = new FVector(m.M20, m.M21, m.M22);
		FVector fVector4 = new FVector(m.M30, m.M31, m.M32);
		FVector fVector5 = new FVector(0.5f, 0.5f, 0.5f);
		FVector fVector6 = (max + min) * fVector5;
		FVector fVector7 = (max - min) * fVector5;
		FVector fVector8 = new FVector(fVector6.X) * fVector + new FVector(fVector6.Y) * fVector2 + new FVector(fVector6.Z) * fVector3 + fVector4;
		FVector fVector9 = (new FVector(fVector7.X) * fVector).Abs() + (new FVector(fVector7.Y) * fVector2).Abs() + (new FVector(fVector7.Z) * fVector3).Abs();
		return new FBox
		{
			Min = fVector8 - fVector9,
			Max = fVector8 + fVector9,
			IsValid = 1
		};
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public FBox TransformBy(FTransform m)
	{
		return TransformBy(m.ToMatrixWithScale());
	}

	public override string ToString()
	{
		return $"IsValid={IsValid != 0}, Min={Min}, Max={Max}";
	}

	public static FBox BuildAABB(FVector origin, FVector extent)
	{
		return new FBox(origin - extent, origin + extent, 1);
	}
}

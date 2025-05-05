using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;
using CUE4Parse.Utils;

namespace CUE4Parse.UE4.Objects.Core.Math;

public class FBoxSphereBounds
{
	public FVector Origin;

	public FVector BoxExtent;

	public float SphereRadius;

	public FBoxSphereBounds()
	{
	}

	public FBoxSphereBounds(FArchive Ar)
	{
		Origin = new FVector(Ar);
		BoxExtent = new FVector(Ar);
		if (Ar.Ver >= EUnrealEngineObjectUE5Version.LARGE_WORLD_COORDINATES)
		{
			SphereRadius = (float)Ar.Read<double>();
		}
		else
		{
			SphereRadius = Ar.Read<float>();
		}
	}

	public FBoxSphereBounds(FVector origin, FVector boxExtent, float sphereRadius)
	{
		Origin = origin;
		BoxExtent = boxExtent;
		SphereRadius = sphereRadius;
	}

	public FBoxSphereBounds(FBox box, FSphere sphere)
	{
		box.GetCenterAndExtents(out Origin, out BoxExtent);
		SphereRadius = MathF.Min(BoxExtent.Size(), (sphere.Center - Origin).Size() + sphere.W);
	}

	public FBoxSphereBounds(FBox box)
	{
		box.GetCenterAndExtents(out Origin, out BoxExtent);
		SphereRadius = BoxExtent.Size();
	}

	public FBoxSphereBounds(FSphere sphere)
	{
		Origin = sphere.Center;
		BoxExtent = new FVector(sphere.W);
		SphereRadius = sphere.W;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public FBox GetBox()
	{
		return new FBox(Origin - BoxExtent, Origin + BoxExtent, 1);
	}

	public FBoxSphereBounds TransformBy(FMatrix m)
	{
		FBoxSphereBounds fBoxSphereBounds = new FBoxSphereBounds();
		FVector origin = Origin;
		FVector boxExtent = BoxExtent;
		Vector3 vector = new Vector3(m.M00, m.M01, m.M02);
		Vector3 vector2 = new Vector3(m.M10, m.M11, m.M12);
		Vector3 vector3 = new Vector3(m.M20, m.M21, m.M22);
		Vector3 vector4 = new Vector3(m.M30, m.M31, m.M32);
		Vector3 v = new Vector3(origin.X) * vector + new Vector3(origin.Y) * vector2 + new Vector3(origin.Z) * vector3 + vector4;
		Vector3 vector5 = Vector3.Abs(new Vector3(boxExtent.X) * vector) + Vector3.Abs(new Vector3(boxExtent.Y) * vector2) + Vector3.Abs(new Vector3(boxExtent.Z) * vector3);
		fBoxSphereBounds.BoxExtent = vector5.ToFVector();
		fBoxSphereBounds.Origin = v.ToFVector();
		Vector3 value = vector * vector + vector2 * vector2 + vector3 * vector3;
		fBoxSphereBounds.SphereRadius = MathF.Sqrt(Vector3.Max(Vector3.Max(value, new Vector3(value.Y)), new Vector3(value.Z)).X) * SphereRadius;
		float y = MathF.Sqrt(Vector3.Dot(vector5, vector5));
		fBoxSphereBounds.SphereRadius = MathF.Min(fBoxSphereBounds.SphereRadius, y);
		return fBoxSphereBounds;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public FBoxSphereBounds TransformBy(FTransform m)
	{
		return TransformBy(m.ToMatrixWithScale());
	}

	public override string ToString()
	{
		return $"Origin=({Origin}), BoxExtent=({BoxExtent}), SphereRadius={SphereRadius}";
	}
}

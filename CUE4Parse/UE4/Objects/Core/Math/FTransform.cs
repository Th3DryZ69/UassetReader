using System;
using System.Runtime.CompilerServices;
using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Assets.Utils;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Objects.Core.Math;

[StructFallback]
public class FTransform : ICloneable
{
	public static FTransform Identity = new FTransform
	{
		Rotation = FQuat.Identity,
		Translation = FVector.ZeroVector,
		Scale3D = FVector.OneVector
	};

	public FQuat Rotation;

	public FVector Translation;

	public FVector Scale3D;

	public bool IsRotationNormalized => Rotation.IsNormalized;

	public FTransform(EForceInit init = EForceInit.ForceInit)
	{
		Rotation = new FQuat(0f, 0f, 0f, 1f);
		Translation = new FVector(0f);
		Scale3D = FVector.OneVector;
	}

	public FTransform(FArchive Ar)
	{
		Rotation = new FQuat(Ar);
		Translation = new FVector(Ar);
		Scale3D = new FVector(Ar);
	}

	public FTransform(FVector translation)
	{
		Rotation = FQuat.Identity;
		Translation = translation;
		Scale3D = FVector.OneVector;
	}

	public FTransform(FQuat rotation)
	{
		Rotation = rotation;
		Translation = FVector.ZeroVector;
		Scale3D = FVector.OneVector;
	}

	public FTransform(FRotator rotation)
	{
		Rotation = new FQuat(rotation);
		Translation = FVector.ZeroVector;
		Scale3D = FVector.OneVector;
	}

	public FTransform(FQuat rotation, FVector translation, FVector scale3D)
	{
		Rotation = rotation;
		Translation = translation;
		Scale3D = scale3D;
	}

	public FTransform(FRotator rotation, FVector translation, FVector scale3D)
	{
		Rotation = new FQuat(rotation);
		Translation = translation;
		Scale3D = scale3D;
	}

	public FTransform(FStructFallback data)
	{
		Rotation = data.GetOrDefault<FQuat>("Rotation");
		Translation = data.GetOrDefault<FVector>("Translation");
		Scale3D = data.GetOrDefault<FVector>("Scale3D");
	}

	public void SetFromMatrix(FMatrix inMatrix)
	{
		FMatrix fMatrix = new FMatrix(inMatrix);
		Scale3D = fMatrix.ExtractScaling();
		if (inMatrix.Determinant() < 0f)
		{
			Scale3D.X *= -1f;
			fMatrix.SetAxis(0, -fMatrix.GetScaledAxis(EAxis.X));
		}
		Rotation = fMatrix.ToQuat();
		Translation = inMatrix.GetOrigin();
		Rotation.Normalize();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public FRotator Rotator()
	{
		return Rotation.Rotator();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float GetDeterminant()
	{
		return Scale3D.X * Scale3D.Y * Scale3D.Z;
	}

	public bool Equals(FTransform other, float tolerance = 0.0001f)
	{
		if (Rotation.Equals(other.Rotation, tolerance) && Translation.Equals(other.Translation, tolerance))
		{
			return Scale3D.Equals(other.Scale3D, tolerance);
		}
		return false;
	}

	public bool ContainsNaN()
	{
		if (!Translation.ContainsNaN() && !Rotation.ContainsNaN())
		{
			return Scale3D.ContainsNaN();
		}
		return true;
	}

	public static bool AnyHasNegativeScale(FVector scale3D, FVector otherScale3D)
	{
		if (!(scale3D.X < 0f) && !(scale3D.Y < 0f) && !(scale3D.Z < 0f) && !(otherScale3D.X < 0f) && !(otherScale3D.Y < 0f))
		{
			return otherScale3D.Z < 0f;
		}
		return true;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void ScaleTranslation(FVector scale3D)
	{
		Translation *= scale3D;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void ScaleTranslation(float scale)
	{
		Translation *= scale;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void RemoveScaling(float tolerance = 1E-08f)
	{
		Scale3D = new FVector(1f, 1f, 1f);
		Rotation.Normalize();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float GetMaximumAxisScale()
	{
		return Scale3D.AbsMax();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float GetMinimumAxisScale()
	{
		return Scale3D.AbsMin();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void CopyTranslation(ref FTransform other)
	{
		Translation = other.Translation;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void CopyRotation(ref FTransform other)
	{
		Rotation = other.Rotation;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void CopyScale3D(ref FTransform other)
	{
		Scale3D = other.Scale3D;
	}

	public FTransform Inverse()
	{
		FQuat fQuat = Rotation.Inverse();
		FVector safeScaleReciprocal = GetSafeScaleReciprocal(Scale3D);
		FVector translation = fQuat * (safeScaleReciprocal * -Translation);
		return new FTransform(fQuat, translation, safeScaleReciprocal);
	}

	public FTransform GetRelativeTransform(FTransform other)
	{
		FTransform outTransform = new FTransform();
		if (AnyHasNegativeScale(Scale3D, other.Scale3D))
		{
			GetRelativeTransformUsingMatrixWithScale(ref outTransform, ref other);
		}
		else
		{
			FVector safeScaleReciprocal = GetSafeScaleReciprocal(other.Scale3D);
			outTransform.Scale3D = Scale3D * safeScaleReciprocal;
			if (!other.Rotation.IsNormalized)
			{
				return Identity;
			}
			FQuat fQuat = other.Rotation.Inverse();
			outTransform.Rotation = fQuat * Rotation;
			outTransform.Translation = fQuat * (Translation - other.Translation) * safeScaleReciprocal;
		}
		return outTransform;
	}

	public static FVector SubstractTranslations(FTransform a, FTransform b)
	{
		return a.Translation - b.Translation;
	}

	public void GetRelativeTransformUsingMatrixWithScale(ref FTransform outTransform, ref FTransform relative)
	{
		FMatrix aMatrix = ToMatrixWithScale();
		FMatrix fMatrix = ToMatrixWithScale();
		FVector safeScaleReciprocal = GetSafeScaleReciprocal(relative.Scale3D);
		ConstructTransformFromMatrixWithDesiredScale(desiredScale: Scale3D * safeScaleReciprocal, aMatrix: aMatrix, bMatrix: fMatrix.InverseFast(), outTransform: ref outTransform);
	}

	public static void ConstructTransformFromMatrixWithDesiredScale(FMatrix aMatrix, FMatrix bMatrix, FVector desiredScale, ref FTransform outTransform)
	{
		FMatrix fMatrix = aMatrix * bMatrix;
		fMatrix.RemoveScaling();
		FVector signVector = desiredScale.GetSignVector();
		fMatrix.SetAxis(0, signVector.X * fMatrix.GetScaledAxis(EAxis.X));
		fMatrix.SetAxis(1, signVector.Y * fMatrix.GetScaledAxis(EAxis.Y));
		fMatrix.SetAxis(2, signVector.Z * fMatrix.GetScaledAxis(EAxis.Z));
		FQuat rotation = new FQuat(fMatrix);
		rotation.Normalize();
		outTransform.Scale3D = desiredScale;
		outTransform.Rotation = rotation;
		outTransform.Translation = fMatrix.GetOrigin();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public FMatrix ToMatrixWithScale()
	{
		FMatrix obj = new FMatrix
		{
			M30 = Translation.X,
			M31 = Translation.Y,
			M32 = Translation.Z
		};
		float num = Rotation.X + Rotation.X;
		float num2 = Rotation.Y + Rotation.Y;
		float num3 = Rotation.Z + Rotation.Z;
		float num4 = Rotation.X * num;
		float num5 = Rotation.Y * num2;
		float num6 = Rotation.Z * num3;
		obj.M00 = (1f - (num5 + num6)) * Scale3D.X;
		obj.M11 = (1f - (num4 + num6)) * Scale3D.Y;
		obj.M22 = (1f - (num4 + num5)) * Scale3D.Z;
		float num7 = Rotation.Y * num3;
		float num8 = Rotation.W * num;
		obj.M21 = (num7 - num8) * Scale3D.Z;
		obj.M12 = (num7 + num8) * Scale3D.Y;
		float num9 = Rotation.X * num2;
		float num10 = Rotation.W * num3;
		obj.M10 = (num9 - num10) * Scale3D.Y;
		obj.M01 = (num9 + num10) * Scale3D.X;
		float num11 = Rotation.X * num3;
		float num12 = Rotation.W * num2;
		obj.M20 = (num11 + num12) * Scale3D.Z;
		obj.M02 = (num11 - num12) * Scale3D.X;
		obj.M03 = 0f;
		obj.M13 = 0f;
		obj.M23 = 0f;
		obj.M33 = 1f;
		return obj;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FVector GetSafeScaleReciprocal(FVector scale, float tolerance = 1E-08f)
	{
		FVector result = default(FVector);
		if (MathF.Abs(scale.X) <= tolerance)
		{
			result.X = 0f;
		}
		else
		{
			result.X = 1f / scale.X;
		}
		if (MathF.Abs(scale.Y) <= tolerance)
		{
			result.Y = 0f;
		}
		else
		{
			result.Y = 1f / scale.Y;
		}
		if (MathF.Abs(scale.Z) <= tolerance)
		{
			result.Z = 0f;
		}
		else
		{
			result.Z = 1f / scale.Z;
		}
		return result;
	}

	public static FTransform operator *(FTransform a, FTransform b)
	{
		if (!a.IsRotationNormalized)
		{
			throw new ArgumentException("Rotation a must be normalized for multiplication");
		}
		if (!b.IsRotationNormalized)
		{
			throw new ArgumentException("Rotation b must be normalized for multiplication");
		}
		FTransform outTransform = new FTransform();
		if (AnyHasNegativeScale(a.Scale3D, b.Scale3D))
		{
			MultiplyUsingMatrixWithScale(ref outTransform, ref a, ref b);
		}
		else
		{
			outTransform.Rotation = b.Rotation * a.Rotation;
			outTransform.Scale3D = b.Scale3D * a.Scale3D;
			outTransform.Translation = b.Rotation * (b.Scale3D * a.Translation) + b.Translation;
		}
		return outTransform;
	}

	public static void MultiplyUsingMatrixWithScale(ref FTransform outTransform, ref FTransform a, ref FTransform b)
	{
		ConstructTransformFromMatrixWithDesiredScale(a.ToMatrixWithScale(), b.ToMatrixWithScale(), a.Scale3D * b.Scale3D, ref outTransform);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public FVector TransformPosition(FVector v)
	{
		return Rotation.RotateVector(Scale3D * v) + Translation;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public FVector TransformPositionNoScale(FVector v)
	{
		return Rotation.RotateVector(v) + Translation;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public FVector InverseTransformPosition(FVector v)
	{
		return Rotation.UnrotateVector(v - Translation) * GetSafeScaleReciprocal(Scale3D);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public FVector InverseTransformPositionNoScale(FVector v)
	{
		return Rotation.UnrotateVector(v - Translation);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public FVector TransformVector(FVector v)
	{
		return Rotation.RotateVector(Scale3D * v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public FVector TransformVectorNoScale(FVector v)
	{
		return Rotation.RotateVector(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public FQuat TransformRotation(FQuat q)
	{
		return Rotation * q;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public FQuat InverseTransformRotation(FQuat q)
	{
		return Rotation.Inverse() * q;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public FTransform GetScaled(float scale)
	{
		Scale3D *= scale;
		return this;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public FTransform GetScaled(FVector scale)
	{
		Scale3D *= scale;
		return this;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public FVector GetScaledAxis(EAxis axis)
	{
		return axis switch
		{
			EAxis.X => TransformVector(new FVector(1f, 0f, 0f)), 
			EAxis.Y => TransformVector(new FVector(0f, 1f, 0f)), 
			_ => TransformVector(new FVector(0f, 0f, 1f)), 
		};
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public FVector GetUnitAxis(EAxis axis)
	{
		return axis switch
		{
			EAxis.X => TransformVectorNoScale(new FVector(1f, 0f, 0f)), 
			EAxis.Y => TransformVectorNoScale(new FVector(0f, 1f, 0f)), 
			_ => TransformVectorNoScale(new FVector(0f, 0f, 1f)), 
		};
	}

	public override string ToString()
	{
		return $"{{T:{Translation} R:{Rotation} S:{Scale3D}}}";
	}

	public object Clone()
	{
		return MemberwiseClone();
	}
}

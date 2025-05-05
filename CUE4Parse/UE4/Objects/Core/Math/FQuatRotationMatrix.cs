namespace CUE4Parse.UE4.Objects.Core.Math;

public class FQuatRotationMatrix : FQuatRotationTranslationMatrix
{
	public FQuatRotationMatrix(FQuat q)
		: base(q, FVector.ZeroVector)
	{
	}
}

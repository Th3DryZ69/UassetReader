namespace CUE4Parse.UE4.Objects.Core.Math;

public sealed class FRotationMatrix : FRotationTranslationMatrix
{
	public FRotationMatrix(FRotator rot)
		: base(rot, default(FVector))
	{
	}
}

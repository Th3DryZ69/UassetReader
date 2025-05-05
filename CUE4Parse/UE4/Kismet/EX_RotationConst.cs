using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Core.Math;

namespace CUE4Parse.UE4.Kismet;

public class EX_RotationConst : KismetExpression<FRotator>
{
	public override EExprToken Token => EExprToken.EX_RotationConst;

	public EX_RotationConst(FKismetArchive Ar)
	{
		Value = new FRotator(Ar);
	}
}

using CUE4Parse.UE4.Assets.Readers;

namespace CUE4Parse.UE4.Kismet;

public class EX_SoftObjectConst : KismetExpression<KismetExpression>
{
	public override EExprToken Token => EExprToken.EX_SoftObjectConst;

	public EX_SoftObjectConst(FKismetArchive Ar)
	{
		Value = Ar.ReadExpression();
	}
}

using CUE4Parse.UE4.Assets.Readers;

namespace CUE4Parse.UE4.Kismet;

public class EX_FieldPathConst : KismetExpression<KismetExpression>
{
	public override EExprToken Token => EExprToken.EX_FieldPathConst;

	public EX_FieldPathConst(FKismetArchive Ar)
	{
		Value = Ar.ReadExpression();
	}
}

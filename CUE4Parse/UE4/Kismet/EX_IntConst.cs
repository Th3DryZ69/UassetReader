using CUE4Parse.UE4.Assets.Readers;

namespace CUE4Parse.UE4.Kismet;

public class EX_IntConst : KismetExpression<int>
{
	public override EExprToken Token => EExprToken.EX_IntConst;

	public EX_IntConst(FKismetArchive Ar)
	{
		Value = Ar.Read<int>();
	}
}

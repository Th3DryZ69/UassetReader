using CUE4Parse.UE4.Assets.Readers;

namespace CUE4Parse.UE4.Kismet;

public class EX_Int64Const : KismetExpression<long>
{
	public override EExprToken Token => EExprToken.EX_Int64Const;

	public EX_Int64Const(FKismetArchive Ar)
	{
		Value = Ar.Read<long>();
	}
}

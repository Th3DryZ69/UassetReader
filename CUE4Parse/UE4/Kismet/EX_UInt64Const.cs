using CUE4Parse.UE4.Assets.Readers;

namespace CUE4Parse.UE4.Kismet;

public class EX_UInt64Const : KismetExpression<ulong>
{
	public override EExprToken Token => EExprToken.EX_UInt64Const;

	public EX_UInt64Const(FKismetArchive Ar)
	{
		Value = Ar.Read<ulong>();
	}
}

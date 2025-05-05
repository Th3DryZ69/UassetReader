using CUE4Parse.UE4.Assets.Readers;

namespace CUE4Parse.UE4.Kismet;

public class EX_StringConst : KismetExpression<string>
{
	public override EExprToken Token => EExprToken.EX_StringConst;

	public EX_StringConst(FKismetArchive Ar)
	{
		Value = Ar.XFERSTRING();
		Ar.Position++;
		Ar.Index++;
	}
}

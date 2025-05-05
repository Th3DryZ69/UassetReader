using CUE4Parse.UE4.Assets.Readers;

namespace CUE4Parse.UE4.Kismet;

public class EX_UnicodeStringConst : KismetExpression<string>
{
	public override EExprToken Token => EExprToken.EX_UnicodeStringConst;

	public EX_UnicodeStringConst(FKismetArchive Ar)
	{
		Value = Ar.XFERUNICODESTRING();
		Ar.Position += 2L;
		Ar.Index += 2;
	}
}

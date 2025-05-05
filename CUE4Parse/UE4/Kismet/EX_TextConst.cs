using CUE4Parse.UE4.Assets.Readers;

namespace CUE4Parse.UE4.Kismet;

public class EX_TextConst : KismetExpression<FScriptText>
{
	public override EExprToken Token => EExprToken.EX_TextConst;

	public EX_TextConst(FKismetArchive Ar)
	{
		Value = new FScriptText(Ar);
	}
}

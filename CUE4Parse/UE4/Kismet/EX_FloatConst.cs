using CUE4Parse.UE4.Assets.Readers;

namespace CUE4Parse.UE4.Kismet;

public class EX_FloatConst : KismetExpression<float>
{
	public override EExprToken Token => EExprToken.EX_FloatConst;

	public EX_FloatConst(FKismetArchive Ar)
	{
		Value = Ar.Read<float>();
	}
}

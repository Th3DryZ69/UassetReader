using CUE4Parse.UE4.Assets.Readers;

namespace CUE4Parse.UE4.Kismet;

public class EX_DoubleConst : KismetExpression<double>
{
	public override EExprToken Token => EExprToken.EX_DoubleConst;

	public EX_DoubleConst(FKismetArchive Ar)
	{
		Value = Ar.Read<double>();
	}
}

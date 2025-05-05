using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Core.Math;

namespace CUE4Parse.UE4.Kismet;

public class EX_VectorConst : KismetExpression<FVector>
{
	public override EExprToken Token => EExprToken.EX_VectorConst;

	public EX_VectorConst(FKismetArchive Ar)
	{
		Value = new FVector(Ar);
	}
}

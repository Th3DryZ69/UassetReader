using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Core.Math;

namespace CUE4Parse.UE4.Kismet;

public class EX_TransformConst : KismetExpression<FTransform>
{
	public override EExprToken Token => EExprToken.EX_TransformConst;

	public EX_TransformConst(FKismetArchive Ar)
	{
		Value = new FTransform(Ar);
	}
}

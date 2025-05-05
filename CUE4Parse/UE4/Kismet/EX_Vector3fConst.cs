using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Core.Math;

namespace CUE4Parse.UE4.Kismet;

public class EX_Vector3fConst : KismetExpression<FVector>
{
	public override EExprToken Token => EExprToken.EX_Vector3fConst;

	public EX_Vector3fConst(FKismetArchive Ar)
	{
		Value = Ar.Read<FVector>();
	}
}

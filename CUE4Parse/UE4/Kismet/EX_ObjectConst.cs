using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.UObject;

namespace CUE4Parse.UE4.Kismet;

public class EX_ObjectConst : KismetExpression<FPackageIndex>
{
	public override EExprToken Token => EExprToken.EX_ObjectConst;

	public EX_ObjectConst(FKismetArchive Ar)
	{
		Value = new FPackageIndex(Ar);
	}
}

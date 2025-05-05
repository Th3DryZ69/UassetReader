using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.UObject;

namespace CUE4Parse.UE4.Kismet;

public class EX_NameConst : KismetExpression<FName>
{
	public override EExprToken Token => EExprToken.EX_NameConst;

	public EX_NameConst(FKismetArchive Ar)
	{
		Value = Ar.ReadFName();
	}
}

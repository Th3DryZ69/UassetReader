using CUE4Parse.UE4.Assets.Readers;

namespace CUE4Parse.UE4.Kismet;

public class EX_SkipOffsetConst : KismetExpression<uint>
{
	public override EExprToken Token => EExprToken.EX_SkipOffsetConst;

	public EX_SkipOffsetConst(FKismetArchive Ar)
	{
		Value = Ar.Read<uint>();
	}
}

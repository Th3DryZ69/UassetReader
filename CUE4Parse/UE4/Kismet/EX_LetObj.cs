using CUE4Parse.UE4.Assets.Readers;

namespace CUE4Parse.UE4.Kismet;

public class EX_LetObj : EX_LetBase
{
	public override EExprToken Token => EExprToken.EX_LetObj;

	public EX_LetObj(FKismetArchive Ar)
		: base(Ar)
	{
	}
}

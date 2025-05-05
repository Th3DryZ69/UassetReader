using CUE4Parse.UE4.Assets.Readers;

namespace CUE4Parse.UE4.Kismet;

public class EX_LetDelegate : EX_LetBase
{
	public override EExprToken Token => EExprToken.EX_LetDelegate;

	public EX_LetDelegate(FKismetArchive Ar)
		: base(Ar)
	{
	}
}

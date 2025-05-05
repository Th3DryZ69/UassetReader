using CUE4Parse.UE4.Assets.Readers;

namespace CUE4Parse.UE4.Kismet;

public class EX_LetBool : EX_LetBase
{
	public override EExprToken Token => EExprToken.EX_LetBool;

	public EX_LetBool(FKismetArchive Ar)
		: base(Ar)
	{
	}
}

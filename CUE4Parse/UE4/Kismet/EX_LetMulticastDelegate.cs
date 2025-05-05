using CUE4Parse.UE4.Assets.Readers;

namespace CUE4Parse.UE4.Kismet;

public class EX_LetMulticastDelegate : EX_LetBase
{
	public override EExprToken Token => EExprToken.EX_LetMulticastDelegate;

	public EX_LetMulticastDelegate(FKismetArchive Ar)
		: base(Ar)
	{
	}
}

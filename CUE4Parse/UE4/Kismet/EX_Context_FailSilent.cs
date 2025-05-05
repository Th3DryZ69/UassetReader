using CUE4Parse.UE4.Assets.Readers;

namespace CUE4Parse.UE4.Kismet;

public class EX_Context_FailSilent : EX_Context
{
	public override EExprToken Token => EExprToken.EX_Context_FailSilent;

	public EX_Context_FailSilent(FKismetArchive Ar)
		: base(Ar)
	{
	}
}

using CUE4Parse.UE4.Assets.Readers;

namespace CUE4Parse.UE4.Kismet;

public class EX_ClassContext : EX_Context
{
	public override EExprToken Token => EExprToken.EX_ClassContext;

	public EX_ClassContext(FKismetArchive Ar)
		: base(Ar)
	{
	}
}

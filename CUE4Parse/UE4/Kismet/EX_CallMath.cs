using CUE4Parse.UE4.Assets.Readers;

namespace CUE4Parse.UE4.Kismet;

public class EX_CallMath : EX_FinalFunction
{
	public override EExprToken Token => EExprToken.EX_CallMath;

	public EX_CallMath(FKismetArchive Ar)
		: base(Ar)
	{
	}
}

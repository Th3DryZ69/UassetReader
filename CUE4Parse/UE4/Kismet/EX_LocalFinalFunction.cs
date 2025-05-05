using CUE4Parse.UE4.Assets.Readers;

namespace CUE4Parse.UE4.Kismet;

public class EX_LocalFinalFunction : EX_FinalFunction
{
	public override EExprToken Token => EExprToken.EX_LocalFinalFunction;

	public EX_LocalFinalFunction(FKismetArchive Ar)
		: base(Ar)
	{
	}
}

using CUE4Parse.UE4.Assets.Readers;

namespace CUE4Parse.UE4.Kismet;

public class EX_LocalVirtualFunction : EX_VirtualFunction
{
	public override EExprToken Token => EExprToken.EX_LocalVirtualFunction;

	public EX_LocalVirtualFunction(FKismetArchive Ar)
		: base(Ar)
	{
	}
}

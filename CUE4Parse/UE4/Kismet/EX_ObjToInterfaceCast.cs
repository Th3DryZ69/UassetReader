using CUE4Parse.UE4.Assets.Readers;

namespace CUE4Parse.UE4.Kismet;

public class EX_ObjToInterfaceCast : EX_CastBase
{
	public override EExprToken Token => EExprToken.EX_ObjToInterfaceCast;

	public EX_ObjToInterfaceCast(FKismetArchive Ar)
		: base(Ar)
	{
	}
}

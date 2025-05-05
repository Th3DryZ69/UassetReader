using CUE4Parse.UE4.Assets.Readers;

namespace CUE4Parse.UE4.Kismet;

public class EX_LetWeakObjPtr : EX_LetBase
{
	public override EExprToken Token => EExprToken.EX_LetWeakObjPtr;

	public EX_LetWeakObjPtr(FKismetArchive Ar)
		: base(Ar)
	{
	}
}

using CUE4Parse.UE4.Assets.Readers;

namespace CUE4Parse.UE4.Kismet;

public class EX_CrossInterfaceCast : EX_CastBase
{
	public override EExprToken Token => EExprToken.EX_CrossInterfaceCast;

	public EX_CrossInterfaceCast(FKismetArchive Ar)
		: base(Ar)
	{
	}
}

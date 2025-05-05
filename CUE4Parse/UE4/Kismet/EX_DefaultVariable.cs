using CUE4Parse.UE4.Assets.Readers;

namespace CUE4Parse.UE4.Kismet;

public class EX_DefaultVariable : EX_VariableBase
{
	public override EExprToken Token => EExprToken.EX_DefaultVariable;

	public EX_DefaultVariable(FKismetArchive Ar)
		: base(Ar)
	{
	}
}

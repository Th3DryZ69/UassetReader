using CUE4Parse.UE4.Assets.Readers;

namespace CUE4Parse.UE4.Kismet;

public class EX_LocalVariable : EX_VariableBase
{
	public override EExprToken Token => EExprToken.EX_LocalVariable;

	public EX_LocalVariable(FKismetArchive Ar)
		: base(Ar)
	{
	}
}

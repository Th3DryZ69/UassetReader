using CUE4Parse.UE4.Assets.Readers;

namespace CUE4Parse.UE4.Kismet;

public class EX_InstanceVariable : EX_VariableBase
{
	public override EExprToken Token => EExprToken.EX_InstanceVariable;

	public EX_InstanceVariable(FKismetArchive Ar)
		: base(Ar)
	{
	}
}

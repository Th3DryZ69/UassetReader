using CUE4Parse.UE4.Assets.Readers;

namespace CUE4Parse.UE4.Kismet;

public class EX_LocalOutVariable : EX_VariableBase
{
	public override EExprToken Token => EExprToken.EX_LocalOutVariable;

	public EX_LocalOutVariable(FKismetArchive Ar)
		: base(Ar)
	{
	}
}

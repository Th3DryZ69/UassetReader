using CUE4Parse.UE4.Assets.Readers;

namespace CUE4Parse.UE4.Kismet;

public class EX_ClassSparseDataVariable : EX_VariableBase
{
	public override EExprToken Token => EExprToken.EX_ClassSparseDataVariable;

	public EX_ClassSparseDataVariable(FKismetArchive Ar)
		: base(Ar)
	{
	}
}

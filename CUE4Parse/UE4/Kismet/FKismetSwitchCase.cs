using CUE4Parse.UE4.Assets.Readers;

namespace CUE4Parse.UE4.Kismet;

public struct FKismetSwitchCase
{
	public KismetExpression CaseIndexValueTerm;

	public uint NextOffset;

	public KismetExpression CaseTerm;

	public FKismetSwitchCase(FKismetArchive Ar)
	{
		CaseIndexValueTerm = Ar.ReadExpression();
		NextOffset = Ar.Read<uint>();
		CaseTerm = Ar.ReadExpression();
	}
}

using CUE4Parse.UE4.Assets.Readers;

namespace CUE4Parse.UE4.Kismet;

public class EX_ByteConst : KismetExpression<byte>
{
	public override EExprToken Token => EExprToken.EX_ByteConst;

	public EX_ByteConst(FKismetArchive Ar)
	{
		Value = Ar.Read<byte>();
	}
}

using CUE4Parse.UE4.Assets.Readers;

namespace CUE4Parse.UE4.Kismet;

public class EX_IntConstByte : KismetExpression<byte>
{
	public override EExprToken Token => EExprToken.EX_IntConstByte;

	public EX_IntConstByte(FKismetArchive Ar)
	{
		Value = Ar.Read<byte>();
	}
}

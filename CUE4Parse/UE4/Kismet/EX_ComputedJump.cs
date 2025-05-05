using CUE4Parse.UE4.Assets.Readers;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Kismet;

public class EX_ComputedJump : KismetExpression
{
	public KismetExpression CodeOffsetExpression;

	public override EExprToken Token => EExprToken.EX_ComputedJump;

	public EX_ComputedJump(FKismetArchive Ar)
	{
		CodeOffsetExpression = Ar.ReadExpression();
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer, bool bAddIndex = false)
	{
		base.WriteJson(writer, serializer, bAddIndex);
		writer.WritePropertyName("OffsetExpression");
		serializer.Serialize(writer, CodeOffsetExpression);
	}
}

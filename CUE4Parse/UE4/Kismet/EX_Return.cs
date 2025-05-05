using CUE4Parse.UE4.Assets.Readers;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Kismet;

public class EX_Return : KismetExpression
{
	public KismetExpression ReturnExpression;

	public override EExprToken Token => EExprToken.EX_Return;

	public EX_Return(FKismetArchive Ar)
	{
		ReturnExpression = Ar.ReadExpression();
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer, bool bAddIndex = false)
	{
		base.WriteJson(writer, serializer, bAddIndex);
		writer.WritePropertyName("Expression");
		serializer.Serialize(writer, ReturnExpression);
	}
}

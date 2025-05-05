using CUE4Parse.UE4.Assets.Readers;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Kismet;

public class EX_JumpIfNot : EX_Jump
{
	public KismetExpression BooleanExpression;

	public override EExprToken Token => EExprToken.EX_JumpIfNot;

	public EX_JumpIfNot(FKismetArchive Ar)
		: base(Ar)
	{
		BooleanExpression = Ar.ReadExpression();
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer, bool bAddIndex = false)
	{
		base.WriteJson(writer, serializer, bAddIndex);
		writer.WritePropertyName("BooleanExpression");
		serializer.Serialize(writer, BooleanExpression);
	}
}

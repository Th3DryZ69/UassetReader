using CUE4Parse.UE4.Assets.Readers;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Kismet;

public class EX_Skip : EX_Jump
{
	public KismetExpression SkipExpression;

	public override EExprToken Token => EExprToken.EX_Skip;

	public EX_Skip(FKismetArchive Ar)
		: base(Ar)
	{
		CodeOffset = Ar.Read<uint>();
		SkipExpression = Ar.ReadExpression();
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer, bool bAddIndex = false)
	{
		base.WriteJson(writer, serializer, bAddIndex);
		writer.WritePropertyName("SkipExpression");
		serializer.Serialize(writer, SkipExpression);
	}
}

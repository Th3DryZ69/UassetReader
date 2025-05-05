using CUE4Parse.UE4.Assets.Readers;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Kismet;

public class EX_InterfaceContext : KismetExpression
{
	public KismetExpression InterfaceValue;

	public override EExprToken Token => EExprToken.EX_InterfaceContext;

	public EX_InterfaceContext(FKismetArchive Ar)
	{
		InterfaceValue = Ar.ReadExpression();
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer, bool bAddIndex = false)
	{
		base.WriteJson(writer, serializer, bAddIndex);
		writer.WritePropertyName("InterfaceValue");
		serializer.Serialize(writer, InterfaceValue);
	}
}

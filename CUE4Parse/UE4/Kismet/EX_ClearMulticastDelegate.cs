using CUE4Parse.UE4.Assets.Readers;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Kismet;

public class EX_ClearMulticastDelegate : KismetExpression
{
	public KismetExpression DelegateToClear;

	public override EExprToken Token => EExprToken.EX_ClearMulticastDelegate;

	public EX_ClearMulticastDelegate(FKismetArchive Ar)
	{
		DelegateToClear = Ar.ReadExpression();
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer, bool bAddIndex = false)
	{
		base.WriteJson(writer, serializer, bAddIndex);
		writer.WritePropertyName("DelegateToClear");
		serializer.Serialize(writer, DelegateToClear);
	}
}

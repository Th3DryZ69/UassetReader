using CUE4Parse.UE4.Assets.Readers;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Kismet;

public class EX_RemoveMulticastDelegate : KismetExpression
{
	public KismetExpression Delegate;

	public KismetExpression DelegateToAdd;

	public override EExprToken Token => EExprToken.EX_RemoveMulticastDelegate;

	public EX_RemoveMulticastDelegate(FKismetArchive Ar)
	{
		Delegate = Ar.ReadExpression();
		DelegateToAdd = Ar.ReadExpression();
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer, bool bAddIndex = false)
	{
		base.WriteJson(writer, serializer, bAddIndex);
		writer.WritePropertyName("MulticastDelegate");
		serializer.Serialize(writer, Delegate);
		writer.WritePropertyName("Delegate");
		serializer.Serialize(writer, DelegateToAdd);
	}
}

using CUE4Parse.UE4.Assets.Readers;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Kismet;

public class EX_LetValueOnPersistentFrame : KismetExpression
{
	public FKismetPropertyPointer DestinationProperty;

	public KismetExpression AssignmentExpression;

	public override EExprToken Token => EExprToken.EX_LetValueOnPersistentFrame;

	public EX_LetValueOnPersistentFrame(FKismetArchive Ar)
	{
		DestinationProperty = new FKismetPropertyPointer(Ar);
		AssignmentExpression = Ar.ReadExpression();
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer, bool bAddIndex = false)
	{
		base.WriteJson(writer, serializer, bAddIndex);
		writer.WritePropertyName("DestinationProperty");
		serializer.Serialize(writer, DestinationProperty);
		writer.WritePropertyName("AssignmentExpression");
		serializer.Serialize(writer, AssignmentExpression);
	}
}

using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.UObject;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Kismet;

public class EX_InstrumentationEvent : KismetExpression
{
	public EScriptInstrumentationType EventType;

	public FName? EventName;

	public override EExprToken Token => EExprToken.EX_InstrumentationEvent;

	public EX_InstrumentationEvent(FKismetArchive Ar)
	{
		EventType = (EScriptInstrumentationType)Ar.Read<byte>();
		if (EventType.Equals(EScriptInstrumentationType.InlineEvent))
		{
			EventName = Ar.ReadFName();
		}
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer, bool bAddIndex = false)
	{
		base.WriteJson(writer, serializer, bAddIndex);
		if (EventType.Equals(EScriptInstrumentationType.InlineEvent))
		{
			writer.WritePropertyName("EventName");
			serializer.Serialize(writer, EventName);
		}
	}
}

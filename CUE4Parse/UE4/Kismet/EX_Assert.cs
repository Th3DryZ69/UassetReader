using CUE4Parse.UE4.Assets.Readers;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Kismet;

public class EX_Assert : KismetExpression
{
	public ushort LineNumber;

	public bool DebugMode;

	public KismetExpression AssertExpression;

	public override EExprToken Token => EExprToken.EX_Assert;

	public EX_Assert(FKismetArchive Ar)
	{
		LineNumber = Ar.Read<ushort>();
		DebugMode = Ar.ReadFlag();
		AssertExpression = Ar.ReadExpression();
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer, bool bAddIndex = false)
	{
		base.WriteJson(writer, serializer, bAddIndex);
		writer.WritePropertyName("LineNumber");
		writer.WriteValue(LineNumber);
		writer.WritePropertyName("DebugMode");
		writer.WriteValue(DebugMode);
		writer.WritePropertyName("AssertExpression");
		serializer.Serialize(writer, AssertExpression);
	}
}

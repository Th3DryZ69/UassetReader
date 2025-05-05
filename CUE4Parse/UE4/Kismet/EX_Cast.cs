using CUE4Parse.UE4.Assets.Readers;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Kismet;

public class EX_Cast : KismetExpression
{
	public ECastToken ConversionType;

	public KismetExpression Target;

	public override EExprToken Token => EExprToken.EX_Cast;

	public EX_Cast(FKismetArchive Ar)
	{
		ConversionType = (ECastToken)Ar.Read<byte>();
		Target = Ar.ReadExpression();
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer, bool bAddIndex = false)
	{
		base.WriteJson(writer, serializer, bAddIndex);
		writer.WritePropertyName("ConversionType");
		writer.WriteValue(ConversionType.ToString());
		writer.WritePropertyName("Target");
		serializer.Serialize(writer, Target);
	}
}

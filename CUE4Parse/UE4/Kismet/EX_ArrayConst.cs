using CUE4Parse.UE4.Assets.Readers;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Kismet;

public class EX_ArrayConst : KismetExpression
{
	public FKismetPropertyPointer InnerProperty;

	public KismetExpression[] Elements;

	public override EExprToken Token => EExprToken.EX_ArrayConst;

	public EX_ArrayConst(FKismetArchive Ar)
	{
		InnerProperty = new FKismetPropertyPointer(Ar);
		Ar.Read<int>();
		Elements = Ar.ReadExpressionArray(EExprToken.EX_EndArrayConst);
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer, bool bAddIndex = false)
	{
		base.WriteJson(writer, serializer, bAddIndex);
		writer.WritePropertyName("InnerProperty");
		serializer.Serialize(writer, InnerProperty);
		writer.WritePropertyName("Values");
		serializer.Serialize(writer, Elements);
	}
}

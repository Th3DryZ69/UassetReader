using CUE4Parse.UE4.Assets.Readers;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Kismet;

public class EX_SetSet : KismetExpression
{
	public KismetExpression SetProperty;

	public KismetExpression[] Elements;

	public override EExprToken Token => EExprToken.EX_SetSet;

	public EX_SetSet(FKismetArchive Ar)
	{
		SetProperty = Ar.ReadExpression();
		Ar.Read<int>();
		Elements = Ar.ReadExpressionArray(EExprToken.EX_EndSet);
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer, bool bAddIndex = false)
	{
		base.WriteJson(writer, serializer, bAddIndex);
		writer.WritePropertyName("SetProperty");
		serializer.Serialize(writer, SetProperty);
		writer.WritePropertyName("Elements");
		serializer.Serialize(writer, Elements);
	}
}

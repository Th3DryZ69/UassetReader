using CUE4Parse.UE4.Assets.Readers;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Kismet;

public class EX_MapConst : KismetExpression
{
	public FKismetPropertyPointer KeyProperty;

	public FKismetPropertyPointer ValueProperty;

	public KismetExpression[] Elements;

	public override EExprToken Token => EExprToken.EX_MapConst;

	public EX_MapConst(FKismetArchive Ar)
	{
		KeyProperty = new FKismetPropertyPointer(Ar);
		ValueProperty = new FKismetPropertyPointer(Ar);
		Ar.Read<int>();
		Elements = Ar.ReadExpressionArray(EExprToken.EX_EndMapConst);
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer, bool bAddIndex = false)
	{
		base.WriteJson(writer, serializer, bAddIndex);
		writer.WritePropertyName("KeyProperty");
		serializer.Serialize(writer, KeyProperty);
		writer.WritePropertyName("ValueProperty");
		serializer.Serialize(writer, ValueProperty);
		writer.WritePropertyName("Values");
		writer.WriteStartArray();
		for (int i = 1; i <= Elements.Length / 2; i++)
		{
			writer.WriteStartObject();
			writer.WritePropertyName("Key");
			serializer.Serialize(writer, Elements[2 * (i - 1)]);
			writer.WritePropertyName("Value");
			serializer.Serialize(writer, Elements[2 * (i - 1) + 1]);
			writer.WriteEndObject();
		}
		writer.WriteEndArray();
	}
}

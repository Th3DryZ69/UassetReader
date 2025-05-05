using CUE4Parse.UE4.Assets.Readers;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Kismet;

public class EX_SetMap : KismetExpression
{
	public KismetExpression MapProperty;

	public KismetExpression[] Elements;

	public override EExprToken Token => EExprToken.EX_SetMap;

	public EX_SetMap(FKismetArchive Ar)
	{
		MapProperty = Ar.ReadExpression();
		Ar.Read<int>();
		Elements = Ar.ReadExpressionArray(EExprToken.EX_EndMap);
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer, bool bAddIndex = false)
	{
		base.WriteJson(writer, serializer, bAddIndex);
		writer.WritePropertyName("MapProperty");
		serializer.Serialize(writer, MapProperty);
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

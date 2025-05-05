using CUE4Parse.UE4.Assets.Readers;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Kismet;

public abstract class EX_LetBase : KismetExpression
{
	public KismetExpression Variable;

	public KismetExpression Assignment;

	public EX_LetBase(FKismetArchive Ar)
	{
		Variable = Ar.ReadExpression();
		Assignment = Ar.ReadExpression();
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer, bool bAddIndex = false)
	{
		base.WriteJson(writer, serializer, bAddIndex);
		writer.WritePropertyName("Variable");
		serializer.Serialize(writer, Variable);
		writer.WritePropertyName("Expression");
		serializer.Serialize(writer, Assignment);
	}
}

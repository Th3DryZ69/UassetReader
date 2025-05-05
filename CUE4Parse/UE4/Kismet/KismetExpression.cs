using Newtonsoft.Json;

namespace CUE4Parse.UE4.Kismet;

[JsonConverter(typeof(KismetExpressionConverter))]
public abstract class KismetExpression
{
	public int StatementIndex;

	public virtual EExprToken Token => EExprToken.EX_Nothing;

	protected internal virtual void WriteJson(JsonWriter writer, JsonSerializer serializer, bool bAddIndex = false)
	{
		writer.WritePropertyName("Inst");
		writer.WriteValue(Token.ToString());
		if (bAddIndex)
		{
			writer.WritePropertyName("StatementIndex");
			writer.WriteValue(StatementIndex);
		}
	}
}
public abstract class KismetExpression<T> : KismetExpression
{
	public T Value;

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer, bool bAddIndex = false)
	{
		base.WriteJson(writer, serializer, bAddIndex);
		writer.WritePropertyName("Value");
		serializer.Serialize(writer, Value);
	}
}

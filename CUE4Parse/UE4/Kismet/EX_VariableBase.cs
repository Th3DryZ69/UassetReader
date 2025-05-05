using CUE4Parse.UE4.Assets.Readers;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Kismet;

public abstract class EX_VariableBase : KismetExpression
{
	public FKismetPropertyPointer Variable;

	public EX_VariableBase(FKismetArchive Ar)
	{
		Variable = new FKismetPropertyPointer(Ar);
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer, bool bAddIndex = false)
	{
		base.WriteJson(writer, serializer, bAddIndex);
		writer.WritePropertyName("Variable");
		serializer.Serialize(writer, Variable);
	}
}

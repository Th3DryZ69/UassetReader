using CUE4Parse.UE4.Assets.Readers;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Kismet;

public class EX_ArrayGetByRef : KismetExpression
{
	public KismetExpression ArrayVariable;

	public KismetExpression ArrayIndex;

	public override EExprToken Token => EExprToken.EX_ArrayGetByRef;

	public EX_ArrayGetByRef(FKismetArchive Ar)
	{
		ArrayVariable = Ar.ReadExpression();
		ArrayIndex = Ar.ReadExpression();
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer, bool bAddIndex = false)
	{
		base.WriteJson(writer, serializer, bAddIndex);
		writer.WritePropertyName("ArrayVariable");
		serializer.Serialize(writer, ArrayVariable);
		writer.WritePropertyName("ArrayIndex");
		serializer.Serialize(writer, ArrayIndex);
	}
}

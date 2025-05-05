using CUE4Parse.UE4.Assets.Readers;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Kismet;

public class EX_PropertyConst : KismetExpression
{
	public FKismetPropertyPointer Property;

	public override EExprToken Token => EExprToken.EX_PropertyConst;

	public EX_PropertyConst(FKismetArchive Ar)
	{
		Property = new FKismetPropertyPointer(Ar);
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer, bool bAddIndex = false)
	{
		base.WriteJson(writer, serializer, bAddIndex);
		writer.WritePropertyName("Property");
		serializer.Serialize(writer, Property);
	}
}

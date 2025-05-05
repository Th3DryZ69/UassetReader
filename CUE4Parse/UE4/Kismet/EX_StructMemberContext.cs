using CUE4Parse.UE4.Assets.Readers;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Kismet;

public class EX_StructMemberContext : KismetExpression
{
	public FKismetPropertyPointer Property;

	public KismetExpression StructExpression;

	public override EExprToken Token => EExprToken.EX_StructMemberContext;

	public EX_StructMemberContext(FKismetArchive Ar)
	{
		Property = new FKismetPropertyPointer(Ar);
		StructExpression = Ar.ReadExpression();
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer, bool bAddIndex = false)
	{
		base.WriteJson(writer, serializer, bAddIndex);
		writer.WritePropertyName("Property");
		serializer.Serialize(writer, Property);
		writer.WritePropertyName("StructExpression");
		serializer.Serialize(writer, StructExpression);
	}
}

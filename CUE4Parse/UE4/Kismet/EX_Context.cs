using CUE4Parse.UE4.Assets.Readers;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Kismet;

public class EX_Context : KismetExpression
{
	public KismetExpression ObjectExpression;

	public uint Offset;

	public FKismetPropertyPointer RValuePointer;

	public KismetExpression ContextExpression;

	public override EExprToken Token => EExprToken.EX_Context;

	public EX_Context(FKismetArchive Ar)
	{
		ObjectExpression = Ar.ReadExpression();
		Offset = Ar.Read<uint>();
		RValuePointer = new FKismetPropertyPointer(Ar);
		ContextExpression = Ar.ReadExpression();
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer, bool bAddIndex = false)
	{
		base.WriteJson(writer, serializer, bAddIndex);
		writer.WritePropertyName("ObjectExpression");
		serializer.Serialize(writer, ObjectExpression);
		writer.WritePropertyName("Offset");
		writer.WriteValue(Offset);
		writer.WritePropertyName("RValuePointer");
		serializer.Serialize(writer, RValuePointer);
		writer.WritePropertyName("ContextExpression");
		serializer.Serialize(writer, ContextExpression);
	}
}

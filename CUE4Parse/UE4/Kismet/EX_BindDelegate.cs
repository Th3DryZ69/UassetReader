using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.UObject;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Kismet;

public class EX_BindDelegate : KismetExpression
{
	public FName FunctionName;

	public KismetExpression Delegate;

	public KismetExpression ObjectTerm;

	public override EExprToken Token => EExprToken.EX_BindDelegate;

	public EX_BindDelegate(FKismetArchive Ar)
	{
		FunctionName = Ar.ReadFName();
		Delegate = Ar.ReadExpression();
		ObjectTerm = Ar.ReadExpression();
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer, bool bAddIndex = false)
	{
		base.WriteJson(writer, serializer, bAddIndex);
		writer.WritePropertyName("FunctionName");
		serializer.Serialize(writer, FunctionName);
		writer.WritePropertyName("Delegate");
		serializer.Serialize(writer, Delegate);
		writer.WritePropertyName("ObjectTerm");
		serializer.Serialize(writer, ObjectTerm);
	}
}

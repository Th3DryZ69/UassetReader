using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.UObject;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Kismet;

public class EX_CallMulticastDelegate : KismetExpression
{
	public FPackageIndex StackNode;

	public KismetExpression Delegate;

	public KismetExpression[] Parameters;

	public override EExprToken Token => EExprToken.EX_CallMulticastDelegate;

	public EX_CallMulticastDelegate(FKismetArchive Ar)
	{
		StackNode = new FPackageIndex(Ar);
		Delegate = Ar.ReadExpression();
		Parameters = Ar.ReadExpressionArray(EExprToken.EX_EndFunctionParms);
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer, bool bAddIndex = false)
	{
		base.WriteJson(writer, serializer, bAddIndex);
		writer.WritePropertyName("FunctionName");
		serializer.Serialize(writer, StackNode);
		writer.WritePropertyName("Delegate");
		serializer.Serialize(writer, Delegate);
		writer.WritePropertyName("Parameters");
		serializer.Serialize(writer, Parameters);
	}
}

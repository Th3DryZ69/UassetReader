using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.UObject;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Kismet;

public class EX_VirtualFunction : KismetExpression
{
	public FName VirtualFunctionName;

	public KismetExpression[] Parameters;

	public override EExprToken Token => EExprToken.EX_VirtualFunction;

	public EX_VirtualFunction(FKismetArchive Ar)
	{
		VirtualFunctionName = Ar.ReadFName();
		Parameters = Ar.ReadExpressionArray(EExprToken.EX_EndFunctionParms);
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer, bool bAddIndex = false)
	{
		base.WriteJson(writer, serializer, bAddIndex);
		writer.WritePropertyName("Function");
		serializer.Serialize(writer, VirtualFunctionName);
		writer.WritePropertyName("Parameters");
		serializer.Serialize(writer, Parameters);
	}
}

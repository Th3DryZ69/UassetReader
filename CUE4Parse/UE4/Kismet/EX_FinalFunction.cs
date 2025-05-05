using System.Text;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.UObject;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Kismet;

public class EX_FinalFunction : KismetExpression
{
	public FPackageIndex StackNode;

	public KismetExpression[] Parameters;

	public override EExprToken Token => EExprToken.EX_FinalFunction;

	public EX_FinalFunction(FKismetArchive Ar)
	{
		StackNode = new FPackageIndex(Ar);
		Parameters = Ar.ReadExpressionArray(EExprToken.EX_EndFunctionParms);
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer, bool bAddIndex = false)
	{
		base.WriteJson(writer, serializer, bAddIndex);
		writer.WritePropertyName("Function");
		serializer.Serialize(writer, StackNode);
		writer.WritePropertyName("Parameters");
		serializer.Serialize(writer, Parameters);
		if (Parameters.Length == 1 && Parameters[0] is EX_IntConst eX_IntConst && StackNode.ResolvedObject != null && StackNode.ResolvedObject.Class?.Name.Text == "Function")
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(StackNode.Owner);
			stringBuilder.Append('.');
			stringBuilder.Append(StackNode.Name);
			stringBuilder.Append('[');
			stringBuilder.Append(eX_IntConst.Value);
			stringBuilder.Append(']');
			writer.WritePropertyName("ObjectPath");
			writer.WriteValue(stringBuilder.ToString());
		}
	}
}

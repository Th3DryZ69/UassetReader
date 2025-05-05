using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.UObject;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Kismet;

public class EX_InstanceDelegate : KismetExpression
{
	public FName FunctionName;

	public override EExprToken Token => EExprToken.EX_InstanceDelegate;

	public EX_InstanceDelegate(FKismetArchive Ar)
	{
		FunctionName = Ar.ReadFName();
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer, bool bAddIndex = false)
	{
		base.WriteJson(writer, serializer, bAddIndex);
		writer.WritePropertyName("FunctionName");
		serializer.Serialize(writer, FunctionName);
	}
}

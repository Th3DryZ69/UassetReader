using System.Text;
using CUE4Parse.UE4.Assets.Readers;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Kismet;

public class EX_PushExecutionFlow : KismetExpression
{
	public uint PushingAddress;

	public StringBuilder ObjectPath = new StringBuilder();

	public override EExprToken Token => EExprToken.EX_PushExecutionFlow;

	public EX_PushExecutionFlow(FKismetArchive Ar)
	{
		PushingAddress = Ar.Read<uint>();
		ObjectPath.Append(Ar.Owner.Name);
		ObjectPath.Append('.');
		ObjectPath.Append(Ar.Name);
		ObjectPath.Append('[');
		ObjectPath.Append(PushingAddress);
		ObjectPath.Append(']');
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer, bool bAddIndex = false)
	{
		base.WriteJson(writer, serializer, bAddIndex);
		writer.WritePropertyName("PushingAddress");
		writer.WriteValue(PushingAddress);
		writer.WritePropertyName("ObjectPath");
		writer.WriteValue(ObjectPath.ToString());
	}
}
